using Azure;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.Dtos.Wpf.Business.WorkFlowLog;
using REAC.Application.Contracts.Dtos.Wpf.System;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Enum.Wpf;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using REAC.EntityFrameworkCore.Entities.Wpf.System;
using static Dapper.SqlMapper;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> ProcessWorkFlow(ProcessRequestPayload requestPayload, CancellationToken cancellationToken)
        {
            ResponseWorkFlowDto responseWorkFlowDto = new ResponseWorkFlowDto();
            try
            {
                var processingLog = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())?
                        .Where(x => x.Id == requestPayload.ProcessingLogId)
                        .Include(x => x.Employee)
                        .Include(x => x.WorkFlowStep)
                        .ThenInclude(x => x.WorkFlowStepEvents
                        .Where(x => x.Id == requestPayload.WorkflowStepEventId))
                        .Single();

                #region Validate Employee
                if (requestPayload.EmployeeId.HasValue)
                {
                    var employee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                        .Where(x => x.Id == requestPayload.EmployeeId).Single();

                    if (employee == null)
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "Employee Not Found"
                        };

                    if (requestPayload.EmployeeId != processingLog?.Employee?.Id)
                    {
                        if (!employee.IsAdmin)
                            return new BaseResponse()
                            {
                                IsDone = false,
                                Message = "Not Allowed"
                            };

                        if (!employee.WorkAsEmployee)
                            return new BaseResponse()
                            {
                                IsDone = false,
                                Message = "Not Allowed"
                            };
                    }
                }
                #endregion

                #region Validate processingLog
                if (processingLog == null)
                    return new BaseResponse() { IsDone = false, Message = "process log not found" };

                if (processingLog.IsFinished)
                    return new BaseResponse() { IsDone = false, Message = "Not Allowed" };
                #endregion

                #region Update processingLog
                processingLog.IsFinished = true;
                processingLog.FinishedOn = DateTime.Now;
                processingLog.ShowForApplicant = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ShowProcessingLogForApplicant;
                processingLog.WorkFlowStepEventId = requestPayload.WorkflowStepEventId;
                #endregion

                #region AllowChangeExternalStatus
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowChangeExternalStatus)
                    responseWorkFlowDto.ExternalStatus = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ExternalStatusId;
                #endregion

                #region AllowChangeInternalStatus
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowChangeInternalStatus)
                    responseWorkFlowDto.InternalStatus = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().InternalStatusId;
                #endregion

                #region AllowChangeIsFinished
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowChangeIsFinished)
                    responseWorkFlowDto.IsFinished = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().IsFinishedValue;
                #endregion

                #region AllowChangeIsActive
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowChangeIsActive)
                    responseWorkFlowDto.IsActive = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().IsActiveValue;
                #endregion

                #region AllowNotes 
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowNotes)
                    processingLog.Notes = requestPayload.Notes;
                #endregion

                #region AllowFileUpload
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowFileUpload)
                {
                    requestPayload.UploadedFilesIds?.ForEach(fileId =>
                    {
                        processingLog.WorkFlowProcessingLogFiles.Add(new WorkFlowProcessingLogFile()
                        {
                            Id = Guid.NewGuid(),
                            FileId = fileId
                        });
                    });
                }
                #endregion

                #region AllowCompletionNotificationPlan 
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowCompletionNotificationPlan)
                    await ExecuteNotificationPlan(processingLog.WorkFlowStep.WorkFlowStepEvents.Single().CompletionNotificationPlanId.Value);
                #endregion

                #region Create Log
                WorkFlowLogCreateUpdateDto workFlowLog = new WorkFlowLogCreateUpdateDto()
                {
                    CreatedOn = DateTime.UtcNow,
                    ShowForApplicant = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ShowLogForApplicant,
                    LogMessage = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().LogMessage,
                    RequestId = requestPayload.RequestId
                };
                processingLog.WorkFlowStep.WorkFlowStepEvents.Single().Translations?.ToList().ForEach(x =>
                {
                    workFlowLog.Translations.Add(new WorkFlowLogTranslation()
                    {
                        LanguageCode = x.LanguageCode,
                        LogMessage = x.LogMessage
                    });
                });
                var workFlowLogResponse = await _workFlowLogService.AddAsync(workFlowLog);
                if (!workFlowLogResponse.IsDone)
                    return workFlowLogResponse;
                #endregion

                #region EventActionMode
                switch (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ActionModeId)
                {
                    case EventActionMode.GoPreviousStep:
                        var previoudProcessingLog = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())?
                                           .Where(x => x.Id == processingLog.SourceWorkFlowProcessingLogId)
                                           .Single();

                        if (previoudProcessingLog != null)
                        {
                            var newProcessingLog = new WorkFlowProcessingLog()
                            {
                                CreatedOn = DateTime.Now,
                                IsFinished = false,
                                EmployeeReferenceId = previoudProcessingLog.EmployeeReferenceId,
                                RequestId = previoudProcessingLog.RequestId,
                                SourceWorkFlowProcessingLogId = processingLog.Id,
                                WorkFlowStepId = previoudProcessingLog.WorkFlowStepId
                            };

                            var newProcessingLogResponse = await _workFlowProcessingLogService.AddAsync(newProcessingLog, cancellationToken);
                            if (!newProcessingLogResponse.IsDone)
                                return newProcessingLogResponse;
                        }
                        break;
                    case EventActionMode.GoNextStep:
                        await ExecuteWorkFlowSteps(processingLog.WorkFlowStep.WorkFlowId,
                                                   requestPayload.RequestId,
                                                   processingLog.Id,
                                                   processingLog.WorkFlowStep.Order + 1,
                                                   processingLog.WorkFlowStep.WorkFlowStepEvents.Single().SameEmployee,
                                                   cancellationToken);
                        break;
                    case EventActionMode.SpecificStep:
                        var specificworkFlowStep = (await _repository.GetQueryable<WorkFlowStep, Guid>())?
                                                   .Where(x => x.Id == processingLog.WorkFlowStep.WorkFlowStepEvents.Single().NextWorkFlowStepDestinationId)
                                                   .Include(x => x.Department)
                                                   .ThenInclude(x => x.SysEmployees
                                                   .OrderBy(x => x.WorkFlowProcessingLogs
                                                   .Count(x => x.IsFinished))
                                                   .ThenBy(x => x.IsAdmin))
                                                   .Single();

                        await ExecuteWorkFlowStep(specificworkFlowStep,
                                                  requestPayload.RequestId,
                                                  processingLog.Id,
                                                  processingLog.WorkFlowStep.WorkFlowStepEvents.Single().SameEmployee,
                                                  cancellationToken);
                        break;
                }
                #endregion

                var response = await _workFlowProcessingLogService.UpdateAsync(processingLog);
                if (!response.IsDone)
                    return response;

                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;           
             
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };

            }

            await _litigationWorkFlowService.UpdateLitigationStatusAsync(responseWorkFlowDto, requestPayload.RequestId);

            return new DataResponse<ResponseWorkFlowDto>()
            {
                IsDone = true,
                Data = responseWorkFlowDto
            };
        }
    }
}
