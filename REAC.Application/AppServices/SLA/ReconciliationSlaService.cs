using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;

namespace REAC.Application.AppServices.SLA
{
    public interface IReconciliationSlaService
    {
        Task<BaseResponse> Execute();
    }
    public class ReconciliationSlaService : IReconciliationSlaService
    {
        protected IEntityRepository _repository;
        protected IMapper _mapper;
        protected ISysSettingService _sysSettingService;
        public ReconciliationSlaService(ISysSettingService sysSettingService, IMapper mapper, IEntityRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
            _sysSettingService = sysSettingService;
        }
        public async Task<BaseResponse> Execute()
        {

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                              .Where(x => x.LitigationRequestStage == LitigationRequestStage.Reconciliation
                              & x.IsFinished != true)
                              .ToList();

            if (litigationRequests != null)
            {
                if (litigationRequests.Any())
                {
                    int ReformerReminderForReconciliationFirstPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_ReformerReminderForReconciliationFirstPeriod).Result.Data?.Value, out ReformerReminderForReconciliationFirstPeriod);
                    int ReformerReminderForReconciliationSecondPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_ReformerReminderForReconciliationSecondPeriod).Result.Data?.Value, out ReformerReminderForReconciliationSecondPeriod);

                    foreach (var litigationRequest in litigationRequests)
                    {
                        if (ReformerReminderForReconciliationFirstPeriod > 0)
                        {
                            if (litigationRequest.IsReformerReminderForReconciliationFirstPeriodSent != true)
                            {
                                double totalHours = 0;

                                totalHours += (DateTime.Now - litigationRequest.StartReconciliationDate).Value.TotalHours;
                                if (totalHours >= ReformerReminderForReconciliationFirstPeriod)
                                {
                                    litigationRequest.IsReformerReminderForReconciliationFirstPeriodSent = true;

                                    //Send Notification for reformer
                                    // Mail - Sms - Application Notification
                                    // يرجى سرعة اتمام عملية الصلح على الدعوى رقم ......
                                }
                            }
                        }

                        if (ReformerReminderForReconciliationSecondPeriod > 0)
                        {
                            double totalReviewHours = 0;

                            totalReviewHours += (DateTime.Now - litigationRequest.StartReconciliationDate).Value.TotalHours;
                            if (totalReviewHours >= ReformerReminderForReconciliationSecondPeriod)
                            {
                                // Send Notification for Reconciliation admin
                                // Mail - Sms - Application Notification
                                // يرجى العمل أن المصلح تجاوز الفترة المسموح بها للصلح على الدعوى رقم .....
                            }
                        }

                        var repositorySaveResponse = await _repository.SaveChangesAsync(default);
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;
                    }
                }
            }

            litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                              .Include(x => x.LitigationRequestReconciliationReports)
                              .Where(x => x.LitigationRequestStage == LitigationRequestStage.Reconciliation
                              & x.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestReportAdded
                              & x.LitigationRequestReconciliationReports.Count > 0
                              & x.IsFinished != true)
                             .ToList();

            if (litigationRequests != null)
            {
                if (litigationRequests.Any())
                {
                    int LitigationRequestReconciliationReportResponsePeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReconciliationReportResponsePeriod).Result.Data?.Value, out LitigationRequestReconciliationReportResponsePeriod);
                    int LitigationRequestReconciliationReportDecisionPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReconciliationReportDecisionPeriod).Result.Data?.Value, out LitigationRequestReconciliationReportDecisionPeriod);

                    foreach (var litigationRequest in litigationRequests)
                    {
                        if (LitigationRequestReconciliationReportResponsePeriod > 0)
                        {
                            if (litigationRequest.IsLitigationRequestReconciliationReportResponsePeriodSent != true)
                            {
                                double totalHours = 0;

                                totalHours += (DateTime.Now - litigationRequest.LitigationRequestReconciliationReports.FirstOrDefault().ReportDate).TotalHours;
                                if (totalHours >= LitigationRequestReconciliationReportResponsePeriod)
                                {
                                    litigationRequest.IsLitigationRequestReconciliationReportResponsePeriodSent = true;
                                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus == null)
                                    {
                                        //Send Notification for Plaintiff
                                        // Mail - Sms - Application Notification
                                        // يرجى سرعة الرد على محضر الصلح الصادر على الدعوى رقم ......
                                    }
                                    if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus == null)
                                    {
                                        //Send Notification for Defendant
                                        // Mail - Sms - Application Notification
                                        // يرجى سرعة الرد على محضر الصلح الصادر على الدعوى رقم ......
                                    }
                                }
                            }
                        }

                        if (LitigationRequestReconciliationReportDecisionPeriod > 0)
                        {
                            double totalReviewHours = 0;

                            totalReviewHours += (DateTime.Now - litigationRequest.LitigationRequestReconciliationReports.FirstOrDefault().ReportDate).TotalHours;
                            if (totalReviewHours >= LitigationRequestReconciliationReportDecisionPeriod)
                            {
                                //Send Notification for reformer
                                // Mail - Sms - Application Notification
                                // يرجى العلم أن الرد على محضر الصلح الصادر على الدعوى رقم ..... قد تجاوز مهلته القصوى
                            }
                        }

                        var repositorySaveResponse = await _repository.SaveChangesAsync(default);
                        if (!repositorySaveResponse.IsDone)
                            return repositorySaveResponse;
                    }
                }
            }


            var litigationRequestReformerAssigningRequests = (await _repository.GetQueryable<LitigationRequestReformerAssigningRequest, Guid>())?
                              .Include(x => x.ReformerCitizen)
                              .Include(x => x.LitigationRequest)
                              .Where(x => x.Response == LitigationRequestReformerAssigningRequestStatus.None)
                              .ToList();

            if (litigationRequestReformerAssigningRequests != null)
            {
                if (litigationRequestReformerAssigningRequests.Any())
                {
                    int LitigationRequestReformerAssigningRequestResponsePeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReformerAssigningRequestResponsePeriod).Result.Data?.Value, out LitigationRequestReformerAssigningRequestResponsePeriod);
                    int LitigationRequestReformerAssigningRequestDecisionPeriod = 0;
                    int.TryParse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReformerAssigningRequestDecisionPeriod).Result.Data?.Value, out LitigationRequestReformerAssigningRequestDecisionPeriod);

                    foreach (var litigationRequestReformerAssigningRequest in litigationRequestReformerAssigningRequests)
                    {
                        if (LitigationRequestReformerAssigningRequestResponsePeriod > 0)
                        {
                            if (litigationRequestReformerAssigningRequest.LitigationRequest.IsLitigationRequestReformerAssigningRequestResponsePeriodSent != true)
                            {
                                double totalHours = 0;

                                totalHours += (DateTime.Now - litigationRequestReformerAssigningRequest.RequestOn).Value.TotalHours;
                                if (totalHours >= LitigationRequestReformerAssigningRequestResponsePeriod)
                                {
                                    litigationRequestReformerAssigningRequest.LitigationRequest.IsLitigationRequestReformerAssigningRequestResponsePeriodSent = true;

                                    // litigationRequestReformerAssigningRequest.CitizenId
                                    // Send Notification for reformer
                                    // Mail - Sms - Application Notification
                                    // يرجى سرعة الرد على طلب مهمة الصلح على الدعوى رقم ......
                                }
                            }
                        }

                        if (LitigationRequestReformerAssigningRequestDecisionPeriod > 0)
                        {
                            double totalHours = 0;

                            totalHours += (DateTime.Now - litigationRequestReformerAssigningRequest.RequestOn).Value.TotalHours;
                            if (totalHours >= LitigationRequestReformerAssigningRequestDecisionPeriod)
                            {
                                litigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.Cancelled;
                                litigationRequestReformerAssigningRequest.ResponseOn = DateTime.Now;

                                // Send Notification for old reformer
                                // old reformer = litigationRequestReformerAssigningRequest.CitizenId
                                // Mail - Sms - Application Notification
                                // تم إلغاء طلب مهمة الصلح على الدعوى رقم ..... لتجاوز الرد على المهمة مهلته القصوى 

                                await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                {
                                    Id = Guid.NewGuid(),
                                    CreatedOn = DateTime.Now,
                                    RequestId = litigationRequestReformerAssigningRequest.LitigationRequestId,
                                    ShowForApplicant = false,
                                    LogMessage = "تم إلغاء طلب مهمة الصلح للمصلح " + litigationRequestReformerAssigningRequest.ReformerCitizen.FullName
                                });

                                var department = (await _repository.GetQueryable<SysDepartment, Guid>())?
                                .Where(x => x.DepartmentType == DepartmentType.Reconciliation)
                                .SingleOrDefault();
                                if (department != null)
                                {
                                    switch (department.DistributionMode)
                                    {
                                        case DepartmentDistributionMode.AutoDistribution:
                                            var reformer = (await _repository.GetQueryable<Citizen, Guid>())?
                                                .Include(x => x.ReformerLitigationRequests)
                                                        .Where(x => x.IsReformer == true & x.ReformerStatusId == ReformerStatus.Certified & x.Id != litigationRequestReformerAssigningRequest.CitizenId)
                                                        .OrderBy(x => x.ReformerLitigationRequests
                                                        .Count(t => t.IsFinished == false))
                                                        .FirstOrDefault();
                                            if (reformer != null)
                                            {

                                                LitigationRequestReformerAssigningRequest newLitigationRequestReformerAssigningRequest = new LitigationRequestReformerAssigningRequest();
                                                newLitigationRequestReformerAssigningRequest.CitizenId = reformer.Id;
                                                newLitigationRequestReformerAssigningRequest.RequestOn = DateTime.Now;
                                                newLitigationRequestReformerAssigningRequest.LitigationRequestId = litigationRequestReformerAssigningRequest.LitigationRequestId;
                                                newLitigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.None;
                                                await _repository.AddAsync<LitigationRequestReformerAssigningRequest, Guid>(newLitigationRequestReformerAssigningRequest);

                                                litigationRequestReformerAssigningRequest.LitigationRequest.IsLitigationRequestReformerAssigningRequestResponsePeriodSent = false;

                                                // Send Notification for new reformer
                                                // new reformer = reformer.Id
                                                // Mail - Sms - Application Notification
                                                // يوجد طلب مهمة صلح جديد على الدعوى رقم  ..........

                                                // Send Notification for Reconciliation admin
                                                // Mail - Sms - Application Notification
                                                // يرجى العلم أن الرد على مهمة الصلح للمصلح على الدعوى رقم ..... قد تجاوز مهلته القصوى وتم إلغاؤها وارسال مهمة جديدة للمصلح reformer.FullName

                                                await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                                {
                                                    Id = Guid.NewGuid(),
                                                    CreatedOn = DateTime.Now,
                                                    RequestId = litigationRequestReformerAssigningRequest.LitigationRequestId,
                                                    ShowForApplicant = false,
                                                    LogMessage = "تم إرسال طلب مهمة الصلح للمصلح " + reformer.FullName
                                                });
                                            }
                                            break;
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

            return new BaseResponse() { IsDone = true };
        }
    }
}
