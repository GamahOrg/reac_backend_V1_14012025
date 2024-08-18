using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Arbitration;
using REAC.Application.Contracts.Dtos.Reconciliation;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Arbitration;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.ModuleServices
{
    public interface IArbitrationService
    {
        #region ArbitratorsCount
        Task<BaseResponse> AddArbitratorsCount(ArbitratorCountRequestPayload arbitratorCountRequestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsCount(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsCount(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsCountReply(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsCountReply(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsCountReply(ArbitratorCountRequestPayload arbitratorCountRequestPayload, CancellationToken cancellationToken);
        #endregion

        #region SelectionDuration
        Task<BaseResponse> AddArbitratorSelectionDuration(ArbitratorSelectionDurationRequestPayload arbitratorCountRequestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorSelectionDuration(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForArbitratorSelectionDuration(int pageIndex, int pageSize, CancellationToken cancellationToken);
        #endregion

        #region ExtendPeriod
        Task<BaseResponse> AddArbitrationExtendPeriod(LitigationRequestExtendPeriodRequestCreateUpdateDto requestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> AddArbitrationExtendPeriodForEmployee(LitigationRequestExtendPeriodRequestCreateUpdateEmployeeDto requestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriod(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriodForEmployee(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriod(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriodForEmployee(int pageIndex, int pageSize, CancellationToken cancellationToken);
        #endregion

        #region Session
        Task<BaseResponse> AddArbitrationSession(ScheduleSessionProcessRequestPayload requestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForSession(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForSession(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAllSession(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAllSessionForArbitrator(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForFullAllSession(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForFullAllArbitrator(int pageIndex, int pageSize, CancellationToken cancellationToken);
        #endregion

        #region Report
        Task<BaseResponse> AddArbitrationReport(WritingArbitrationReportRequestPayload requestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForReport(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForReport(int pageIndex, int pageSize, CancellationToken cancellationToken);
        #endregion

        #region ExplanationRequest
        Task<BaseResponse> AddArbitrationReportExplanationRequest(LitigationRequestArbitrationReportExplanationRequestCreateDto requestPayload);
        Task<BaseResponse> GetLitigationRequestForReportExplanationRequest(Guid requestId);
        Task<BaseResponse> GetLitigationRequestForReportExplanationRequest(int pageIndex, int pageSize);
        Task<BaseResponse> GetLitigationRequestForReportExplanationRequestForArbitrator(int pageIndex, int pageSize);
        Task<BaseResponse> GetLitigationRequestForReportExplanationRequestForArbitrator(Guid requestId);
        Task<BaseResponse> AddArbitrationReportExplanationRequestForArbitrator(LitigationRequestArbitrationReportExplanationResponseDto requestPayload);
        #endregion

        #region NominationList
        Task<BaseResponse> AddLitigationRequestArbitratorsNominationList(ArbitratorNominationListRequestPayload arbitratorNominationListRequestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsNominationList(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsNominationList(int pageIndex, int pageSize, CancellationToken cancellationToken);

        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsNominationListReply(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsNominationListReply(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsNominationListReply(ArbitratorNominationListRequestReplyPayload arbitratorNominationListRequestReplyPayload, CancellationToken cancellationToken);
        #endregion

        #region ChairmanNominationList
        Task<BaseResponse> AddLitigationRequestArbitratorsChairmanNominationList(ArbitratorNominationListRequestPayload arbitratorNominationListRequestPayload);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsChairmanNominationList(Guid requestId);
        Task<BaseResponse> GetLitigationRequestForAddArbitratorsChairmanNominationList(int pageIndex, int pageSize);
        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsChairmanNominationListReply(int pageIndex, int pageSize);
        Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsChairmanNominationListReply(Guid requestId);
        Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsChairmanNominationListReply(ArbitratorNominationListRequestReplyPayload arbitratorNominationListRequestReplyPayload);
        #endregion

        #region RecusalRequest
        Task<BaseResponse> AddArbitrationArbitratorRecusalRequest(LitigationRequestArbitrationArbitratorRecusalRequestCreateDto requestPayload);
        Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequest(Guid requestId);
        Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequest(int pageIndex, int pageSize);

        // Backoffice
        Task<BaseResponse> AddArbitrationReportArbitratorRecusalForArbitrator(LitigationRequestArbitrationRecusalRequestResponseDto requestPayload);
        Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequestForArbitratorByRequestId(Guid litigationRequestId);
        Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequestForArbitrator(int pageIndex, int pageSize);

        #endregion


        Task<BaseResponse> Assign(AssignArbitratorRequestPayload assignRequestPayload);
        Task<BaseResponse> GetAllArbitrationLitigationRequest(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }

    public partial class ArbitrationService : IArbitrationService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository _repository;
        private readonly IJwtReader _jwtReader;
        private readonly ISysSettingService _settingService;

        public ArbitrationService(IEntityRepository repository, ISysSettingService settingService,
                          IWorkFlowProcessingLogService workFlowProcessingLogService,
                          IMapper mapper,
                          IJwtReader jwtReader)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtReader = jwtReader;
            _settingService = settingService;
        }

        public async Task<BaseResponse> GetLitigationRequestForFullAllArbitrator(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                      .Include(x => x.ArbitratorCitizen)
                                      .Include(x => x.LitigationRequestArbitratorNominationLists)
                                      .Include(x => x.LitigationRequestArbitratorLists)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);

            litigationRequests = entitiesQuerable.Where(x => x.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation & x.IsFinished != true & x.LitigationRequestStage == LitigationRequestStage.Arbitration)
              .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.CreatedOn)
              .LastOrDefault().SessionDatetime)
              .AsSplitQuery()
            .AsNoTracking()
            .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    bool allowAdd = true;

                    if (!litigationRequest.LitigationRequestArbitratorNominationLists.Any())
                        allowAdd = false;

                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;


                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoDto>
                   {
                       Results = litigationRequestMircoDtos,
                       CurrentPage = litigationRequests.CurrentPage,
                       PageCount = litigationRequests.PageCount,
                       PageSize = litigationRequests.PageSize,
                       TotalCount = litigationRequests.TotalCount
                   },
                IsDone = true
            };

        }

        public async Task<BaseResponse> GetAllArbitrationLitigationRequest(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                      .Include(x => x.ArbitratorCitizen)
                                      .Include(x => x.LitigationRequestArbitratorLists)
                                      .ThenInclude(x => x.ArbitratorCitizen)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);

            litigationRequests = entitiesQuerable.Where(x => x.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation & x.IsFinished != true
            & x.LitigationRequestStage == LitigationRequestStage.Arbitration)
              .OrderByDescending(x => x.CreatedOn)
              .AsSplitQuery()
            .AsNoTracking()
            .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoArbitratorDto> litigationRequestMircoDtos = new List<LitigationRequestMircoArbitratorDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    bool allowAdd = true;



                    if (litigationRequest.ArbitratorCitizen != null)
                    {
                        LitigationRequestMircoArbitratorDto litigationRequestMircoDto = new LitigationRequestMircoArbitratorDto();
                        litigationRequestMircoDto.Id = litigationRequest.Id;
                        litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                        litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                        litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                        litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                        litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                        litigationRequestMircoDto.ArbitratorName = litigationRequest.ArbitratorCitizen?.FullName;
                        litigationRequestMircoDto.ArbitratoId = litigationRequest.ArbitratorCitizen?.Id;
                        litigationRequestMircoDto.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.ArbitratorsChairman;
                        litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                    }
                    litigationRequest.LitigationRequestArbitratorLists.ToList().ForEach(t =>
                    {
                        LitigationRequestMircoArbitratorDto litigationRequestMircoDto = new LitigationRequestMircoArbitratorDto();
                        litigationRequestMircoDto.Id = litigationRequest.Id;
                        litigationRequestMircoDto.LitigationRequestArbitratorListId = t.Id;
                        litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                        litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                        litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                        litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                        litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                        litigationRequestMircoDto.ArbitratorName = t.ArbitratorCitizen?.FullName;
                        litigationRequestMircoDto.ArbitratoId = t.ArbitratorCitizen?.Id;

                        litigationRequestMircoDto.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.Arbitrator;
                        litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                    });
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoArbitratorDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoArbitratorDto>
                   {
                       Results = litigationRequestMircoDtos,
                       CurrentPage = litigationRequests.CurrentPage,
                       PageCount = litigationRequests.PageCount,
                       PageSize = litigationRequests.PageSize,
                       TotalCount = litigationRequests.TotalCount
                   },
                IsDone = true
            };

        }

        public async Task<BaseResponse> Assign(AssignArbitratorRequestPayload assignRequestPayload)
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
                       .Include(x => x.LitigationRequestArbitratorLists)
                       .Where(x => x.Id == assignRequestPayload.RequestId).Single();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                if (litigationRequest.ArbitratorCitizenId.HasValue)
                {
                    if (!employee.AllowReAssign)
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "موظف غير مسموح له"
                        };
                }

                if (litigationRequest.LitigationRequestArbitratorLists.Any())
                {
                    if (litigationRequest.LitigationRequestArbitratorLists.Where(x => x.ArbitratorCitizenId == assignRequestPayload.ArbitratorId).Any())
                    {
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "المحكم بالفعل عضو بهيئة المحكمين بالدعوى"
                        };
                    }
                }

                var litigationRequestArbitratorAssigningRequests = (await _repository.GetQueryable<LitigationRequestArbitratorAssigningRequest, Guid>())?
                     .Where(x => x.LitigationRequestId == assignRequestPayload.RequestId & x.CitizenId == assignRequestPayload.ArbitratorId & x.Response == LitigationRequestArbitratorAssigningRequestStatus.None);

                if (litigationRequestArbitratorAssigningRequests.Any())
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "يوجد طلب مهمه سابق لم يتم الرد عليه حتى الان من قبل المحكم"
                    };
                }

                switch (assignRequestPayload.ArbitratorAssigningRequestTypeId)
                {
                    case ArbitratorAssigningRequestType.Arbitrator:
                        var litigationRequestArbitratorList = (await _repository.GetQueryable<LitigationRequestArbitratorList, Guid>())?
                             .Where(x => x.Id == assignRequestPayload.LitigationRequestArbitratorListId).Single();
                        _repository.Remove<LitigationRequestArbitratorList, Guid>(litigationRequestArbitratorList);
                        // Send Notification
                        // تم إلغاء تعيينك كمحكم على الدعوى رقم كذا
                        break;
                    case ArbitratorAssigningRequestType.ArbitratorsChairman:
                        litigationRequest.ArbitratorCitizenId = null;
                        // Send Notification
                        // تم إلغاء تعيينك كرئيس هيئة تحكيم على الدعوى رقم كذا
                        break;
                }

                LitigationRequestArbitratorAssigningRequest litigationRequestArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                litigationRequestArbitratorAssigningRequest.RequestOn = DateTime.Now;
                litigationRequestArbitratorAssigningRequest.CitizenId = assignRequestPayload.ArbitratorId;
                litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = assignRequestPayload.ArbitratorAssigningRequestTypeId;
                litigationRequestArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(litigationRequestArbitratorAssigningRequest);

                switch (assignRequestPayload.ArbitratorAssigningRequestTypeId)
                {
                    case ArbitratorAssigningRequestType.Arbitrator:
                        // Send Notification
                        // تم تعيينك كمحكم على الدعوى رقم كذا
                        break;
                    case ArbitratorAssigningRequestType.ArbitratorsChairman:
                        // Send Notification
                        // تم تعيينك كرئيس هيئة تحكيم على الدعوى رقم كذا
                        break;
                }

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


    }
}
