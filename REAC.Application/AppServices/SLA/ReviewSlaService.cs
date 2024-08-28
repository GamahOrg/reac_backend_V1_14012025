using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.IAppServices.SLA;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;

namespace REAC.Application.AppServices.SLA
{

    public class ReviewSlaService : IReviewSlaService
    {
        protected IEntityRepository _repository;
        protected IMapper _mapper;
        protected ISysSettingService _sysSettingService;
        public ReviewSlaService(ISysSettingService sysSettingService, IMapper mapper, IEntityRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
            _sysSettingService = sysSettingService;
        }
        public async Task<BaseResponse> Execute()
        {
            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                             .Include(x => x.WorkFlowProcessingLogs)
                             .ThenInclude(x => x.WorkFlowStep)
                             .Where(x => x.InternalStatusId == RequestStatus.New & x.IsFinished != true)
                             .ToList();

            if (litigationRequests != null)
            {
                if (litigationRequests.Any())
                {
                    int reminderForReviewerPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_ReminderForReviewer).Result.Data?.Value, out reminderForReviewerPeriod);
                    int reminderForReviewerManagerPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_ReminderForReviewerManager).Result.Data?.Value, out reminderForReviewerManagerPeriod);

                    foreach (var litigationRequest in litigationRequests)
                    {
                        if (litigationRequest.IsReminderForReviewerSent != true)
                        {
                            if (reminderForReviewerPeriod > 0)
                            {
                                var workFlowProcessingLogs = litigationRequest.WorkFlowProcessingLogs.Where(x => x.EmployeeId.HasValue == true).OrderBy(x => x.CreatedOn).ToList();
                                Guid? employeeId = workFlowProcessingLogs.Last().EmployeeId;
                                var employeeWorkFlowProcessingLogs = workFlowProcessingLogs.Where(x => x.EmployeeId == employeeId);

                                if (employeeWorkFlowProcessingLogs.Any())
                                {
                                    double totalReviewHours = 0;

                                    foreach (var employeeWorkFlowProcessingLog in employeeWorkFlowProcessingLogs)
                                    {
                                        if (employeeWorkFlowProcessingLog.IsFinished)
                                        {
                                            totalReviewHours += (employeeWorkFlowProcessingLog.FinishedOn - employeeWorkFlowProcessingLog.CreatedOn).Value.TotalHours;
                                        }
                                        else
                                        {
                                            totalReviewHours += (DateTime.Now - employeeWorkFlowProcessingLog.CreatedOn).TotalHours;
                                        }
                                    }
                                    if (totalReviewHours >= reminderForReviewerPeriod)
                                    {
                                        litigationRequest.IsReminderForReviewerSent = true;

                                        //Send Notification for reviewer employee
                                        // Mail - Sms - Application Notification
                                        //يرجى اتمام مراجعة الرعوى رقم .....  
                                    }
                                }

                            }
                        }

                        if (litigationRequest.IsReminderForReviewerManagerSent != true)
                        {
                            if (reminderForReviewerManagerPeriod > 0)
                            {
                                var workFlowProcessingLogs = litigationRequest.WorkFlowProcessingLogs.Where(x => x.EmployeeId.HasValue == true).OrderBy(x => x.CreatedOn).ToList();
                                Guid? employeeId = workFlowProcessingLogs.Last().EmployeeId;
                                var employeeWorkFlowProcessingLogs = workFlowProcessingLogs.Where(x => x.EmployeeId == employeeId);

                                if (employeeWorkFlowProcessingLogs.Any())
                                {
                                    double totalReviewHours = 0;

                                    foreach (var employeeWorkFlowProcessingLog in employeeWorkFlowProcessingLogs)
                                    {
                                        if (employeeWorkFlowProcessingLog.IsFinished)
                                        {
                                            totalReviewHours += (employeeWorkFlowProcessingLog.FinishedOn - employeeWorkFlowProcessingLog.CreatedOn).Value.TotalHours;
                                        }
                                        else
                                        {
                                            totalReviewHours += (DateTime.Now - employeeWorkFlowProcessingLog.CreatedOn).TotalHours;
                                        }
                                    }
                                    if (totalReviewHours >= reminderForReviewerManagerPeriod)
                                    {
                                        litigationRequest.IsReminderForReviewerManagerSent = true;

                                        var employee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                                                             .Include(x => x.Department)
                                                             .ThenInclude(x => x.SysEmployees)
                                                             .ThenInclude(x => x.WorkFlowProcessingLogs)
                                                             .Where(x => x.Id == employeeId)
                                                             .FirstOrDefault();

                                        if (employee.Department.DistributionMode == DepartmentDistributionMode.AutoDistribution)
                                        {
                                            if (!employeeWorkFlowProcessingLogs.LastOrDefault().IsFinished)
                                            {
                                                Guid? newEmployeeId = employee?.Department.SysEmployees?.Where(x => x.Id != employeeId & x.IsAdmin == false).OrderBy(x => x.WorkFlowProcessingLogs.Where(x => x.IsFinished != true).Count()).FirstOrDefault()?.Id;

                                                if (!newEmployeeId.HasValue)
                                                {
                                                    newEmployeeId = employee?.Department.SysEmployees.Where(x => x.Id != employeeId & x.IsAdmin == true).ToList().FirstOrDefault()?.Id;
                                                }

                                                if (newEmployeeId.HasValue)
                                                {
                                                    litigationRequest.IsReminderForReviewerSent = false;
                                                    litigationRequest.IsReminderForReviewerManagerSent = false;

                                                    employeeWorkFlowProcessingLogs.LastOrDefault().IsFinished = true;

                                                    WorkFlowProcessingLog workFlowProcessingLog = new WorkFlowProcessingLog();
                                                    workFlowProcessingLog.Id = Guid.NewGuid();
                                                    workFlowProcessingLog.EmployeeId = newEmployeeId;
                                                    workFlowProcessingLog.IsFinished = false;
                                                    workFlowProcessingLog.RequestId = litigationRequest.Id;
                                                    workFlowProcessingLog.CreatedOn = DateTime.Now;
                                                    workFlowProcessingLog.WorkFlowStepId = employeeWorkFlowProcessingLogs.LastOrDefault().WorkFlowStepId;
                                                    await _repository.AddAsync<WorkFlowProcessingLog, Guid>(workFlowProcessingLog);

                                                    litigationRequest.IsReminderForReviewerManagerSent = false;

                                                    //Send Notification for old reviewer
                                                    // Mail - Sms - Application Notification
                                                    //تم سحب دعوى جديدة رقم ..... لتخطى مدة التدقيق الفصوى

                                                    //Send Notification for new reviewer
                                                    // Mail - Sms - Application Notification
                                                    //تم ارسال دعوى جديدة رقم ..... للتدقيق

                                                    //Send Notification for review department admin
                                                    // Mail - Sms - Application Notification
                                                    //تم ارسال الدعوى رقم .... إلى مدقق جديد لتخطى المدقق السابق مدة التدقيق الفصوى
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Send Notification for review department admin
                                            // Mail - Sms - Application Notification
                                            // يوجد تأخير تجاوز المده المسموح بها للتدقيق على الدعوى رقم .....
                                        }
                                    }
                                }
                            }
                        }

                        var repositorySaveResponse = await _repository.SaveChangesAsync(default);
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;
                    }
                }
            }

            litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                             .Include(x => x.WorkFlowProcessingLogs)
                             .ThenInclude(x => x.WorkFlowStep)
                             .Where(x => x.InternalStatusId == RequestStatus.SentBackForEdit & x.IsFinished != true)
                             .ToList();

            if (litigationRequests != null)
            {
                if (litigationRequests.Any())
                {
                    int reminderForapplicantFirstPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_FirstEditReminderForRequestApplicant).Result.Data?.Value, out reminderForapplicantFirstPeriod);
                    int reminderForApplicantSecondPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_SecondEditReminderForForRequestApplicant).Result.Data?.Value, out reminderForApplicantSecondPeriod);

                    foreach (var litigationRequest in litigationRequests)
                    {
                        var workFlowProcessingLog = litigationRequest.WorkFlowProcessingLogs.Where(x => x.EmployeeId.HasValue == false & x.IsFinished == false).OrderBy(x => x.CreatedOn).LastOrDefault();

                        if (workFlowProcessingLog != null)
                        {
                            double totalReviewHours = 0;

                            totalReviewHours = (DateTime.Now - workFlowProcessingLog.CreatedOn).TotalHours;

                            if (reminderForapplicantFirstPeriod > 0)
                            {
                                if (litigationRequest.IsReminderForApplicantSent != true)
                                {
                                    if (totalReviewHours >= reminderForapplicantFirstPeriod)
                                    {
                                        litigationRequest.IsReminderForApplicantSent = true;

                                        //Send Notification for applicant and plaintiff
                                        // Mail - Sms - Application Notification
                                        //يرجى سرعة معالجة كافة الملاحظات على الدعوى رقم ........
                                    }
                                }
                            }
                            if (reminderForApplicantSecondPeriod > 0)
                            {
                                if (totalReviewHours >= reminderForApplicantSecondPeriod)
                                {
                                    litigationRequest.IsFinished = true;
                                    litigationRequest.IsActive = false;
                                    litigationRequest.InternalStatusId = RequestStatus.Cancelled;
                                    litigationRequest.ExternalStatusId = RequestStatus.Cancelled;
                                    litigationRequest.WorkFlowProcessingLogs.OrderBy(x => x.CreatedOn).LastOrDefault().IsFinished = true;
                                    litigationRequest.WorkFlowProcessingLogs.OrderBy(x => x.CreatedOn).LastOrDefault().FinishedOn = DateTime.Now;

                                    //Send Notification for applicant and plaintiff and review department admin and reviewer
                                    // Mail - Sms - Application Notification
                                    //تم إلفاء الدعوى رقم ........ لعدم الرد على الملاحظات المرسلة
                                }
                            }
                        }

                        var repositorySaveResponse = await _repository.SaveChangesAsync(default);
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;
                    }
                }
            }

            litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                             .Include(x => x.WorkFlowProcessingLogs)
                             .ThenInclude(x => x.WorkFlowStep)
                             .Where(x => x.InternalStatusId == RequestStatus.AgreementRequestHasBeenSent & x.IsFinished != true)
                             .ToList();

            if (litigationRequests != null)
            {
                if (litigationRequests.Any())
                {
                    int firstAgreementRequestForRequest = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_FirstAgreementRequestForRequest).Result.Data?.Value, out firstAgreementRequestForRequest);
                    int secondAgreementRequestForForRequest = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_SecondAgreementRequestForForRequest).Result.Data?.Value, out secondAgreementRequestForForRequest);

                    foreach (var litigationRequest in litigationRequests)
                    {
                        double totalReviewHours = 0;

                        totalReviewHours = (DateTime.Now - litigationRequest.AgreementRequestRequiredOn).Value.TotalHours;

                        if (firstAgreementRequestForRequest > 0)
                        {
                            if (litigationRequest.IsReminderForAgreementRequestForRequestSent != true)
                            {
                                if (totalReviewHours >= secondAgreementRequestForForRequest)
                                {
                                    litigationRequest.IsReminderForAgreementRequestForRequestSent = true;

                                    if (!litigationRequest.DefendantAgreementRequestResponse.HasValue)
                                    {
                                        //Send Notification for Defendant
                                        // Mail - Sms - Application Notification
                                        //يرجى سرعة على طلب المشارطة المرسل على الدعوى رقم ........
                                    }
                                    if (!litigationRequest.PlaintiffAgreementRequestResponse.HasValue)
                                    {
                                        //Send Notification for applicant and plaintiff
                                        // Mail - Sms - Application Notification
                                        //يرجى سرعة على طلب المشارطة المرسل على الدعوى رقم ........
                                    }
                                }
                            }
                        }

                        if (secondAgreementRequestForForRequest > 0)
                        {
                            if (totalReviewHours >= secondAgreementRequestForForRequest)
                            {
                                litigationRequest.IsFinished = true;
                                litigationRequest.IsActive = false;
                                litigationRequest.InternalStatusId = RequestStatus.Cancelled;
                                litigationRequest.ExternalStatusId = RequestStatus.Cancelled;
                                litigationRequest.WorkFlowProcessingLogs.OrderBy(x => x.CreatedOn).LastOrDefault().IsFinished = true;
                                litigationRequest.WorkFlowProcessingLogs.OrderBy(x => x.CreatedOn).LastOrDefault().FinishedOn = DateTime.Now;

                                //Send Notification for applicant and plaintiff and Defendant and review department admin
                                // Mail - Sms - Application Notification
                                //تم إلفاء الدعوى رقم ........ لعدم الرد على طلب المشارطة
                            }
                        }

                        var repositorySaveResponse = await _repository.SaveChangesAsync(default);
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;
                    }
                }
            }

            return new BaseResponse() { IsDone = true };

        }
    }
}
