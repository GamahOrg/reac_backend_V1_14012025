using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.Dtos.Wpf.Business.WorkFlowLog;
using REAC.Application.Contracts.Dtos.Wpf.System;
using REAC.Domain.Shared.Enum;
using REAC.Domain.Shared.Enum.Wpf;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using REAC.EntityFrameworkCore.Entities.Wpf.System;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> ProcessWorkFlow(ProcessRequestPayload requestPayload, CancellationToken cancellationToken)
        {
            ResponseWorkFlowDto responseWorkFlowDto = new ResponseWorkFlowDto();
            try
            {
                Guid employeeId = Guid.Empty;
                _jwtReader.GetReferenceId(ref employeeId);

                var processingLog = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())?
                        .Where(x => x.Id == requestPayload.ProcessingLogId)
                        .Include(x => x.Employee)
                        .Include(x => x.WorkFlowStep)
                        .ThenInclude(x => x.WorkFlowStepEvents
                        .Where(x => x.Id == requestPayload.WorkflowStepEventId))
                        .Single();


                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                    .Include(x => x.Contract)
                        .Where(x => x.Id == requestPayload.RequestId)
                        .Single();

                #region Validate Employee

                //var employee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                //    .Where(x => x.Id == employeeId).Single();

                //if (employee == null)
                //    return new BaseResponse()
                //    {
                //        IsDone = false,
                //        Message = "Employee Not Found"
                //    };


                //if (employeeId != processingLog?.Employee?.Id)
                //{
                //    if (!employee.IsAdmin)
                //        return new BaseResponse()
                //        {
                //            IsDone = false,
                //            Message = "Not Allowed"
                //        };

                //    if (!employee.WorkAsEmployee)
                //        return new BaseResponse()
                //        {
                //            IsDone = false,
                //            Message = "Not Allowed"
                //        };
                //}

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
                {
                    responseWorkFlowDto.ExternalStatus = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ExternalStatusId;
                    litigationRequest.ExternalStatusId = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ExternalStatusId.Value;
                }
                #endregion

                #region AllowChangeInternalStatus
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().AllowChangeInternalStatus)
                {
                    responseWorkFlowDto.InternalStatus = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().InternalStatusId;
                    litigationRequest.InternalStatusId = processingLog.WorkFlowStep.WorkFlowStepEvents.Single().InternalStatusId.Value;

                }
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
                {
                    if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().CompletionNotificationPlanId.HasValue)
                        await ExecuteNotificationPlan(processingLog.WorkFlowStep.WorkFlowStepEvents.Single().CompletionNotificationPlanId.Value);
                }
                #endregion

                #region Create Log
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().LogMessage != null)
                {
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
                }
                #endregion

                #region EventActionMode
                if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().ActionModeId.HasValue)
                {
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
                                    EmployeeId = previoudProcessingLog.EmployeeId,
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
                            if (processingLog.WorkFlowStep.WorkFlowStepEvents.Single().NextWorkFlowStepDestinationId.HasValue)
                            {
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
                            }
                            break;
                    }
                }
                #endregion



                switch (litigationRequest.InternalStatusId)
                {
                    case RequestStatus.Rejected:
                        // Send Notification
                        // تم رفض الطلب
                        break;
                    case RequestStatus.SentBackForEdit:
                        // Send Notification
                        // تم إعادة الطلب لوجود بعض الملاحظات
                        break;
                    case RequestStatus.Received:

                        switch (litigationRequest?.LitigationRequestCategoryId)
                        {
                            case REAC.Domain.Shared.Enum.LitigationRequestCategory.Arbitration:
                                litigationRequest.LitigationRequestStage = LitigationRequestStage.Arbitration;
                                litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.ArbitratorSelectionDuration;
                                break;
                            default:
                                litigationRequest.LitigationRequestStage = LitigationRequestStage.SetReformer;
                                litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.SetReformerFirstTime;
                                break;
                        }

                        var response = _repository.Update<LitigationRequest, Guid>(litigationRequest);
                        if (!response.IsDone)
                            return response;

                        var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;

                        switch (litigationRequest.LitigationRequestCategoryId)
                        {
                            case LitigationRequestCategory.Arbitration:
                                break;
                            default:
                                var department = (await _repository.GetQueryable<SysDepartment, Guid>())?
                                   .Where(x => x.DepartmentType == DepartmentType.Reconciliation)
                                   .SingleOrDefault();
                                if (department != null)
                                {
                                    switch (department.DistributionMode)
                                    {
                                        case DepartmentDistributionMode.AutoDistribution:
                                            var reformers = (await _repository.GetQueryable<Citizen, Guid>())?
                                                            .Where(x => x.IsReformer == true & x.ReformerStatusId == ReformerStatus.Certified
                                                            & x.Id != litigationRequest.ApplicantCitizenId
                                                            & x.Id != litigationRequest.Contract.FirstPartyCitizenId
                                                            & x.Id != litigationRequest.Contract.SecondPartyCitizenId)

                                                            .OrderBy(x => x.ReformerLitigationRequests
                                                            .Count(t => t.IsFinished == false) + x.LitigationRequestArbitratorLists.Count(x => x.LitigationRequest.IsFinished == false))
                                                            .ToList();

                                            if (reformers != null)
                                            {
                                                if (reformers.Any())
                                                {
                                                    LitigationRequestReformerAssigningRequest litigationRequestReformerAssigningRequest = new LitigationRequestReformerAssigningRequest();
                                                    litigationRequestReformerAssigningRequest.CitizenId = reformers.FirstOrDefault().Id;
                                                    litigationRequestReformerAssigningRequest.RequestOn = DateTime.Now;
                                                    litigationRequestReformerAssigningRequest.LitigationRequestId = litigationRequest.Id;
                                                    litigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.None;
                                                    await _repository.AddAsync<LitigationRequestReformerAssigningRequest, Guid>(litigationRequestReformerAssigningRequest);
                                                }
                                                var approveSaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                                                if (!approveSaveResponse.IsDone)
                                                    return approveSaveResponse;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                        // Send Notification
                        // تم قبول الدعوى واستلامها
                        break;
                    case RequestStatus.Edited:
                        // Send Notification
                        // تم تعديل الدعوى وإرسالها مرة أخرى
                        break;
                    case RequestStatus.AgreementRequestHasBeenSent:
                        if (litigationRequest.IsAgreementRequestRequired.HasValue)
                            return new BaseResponse() { IsDone = false, Message = "تم طلب المشارطة من قبل" };

                        litigationRequest.IsAgreementRequestRequired = true;
                        litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAgreementRequestRequired;
                        var repositorySaveRes = await _repository.SaveChangesAsync(new CancellationToken());
                        if (!repositorySaveRes.IsDone)
                            return repositorySaveRes;

                        // Send Notification
                        // تم إرسال طلب المشارطة
                        break;
                }


            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return new DataResponse<ResponseWorkFlowDto>()
            {
                IsDone = true,
                Data = responseWorkFlowDto
            };
        }
    }
}
