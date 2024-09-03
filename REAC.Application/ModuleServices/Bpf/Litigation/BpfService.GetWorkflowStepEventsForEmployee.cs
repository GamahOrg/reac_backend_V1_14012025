using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Wpf.System.WorkFlowStepEvent;
using REAC.Domain.Shared.Enum.Wpf;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using System.Globalization;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> GetWorkflowStepEventsForEmployee(Guid requestId, CancellationToken cancellationToken)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var employee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                    .Where(x => x.Id == referenceId).Single();

                if (employee == null)
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Employee Not Found"
                    };
                }

                var processinglog = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())?
                    .Where(x => x.RequestId == requestId &
                           x.IsFinished == false)
                    .Include(x => x.Employee)
                    .Include(x => x.WorkFlowStep)
                    .ThenInclude(x => x.WorkFlowStepEvents)
                    .ThenInclude(x => x.WorkFlowStepEventSecurityLevelActors).ToList();

                if (processinglog == null)
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "not  defined!!"
                    };
                }

                WorkFlowStepEventForActorShortDto WorkFlowStepEventForActorShortDtos = new WorkFlowStepEventForActorShortDto();

                WorkFlowStepEventForActorShortDtos.AllowAssign = employee.AllowAssign;
                WorkFlowStepEventForActorShortDtos.AllowReAssign = employee.AllowReAssign;

                processinglog?.ForEach(x =>
                {
                    x.WorkFlowStep.WorkFlowStepEvents.ToList().ForEach(y =>
                    {
                        switch (y.EventSecurityLevelId)
                        {
                            case EventSecurityLevel.Specified:
                                bool mainBreak = false;
                                y.WorkFlowStepEventSecurityLevelActors.ToList().ForEach(z =>
                                {

                                    if (mainBreak) { return; }

                                    switch (z.SecurityLevelActorId)
                                    {
                                        case SecurityLevelActorType.Department:
                                            if (z.DepartmentId.HasValue)
                                            {
                                                if (employee.DepartmentId == z.DepartmentId)
                                                {
                                                    WorkFlowStepEventForActorShortDtos.WorkFlowStepEventShortDtos.Add(new WorkFlowStepEventShortDto()
                                                    {
                                                        Id = y.Id,
                                                        EventName = (y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName)) ? y.EventName : y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName,
                                                        AllowFileUpload = y.AllowFileUpload,
                                                        AllowNotes = y.AllowNotes,
                                                        ProcessingLogId = x.Id,
                                                        CustomActionType = y.CustomActionType
                                                    });
                                                    mainBreak = true;
                                                    break;
                                                }
                                            }
                                            break;
                                        case SecurityLevelActorType.Admin:
                                            if (z.DepartmentId.HasValue)
                                            {
                                                if (employee.DepartmentId == z.DepartmentId & employee.IsAdmin)
                                                {
                                                    WorkFlowStepEventForActorShortDtos.WorkFlowStepEventShortDtos.Add(new WorkFlowStepEventShortDto()
                                                    {
                                                        Id = y.Id,
                                                        EventName = (y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName)) ? y.EventName : y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName,
                                                        AllowFileUpload = y.AllowFileUpload,
                                                        AllowNotes = y.AllowNotes,
                                                        ProcessingLogId = x.Id,
                                                        CustomActionType = y.CustomActionType
                                                    });
                                                    mainBreak = true;
                                                    break;
                                                }
                                            }
                                            break;
                                        default:
                                            if (z.EmployeeId.HasValue)
                                            {
                                                if (z.EmployeeId == referenceId)
                                                {
                                                    WorkFlowStepEventForActorShortDtos.WorkFlowStepEventShortDtos.Add(new WorkFlowStepEventShortDto()
                                                    {
                                                        Id = y.Id,
                                                        EventName = (y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName)) ? y.EventName : y.Translations.FirstOrDefault(
                                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName,
                                                        AllowFileUpload = y.AllowFileUpload,
                                                        AllowNotes = y.AllowNotes,
                                                        ProcessingLogId = x.Id,
                                                        CustomActionType = y.CustomActionType
                                                    });
                                                    mainBreak = true;
                                                    break;
                                                }

                                            }
                                            break;
                                    }
                                });
                                break;
                            case EventSecurityLevel.UnSpecified:
                                if (x.EmployeeId == employee.Id)
                                {
                                    WorkFlowStepEventForActorShortDtos.WorkFlowStepEventShortDtos.Add(new WorkFlowStepEventShortDto()
                                    {
                                        Id = y.Id,
                                        EventName = (y.Translations.FirstOrDefault(
                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(y.Translations.FirstOrDefault(
                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName)) ? y.EventName : y.Translations.FirstOrDefault(
                                        x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName,
                                        AllowFileUpload = y.AllowFileUpload,
                                        AllowNotes = y.AllowNotes,
                                        ProcessingLogId = x.Id,
                                        CustomActionType = y.CustomActionType
                                    });
                                }
                                else
                                {
                                    if (employee.IsAdmin & employee.WorkAsEmployee)
                                    {
                                        if (x.WorkFlowStep.DepartmentId == employee.DepartmentId)
                                        {
                                            WorkFlowStepEventForActorShortDtos.WorkFlowStepEventShortDtos.Add(new WorkFlowStepEventShortDto()
                                            {
                                                Id = y.Id,
                                                EventName = (y.Translations.FirstOrDefault(
                                                x => x.LanguageCode == CultureInfo.CurrentUICulture.Name) == null || string.IsNullOrEmpty(y.Translations.FirstOrDefault(
                                                x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName)) ? y.EventName : y.Translations.FirstOrDefault(
                                                x => x.LanguageCode == CultureInfo.CurrentUICulture.Name).EventName,
                                                AllowFileUpload = y.AllowFileUpload,
                                                AllowNotes = y.AllowNotes,
                                                ProcessingLogId = x.Id,
                                                CustomActionType = y.CustomActionType
                                            });
                                        }
                                    }
                                }
                                break;
                        }
                    });
                });

                return new DataResponse<WorkFlowStepEventForActorShortDto>()
                {
                    Data = WorkFlowStepEventForActorShortDtos,
                    IsDone = true
                };
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Exception = ex.Message + "\n" + ex.ToString(),
                };
            }
        }
    }
}
