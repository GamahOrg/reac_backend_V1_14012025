using AutoMapper;
using Core.Contracts;
using Core.GenericRepository.BaseRepository;
using Core.Shared.Responses;
using Microsoft.AspNetCore.SignalR;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using Wafi.Application.Hubs;
using Wafi.Application.Hubs.Contracts;

namespace REAC.Application.ModuleServices
{
    public partial class ReconciliationService : IReconciliationService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository _repository;
        private readonly IJwtReader _jwtReader;
        private readonly ISysSettingService _settingService;
        private readonly ISMSService _sMSService;
        private readonly IMailService _mailService;
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;

        public ReconciliationService(IEntityRepository repository, ISysSettingService settingService,
                          IWorkFlowProcessingLogService workFlowProcessingLogService,
                          IMapper mapper,
                          IJwtReader jwtReader,
                          ISMSService sMSService,
                          IMailService mailService,
                          IHubContext<NotificationHub, INotificationClient> notificationHub)

        {
            _mapper = mapper;
            _repository = repository;
            _jwtReader = jwtReader;
            _settingService = settingService;
            _sMSService = sMSService;
            _mailService = mailService;
            _notificationHub = notificationHub;
        }

        public async Task<BaseResponse> Assign(AssignRequestPayload assignRequestPayload, CancellationToken cancellationToken)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                if (referenceId == Guid.Empty)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "موظف غير معروف"
                    };
                var employee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                        .Where(x => x.Id == referenceId).Single();

                if (employee == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "موظف غير معروف"
                    };

                if (!employee.AllowAssign)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "موظف غير مسموح له"
                    };

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                       .Where(x => x.Id == assignRequestPayload.RequestId).Single();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب دعوى غير موجود"
                    };

                if (litigationRequest.IsFinished == true)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب دعوى منتهى"
                    };

                if (litigationRequest.ReformerCitizenId.HasValue)
                {
                    if (!employee.AllowReAssign)
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "موظف غير مسموح له"
                        };
                }
                var litigationRequestReformerAssigningRequests = (await _repository.GetQueryable<LitigationRequestReformerAssigningRequest, Guid>())?
                     .Where(x => x.LitigationRequestId == assignRequestPayload.RequestId & x.CitizenId == assignRequestPayload.ReformerId & x.Response == 0);

                if (litigationRequestReformerAssigningRequests.Any())
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "يوجد طلب مهمه سابق لم يتم الرد عليه حتى الان من قبل المصلح"
                    };
                }

                litigationRequestReformerAssigningRequests = (await _repository.GetQueryable<LitigationRequestReformerAssigningRequest, Guid>())?
                   .Where(x => x.LitigationRequestId == assignRequestPayload.RequestId & x.Response == 0);


                if (litigationRequestReformerAssigningRequests.Any())
                {
                    foreach (var request in litigationRequestReformerAssigningRequests.ToList())
                    {
                        request.Response = LitigationRequestReformerAssigningRequestStatus.Cancelled;
                        var saveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                        if (!saveResponse.IsDone)
                            return saveResponse;

                        // Send Notification
                        // تم إلغاء المهمة (دعوى صلح)
                    }
                }

                LitigationRequestReformerAssigningRequest litigationRequestReformerAssigningRequest = new LitigationRequestReformerAssigningRequest();
                litigationRequestReformerAssigningRequest.CitizenId = assignRequestPayload.ReformerId;
                litigationRequestReformerAssigningRequest.LitigationRequestId = litigationRequest.Id;
                litigationRequestReformerAssigningRequest.RequestOn = DateTime.Now;
                litigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.None;
                await _repository.AddAsync<LitigationRequestReformerAssigningRequest, Guid>(litigationRequestReformerAssigningRequest);

                // Send Notification
                // تم إرسال مهمة مصلح لمصلح أخر

                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                return repositorySaveResponse;
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Exception = ex.Message + "\n" + ex.ToString(),
                };
            }
        }

        #region Decision
        public async Task<BaseResponse> AddLitigationRequestDecision(LitigationRequestAction requestPayload, CancellationToken cancellationToken)

        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                      .Where(x => x.Id == requestPayload.RequestId & x.ReformerCitizenId == referenceId).ToList();

            if (!litigationRequests.Any())
                return new BaseResponse() { IsDone = false, Message = "طلب الدعوى غير موجود" };

            switch (requestPayload.LitigationRequestDecision)
            {
                case LitigationRequestDecision.WaiverOfRight:
                    litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.WaiverOfRight;
                    litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.WaiverOfRight;
                    litigationRequests.FirstOrDefault().IsActive = false;
                    litigationRequests.FirstOrDefault().IsFinished = true;
                    litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReformerDecisionAdded;
                    // Send Notification
                    // تم التنازل عن الحق
                    break;
                case LitigationRequestDecision.ReconciliationWasMade:
                    litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasMade;
                    litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasMade;
                    litigationRequests.FirstOrDefault().IsActive = false;
                    litigationRequests.FirstOrDefault().IsFinished = true;
                    litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReformerDecisionAdded;
                    // Send Notification
                    // تم الصلح
                    break;
                case LitigationRequestDecision.WaiverOfTheLitigation:
                    litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.WaiverOfTheLitigation;
                    litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.WaiverOfTheLitigation;
                    litigationRequests.FirstOrDefault().IsActive = false;
                    litigationRequests.FirstOrDefault().IsFinished = true;
                    litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReformerDecisionAdded;
                    // Send Notification
                    // تم التنازل عن الدعوى
                    break;
                case LitigationRequestDecision.ReconciliationWasNotMade:

                    if (litigationRequests.FirstOrDefault().LitigationRequestCategoryId == LitigationRequestCategory.ReconciliationAndArbitration)
                    {
                        litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasNotMadeAndMoveToArbitration;
                        litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasNotMadeAndMoveToArbitration;
                        litigationRequests.FirstOrDefault().StartArbitrationDate = DateTime.Now;
                        litigationRequests.FirstOrDefault().LitigationRequestStage = LitigationRequestStage.Arbitration;
                        litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.ArbitratorSelectionPeriod;
                        // Send Notification
                        // عدم الصلح والانتقال للتحكيم
                    }
                    else
                    {
                        litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasNotMade;
                        litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasNotMade;
                        litigationRequests.FirstOrDefault().IsActive = false;
                        litigationRequests.FirstOrDefault().IsFinished = true;
                        // Send Notification
                        // عدم الصلح
                    }
                    break;
            }

            litigationRequests.FirstOrDefault().EndReconciliationDate = DateTime.Now;

            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            return await GetLitigationRequestForReport(requestPayload.RequestId, cancellationToken);

        }
        #endregion
    }
}
