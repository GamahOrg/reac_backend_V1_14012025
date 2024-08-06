using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Reconciliation;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.ModuleServices
{
    public class ReconciliationService : IReconciliationService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository _repository;
        private readonly IJwtReader _jwtReader;
        private readonly ISysSettingService _settingService;

        public ReconciliationService(IEntityRepository repository, ISysSettingService settingService,
                          IWorkFlowProcessingLogService workFlowProcessingLogService,
                          IMapper mapper,
                          IJwtReader jwtReader)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtReader = jwtReader;
            _settingService = settingService;
        }

        #region Session
        public async Task<BaseResponse> AddReconciliationSession(ScheduleSessionProcessRequestPayload requestPayload, CancellationToken cancellationToken)
        {
            try
            {
                Guid employeeId = Guid.Empty;
                _jwtReader.GetReferenceId(ref employeeId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                        .Where(x => x.Id == requestPayload.RequestId)
                        .Single();

                bool allowAdd = true;

                if (litigationRequest.IsFinished.HasValue)
                {
                    if (litigationRequest.IsFinished.Value)
                        allowAdd = false;
                }

                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                    allowAdd = false;

                if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestReportAdded)
                    allowAdd = false;


                int sessionCountValidation = 3;
                try
                {
                    sessionCountValidation = int.Parse((await _settingService.GetBySettingKey(SysSettingKeyConstants.Litigation_Request_Max_Session_ReScheduleTime)).Data?.Value);
                }
                catch { }
                if (litigationRequest.LitigationRequestSessions != null)
                {
                    if (litigationRequest.LitigationRequestSessions.Count(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation) >= sessionCountValidation)
                        allowAdd = false;
                    else
                    {
                        if (litigationRequest.LitigationRequestSessions.Count == 0)
                        {
                            litigationRequest.InternalStatusId = RequestStatus.AsessionTimeHasBeenSet;
                            litigationRequest.ExternalStatusId = RequestStatus.AsessionTimeHasBeenSet;
                        }
                        else
                        {
                            litigationRequest.InternalStatusId = RequestStatus.AsessionTimeHasBeenReset;
                            litigationRequest.ExternalStatusId = RequestStatus.AsessionTimeHasBeenReset;
                        }
                    }


                }
                if (allowAdd)
                {
                    LitigationRequestSession litigationRequestSession = new LitigationRequestSession()
                    {
                        Id = Guid.NewGuid(),
                        LitigationRequestId = requestPayload.RequestId,
                        SessionDatetime = requestPayload.SessionDateTime,
                        CreatedOn = DateTime.Now,
                        LitigationRequestSessionRequestTypeId = LitigationRequestSessionRequestType.Reconciliation
                    };


                    await _repository.AddAsync<LitigationRequestSession, Guid>(litigationRequestSession);

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;
                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة جلسة جديدة"
                    };

                }
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestForSession(requestPayload.RequestId, cancellationToken);

        }
        public async Task<BaseResponse> GetLitigationRequestForSession(Guid requestId, CancellationToken cancellationToken)
        {
            if (requestId == Guid.Empty)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.Area)
                          .Include(x => x.City)
                          .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                          .Include(x => x.LitigationRequestReconciliationReports)
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.InternalStatus)
                          .Include(x => x.ExternalStatus)
                          .Include(x => x.LitigationRequestAmount)
                          .Include(x => x.LitigationRequestCategory)
                          .Include(x => x.SysExemptionReductionCategory)
                          .Include(x => x.LitigationRequestOwnerType)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.ContractFile)
                          .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                          .Include(x => x.Contract.FoundingLetterFile)
                          .Include(x => x.Contract.InternalRegulationsFile)
                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Include(x => x.Contract.LitigationRequestContractType)
                          .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            if (litigationRequest == null)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var sessions = _mapper.Map<List<LitigationRequestSessionCreateUpdateDto>>(litigationRequest?.LitigationRequestSessions);

            bool allowAdd = true;
            bool isDone = false;

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                    allowAdd = false;
                else
                {

                    if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestReportAdded)
                        allowAdd = false;
                    else
                    {
                        int sessionCountValidation = 3;
                        try
                        {
                            sessionCountValidation = int.Parse((await _settingService.GetBySettingKey(SysSettingKeyConstants.Litigation_Request_Max_Session_ReScheduleTime)).Data?.Value);
                        }
                        catch { }
                        if (litigationRequest.LitigationRequestSessions.Count(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation) >= sessionCountValidation)
                            allowAdd = false;
                        else
                        {
                            if (litigationRequest?.LitigationRequestReconciliationReports != null)
                            {
                                if (litigationRequest?.LitigationRequestReconciliationReports.Count > 0)
                                {
                                    allowAdd = false;
                                }
                            }
                        }
                    }
                }
            }

            return new DataResponse<LitigationRequestForSessionDto>()
            {
                Data = new LitigationRequestForSessionDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    Sessions = (sessions.Count > 0) ? sessions : null
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForSession(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                      .Include(x => x.Area)
                                      .Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Where(x => x.ReformerCitizenId == referenceId & x.RequestCode != null & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration).ToList();

            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool isDone = false;
                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }
                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                            allowAdd = false;
                        else
                        {

                            if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestReportAdded)
                                allowAdd = false;
                            else
                            {
                                int sessionCountValidation = 3;
                                try
                                {
                                    sessionCountValidation = int.Parse((await _settingService.GetBySettingKey(SysSettingKeyConstants.Litigation_Request_Max_Session_ReScheduleTime)).Data?.Value);
                                }
                                catch { }
                                if (litigationRequest.LitigationRequestSessions.Count(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation) >= sessionCountValidation)
                                    allowAdd = false;
                                else
                                {
                                    if (litigationRequest?.LitigationRequestReconciliationReports != null)
                                    {
                                        if (litigationRequest?.LitigationRequestReconciliationReports.Count > 0)
                                        {
                                            allowAdd = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);

                }
            }

            return new DataResponse<List<LitigationRequestMircoDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForAllSession(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                         .Include(x => x.Area)
                                      .Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);
            litigationRequests = entitiesQuerable
                .Where(x =>


                x.RequestCode != null
                & x.LitigationRequestSessions.Count > 0
                & x.ApplicantCitizenId == referenceId
                & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration

                |
                x.RequestCode != null
                & x.LitigationRequestSessions.Count > 0 & x.RequestCode != null
                & x.Contract.FirstPartyCitizenId == referenceId
                & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration

                |
                x.RequestCode != null
                & x.LitigationRequestSessions.Count > 0
                & x.RequestCode != null
                & x.Contract.SecondPartyCitizenId == referenceId
                & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration


                |

                x.RequestCode != null
                & x.LitigationRequestSessions.Count > 0
                & x.RequestCode != null
                & x.Contract.FirstPartyFacility.CitizenId == referenceId
                & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration


                |

                x.RequestCode != null
                & x.LitigationRequestSessions.Count > 0
                & x.RequestCode != null
                & x.Contract.SecondPartyFacility.CitizenId == referenceId
                & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration)
                                        .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.CreatedOn)
                                        .LastOrDefault().SessionDatetime)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoForRequestSessionDto> litigationRequestMircoDtos = new List<LitigationRequestMircoForRequestSessionDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoForRequestSessionDto litigationRequestMircoDto = new LitigationRequestMircoForRequestSessionDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.SessionDatetime = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionDatetime;
                    litigationRequestMircoDto.Url = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionUrl;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoForRequestSessionDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoForRequestSessionDto>
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
        public async Task<BaseResponse> GetLitigationRequestForAllSessionForReformer(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                         .Include(x => x.Area)
                                      .Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);
            litigationRequests = entitiesQuerable
                .Where(x => x.ReformerCitizenId == referenceId & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration)
                                        .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.CreatedOn)
                                        .LastOrDefault().SessionDatetime)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoForRequestSessionDto> litigationRequestMircoDtos = new List<LitigationRequestMircoForRequestSessionDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoForRequestSessionDto litigationRequestMircoDto = new LitigationRequestMircoForRequestSessionDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.SessionDatetime = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionDatetime;
                    litigationRequestMircoDto.Url = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionUrl;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoForRequestSessionDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoForRequestSessionDto>
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
        public async Task<BaseResponse> GetLitigationRequestForFullAllSession(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                         .Include(x => x.Area)
                                      .Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);
            litigationRequests = entitiesQuerable.Where(x => x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration)
                                        .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.CreatedOn)
                                        .LastOrDefault().SessionDatetime)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoForRequestSessionDto> litigationRequestMircoDtos = new List<LitigationRequestMircoForRequestSessionDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoForRequestSessionDto litigationRequestMircoDto = new LitigationRequestMircoForRequestSessionDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.SessionDatetime = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionDatetime;
                    litigationRequestMircoDto.Url = litigationRequest.LitigationRequestSessions.OrderBy(x => x.SessionDatetime).Last().SessionUrl;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoForRequestSessionDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoForRequestSessionDto>
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
        public async Task<BaseResponse> GetLitigationRequestForFullAllReformer(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                                         .Include(x => x.ReformerCitizen)

                                         .Include(x => x.Area)
                                      .Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions.Where(x => x.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation))
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType);

            litigationRequests = entitiesQuerable.Where(x => x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration & x.IsFinished != true & x.LitigationRequestStage == LitigationRequestStage.Reconciliation)
              .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.CreatedOn)
              .LastOrDefault().SessionDatetime)
              .AsSplitQuery()
              .AsNoTracking()
              .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoForRequestReformerDto> litigationRequestMircoDtos = new List<LitigationRequestMircoForRequestReformerDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoForRequestReformerDto litigationRequestMircoDto = new LitigationRequestMircoForRequestReformerDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.ReformerId = litigationRequest.ReformerCitizen?.Id;
                    litigationRequestMircoDto.ReformerName = litigationRequest.ReformerCitizen?.FullName;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<LitigationRequestMircoForRequestReformerDto>>
            {
                Data =
                   new PagedResult<LitigationRequestMircoForRequestReformerDto>
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

        #endregion

        #region Report
        public async Task<BaseResponse> AddReconciliationReport(WritingMinutesOfTheMeetingRequestPayload requestPayload, CancellationToken cancellationToken)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestReconciliationReports)
                        .Where(x => x.Id == requestPayload.RequestId & x.ReformerCitizenId == referenceId)
                        .Single();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                bool allowAdd = true;

                if (litigationRequest.LitigationRequestReconciliationReports.Count() > 0)
                    allowAdd = false;

                if (allowAdd)
                {
                    LitigationRequestReconciliationReport litigationRequestReconciliationReport = new LitigationRequestReconciliationReport()
                    {
                        Id = Guid.NewGuid(),
                        ReportDate = DateTime.Now,
                        ReconciliationSessionDetails = requestPayload.ReconciliationSessionDetails,
                        ModificationRequest = requestPayload.ModificationRequest,
                        LitigationRequestId = requestPayload.RequestId,
                    };

                    if (requestPayload.LitigationRequestMeetingReconciliationItems != null)
                    {
                        requestPayload.LitigationRequestMeetingReconciliationItems.ToList().ForEach(t =>
                        {
                            litigationRequestReconciliationReport.LitigationRequestMeetingReconciliationItems.Add(new LitigationRequestReconciliationReportItem()
                            {
                                Id = Guid.NewGuid(),
                                LitigationRequestReconciliationReportId = litigationRequestReconciliationReport.Id,
                                ReconciliationItem = t.ReconciliationItem
                            });


                        });
                    }

                    litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReportAdded;
                    litigationRequest.InternalStatusId = RequestStatus.TheMinutesOfTheMeetingWereWritten;
                    litigationRequest.ExternalStatusId = RequestStatus.TheMinutesOfTheMeetingWereWritten;
                    litigationRequest.IsMinutesOfMeetingApprovalRequired = true;

                    await _repository.AddAsync<LitigationRequestReconciliationReport, Guid>(litigationRequestReconciliationReport);

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;

                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة محضر جديد"
                    };

                }
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestForReport(requestPayload.RequestId, cancellationToken);
        }
        public async Task<BaseResponse> GetLitigationRequestForReport(Guid requestId, CancellationToken cancellationToken)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestSessions)
                         .Include(x => x.LitigationRequestReconciliationReports)
                         .ThenInclude(x => x.LitigationRequestMeetingReconciliationItems)

                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)

                         .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var reports = _mapper.Map<List<LitigationRequestReconciliationReportCreateUpdateDto>>(litigationRequest?.LitigationRequestReconciliationReports);

            bool isDone = false;
            bool allowAdd = true;
            bool allowDecision = true;


            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    allowDecision = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                {
                    allowAdd = false;
                    allowDecision = false;
                }
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded)
                    {
                        allowAdd = false;
                    }
                    else
                    {
                        if (litigationRequest?.LitigationRequestReconciliationReports != null)
                        {
                            if (litigationRequest.LitigationRequestReconciliationReports.Count > 0)
                                allowAdd = false;
                        }
                    }
                }
            }
            if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded)
            {
                allowDecision = false;
            }

            return new DataResponse<LitigationRequestForReportDto>()
            {
                Data = new LitigationRequestForReportDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    AllowDecision = allowDecision,
                    Report = reports.FirstOrDefault()
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForReport(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions)
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen).Include(x => x.Contract.LitigationRequestContractType)

                                      .Where(x => x.ReformerCitizenId == referenceId & x.RequestCode != null & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration).ToList();

            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool isDone = false;

                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }
                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                            allowAdd = false;
                        else
                        {

                            if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded)
                                allowAdd = false;
                            else
                            {
                                if (litigationRequest?.LitigationRequestReconciliationReports != null)
                                {
                                    if (litigationRequest?.LitigationRequestReconciliationReports.Count > 0)
                                    {
                                        allowAdd = false;
                                    }
                                }
                            }
                        }
                    }
                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);

                }
            }


            return new DataResponse<List<LitigationRequestMircoDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };
        }
        #endregion

        #region Extend Period Request
        public async Task<BaseResponse> AddReconciliationExtendPeriod(LitigationRequestExtendPeriodRequestCreateUpdateDto requestPayload, CancellationToken cancellationToken)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestExtendPeriodRequests.Where(x => x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation))
                        .Where(x => x.Id == requestPayload.LitigationRequestId & x.ReformerCitizenId == referenceId)
                        .Single();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                bool allowAdd = true;

                if (litigationRequest.LitigationRequestExtendPeriodRequests.Count() > 0)
                    allowAdd = false;

                if (allowAdd)
                {
                    LitigationRequestExtendPeriodRequest litigationRequestExtendPeriodRequest = new LitigationRequestExtendPeriodRequest()
                    {
                        Id = requestPayload.LitigationRequestId,
                        LitigationRequestId = requestPayload.LitigationRequestId,
                        RequiredDays = requestPayload.RequiredDays,
                        RequestReason = requestPayload.RequestReason,
                        LitigationRequestExtendPeriodRequestTypeId = LitigationRequestExtendPeriodRequestType.Reconciliation
                    };

                    if (requestPayload.Files != null)
                    {
                        requestPayload.Files.ToList().ForEach(t =>
                        {
                            litigationRequestExtendPeriodRequest.LitigationRequestExtendPeriodRequestFiles.Add(new LitigationRequestExtendPeriodRequestFile()
                            {
                                Id = Guid.NewGuid(),
                                FileManagerId = t,
                            });


                        });
                    }

                    await _repository.AddAsync<LitigationRequestExtendPeriodRequest, Guid>(litigationRequestExtendPeriodRequest);

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;

                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة طلب جديد"
                    };

                }
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestExtendPeriod(requestPayload.LitigationRequestId, cancellationToken);
        }
        public async Task<BaseResponse> AddReconciliationExtendPeriodForEmployee(LitigationRequestExtendPeriodRequestCreateUpdateEmployeeDto requestPayload, CancellationToken cancellationToken)
        {
            try
            {
                var litigationRequestExtendPeriodRequest = (await _repository.GetQueryable<LitigationRequestExtendPeriodRequest, Guid>())?
                        .Where(x => x.LitigationRequestId == requestPayload.LitigationRequestId & x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation)
                        .SingleOrDefault();


                if (litigationRequestExtendPeriodRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };
                litigationRequestExtendPeriodRequest.LitigationRequestExtendPeriodRequestStatusId = requestPayload.LitigationRequestExtendPeriodRequestStatusId;
                litigationRequestExtendPeriodRequest.FinishStatusReason = requestPayload.FinishStatusReason;

                var savePeriodResponse = await _repository.SaveChangesAsync();
                if (!savePeriodResponse.IsDone)
                    return savePeriodResponse;
                //var updateResponse = _repository.Update<LitigationRequestExtendPeriodRequest, Guid>(litigationRequestExtendPeriodRequest);
                //if (!updateResponse.IsDone)
                //    return updateResponse;

                if (requestPayload.LitigationRequestExtendPeriodRequestStatusId == LitigationRequestExtendPeriodRequestStatus.Approve)
                {
                    var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                       .Where(x => x.Id == requestPayload.LitigationRequestId)
                       .SingleOrDefault();

                    litigationRequest.ReconciliationPeriod += litigationRequestExtendPeriodRequest.RequiredDays;

                    savePeriodResponse = await _repository.SaveChangesAsync();
                    if (!savePeriodResponse.IsDone)
                        return savePeriodResponse;
                }

                // Send Notification

                return await GetLitigationRequestExtendPeriodForEmployee(requestPayload.LitigationRequestId, cancellationToken);
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestExtendPeriod(requestPayload.LitigationRequestId, cancellationToken);
        }

        public async Task<BaseResponse> GetLitigationRequestExtendPeriod(Guid requestId, CancellationToken cancellationToken)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestExtendPeriodRequests)
                         .ThenInclude(x => x.LitigationRequestExtendPeriodRequestFiles)
                         .ThenInclude(x => x.FileManager)
                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)
                         .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var extendPeriodRequest = _mapper.Map<List<LitigationRequestExtendPeriodRequestGetDto>>(litigationRequest?.LitigationRequestExtendPeriodRequests);

            bool isDone = false;
            bool allowAdd = true;

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestExtendPeriodRequests != null)
                    {
                        if (litigationRequest.LitigationRequestExtendPeriodRequests.Count(x => x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation) > 0)
                            allowAdd = false;
                    }
                }
            }

            return new DataResponse<LitigationRequestForExtendPeriodRequestDto>()
            {
                Data = new LitigationRequestForExtendPeriodRequestDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    LitigationRequestExtendPeriodRequestCreateUpdateDto = extendPeriodRequest.FirstOrDefault()
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestExtendPeriodForEmployee(Guid requestId, CancellationToken cancellationToken)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestExtendPeriodRequests)
                         .ThenInclude(x => x.LitigationRequestExtendPeriodRequestFiles)
                         .ThenInclude(x => x.FileManager)
                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)
                         .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var extendPeriodRequest = _mapper.Map<List<LitigationRequestExtendPeriodRequestGetDto>>(litigationRequest?.LitigationRequestExtendPeriodRequests.Where(x => x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation));

            bool isDone = false;
            bool allowAdd = true;

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestExtendPeriodRequests != null)
                    {
                        if (litigationRequest.LitigationRequestExtendPeriodRequests.Count > 0)
                        {
                            if (litigationRequest.LitigationRequestExtendPeriodRequests.FirstOrDefault()?.LitigationRequestExtendPeriodRequestStatusId != null)
                                allowAdd = false;
                        }
                    }
                }
            }

            return new DataResponse<LitigationRequestForExtendPeriodRequestDto>()
            {
                Data = new LitigationRequestForExtendPeriodRequestDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    LitigationRequestExtendPeriodRequestCreateUpdateDto = extendPeriodRequest.FirstOrDefault()
                },
                IsDone = true,
            };

        }

        public async Task<BaseResponse> GetLitigationRequestExtendPeriod(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                      .Include(x => x.LitigationRequestExtendPeriodRequests)
                                      .ThenInclude(x => x.LitigationRequestExtendPeriodRequestFiles)
                                      .ThenInclude(x => x.FileManager)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen).Include(x => x.Contract.LitigationRequestContractType)

                                      .Where(x => x.ReformerCitizenId == referenceId & x.RequestCode != null & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration).ToList();

            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool isDone = false;
                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }
                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                            allowAdd = false;
                        else
                        {
                            if (litigationRequest?.LitigationRequestExtendPeriodRequests != null)
                            {
                                if (litigationRequest.LitigationRequestExtendPeriodRequests.Count(x => x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation) > 0)
                                    allowAdd = false;
                            }
                        }
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);

                }
            }

            return new DataResponse<List<LitigationRequestMircoDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };
        }
        public async Task<BaseResponse> GetLitigationRequestExtendPeriodForEmployee(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {

            PagedResult<LitigationRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                  .Include(x => x.LitigationRequestExtendPeriodRequests)
                                      .ThenInclude(x => x.LitigationRequestExtendPeriodRequestFiles)
                                      .ThenInclude(x => x.FileManager)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen).Include(x => x.Contract.LitigationRequestContractType);


            litigationRequests = entitiesQuerable.Where(x => x.RequestCode != null & x.LitigationRequestExtendPeriodRequests.Count() > 0)
                                       .OrderByDescending(x => x.CreatedOn)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .GetPaged(pageIndex, pageSize);


            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool isDone = false;
                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }
                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                            allowAdd = false;
                        else
                        {
                            if (litigationRequest?.LitigationRequestExtendPeriodRequests != null)
                            {
                                if (litigationRequest.LitigationRequestExtendPeriodRequests.Count(x => x.LitigationRequestExtendPeriodRequestTypeId == LitigationRequestExtendPeriodRequestType.Reconciliation) > 0)
                                {
                                    if (litigationRequest.LitigationRequestExtendPeriodRequests.FirstOrDefault()?.LitigationRequestExtendPeriodRequestStatusId != null)
                                        allowAdd = false;
                                }
                                else
                                {
                                    allowAdd = false;
                                }
                            }
                            else
                            {
                                allowAdd = false;
                            }
                        }
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest?.InternalStatus?.RequestStatus;
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

        #endregion

        #region Citizen Report Reply
        public async Task<BaseResponse> GetCitizenLitigationRequestForReportReply(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                      .Include(x => x.LitigationRequestSessions)
                                      .Include(x => x.LitigationRequestReconciliationReports)
                                      .Include(x => x.ApplicantCitizen)
                                      .Include(x => x.InternalStatus)
                                      .Include(x => x.ExternalStatus)
                                      .Include(x => x.LitigationRequestAmount)
                                      .Include(x => x.LitigationRequestCategory)
                                      .Include(x => x.SysExemptionReductionCategory)
                                      .Include(x => x.LitigationRequestOwnerType)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility)
                                      .Include(x => x.Contract.SecondPartyCitizen).Include(x => x.Contract.LitigationRequestContractType)
                                      .Where(x => x.RequestCode != null & x.ApplicantCitizenId == referenceId
                                      | x.LitigationRequestReconciliationReports.Count > 0 & x.RequestCode != null & x.Contract.FirstPartyCitizenId == referenceId & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration
                                      | x.LitigationRequestReconciliationReports.Count > 0 & x.RequestCode != null & x.Contract.SecondPartyCitizenId == referenceId & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration
                                      | x.LitigationRequestReconciliationReports.Count > 0 & x.RequestCode != null & x.Contract.FirstPartyFacility.CitizenId == referenceId & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration
                                      | x.LitigationRequestReconciliationReports.Count > 0 & x.RequestCode != null & x.Contract.SecondPartyFacility.CitizenId == referenceId & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration).ToList();

            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool IsDone = false;
                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            IsDone = true;
                        }
                    }
                    if (!IsDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                            allowAdd = false;
                        else
                        {

                            if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded)
                                allowAdd = false;
                            else
                            {

                                if (litigationRequest.ApplicantCitizenId == referenceId)
                                {
                                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                        allowAdd = false;

                                }
                                if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                                {
                                    if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                                    {
                                        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                            allowAdd = false;
                                    }
                                }
                                if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                                {
                                    if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                                    {
                                        if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                            allowAdd = false;
                                    }
                                }
                                if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                                {
                                    if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                                    {
                                        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                            allowAdd = false;
                                    }
                                }
                                if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                                {
                                    if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                                    {
                                        if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                            allowAdd = false;
                                    }
                                }
                            }
                        }
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);

                }
            }

            return new DataResponse<List<LitigationRequestMircoDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };
        }
        public async Task<BaseResponse> GetCitizenLitigationRequestForReportReply(Guid requestId, CancellationToken cancellationToken)
        {

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestSessions)
                         .Include(x => x.LitigationRequestReconciliationReports)
                         .ThenInclude(x => x.LitigationRequestMeetingReconciliationItems)
                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)

                         .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var reports = _mapper.Map<List<LitigationRequestReconciliationReportCreateUpdateDto>>(litigationRequest?.LitigationRequestReconciliationReports);

            LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();

            bool allowAdd = true;
            bool IsDone = false;
            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    IsDone = true;
                }
            }
            if (!IsDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Reconciliation)
                    allowAdd = false;
                else
                {

                    if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded)
                        allowAdd = false;
                    else
                    {

                        if (litigationRequest.ApplicantCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                allowAdd = false;

                        }
                        if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                    allowAdd = false;
                            }
                        }

                    }
                }
            }

            return new DataResponse<LitigationRequestForReportDto>()
            {
                Data = new LitigationRequestForReportDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    Report = reports.FirstOrDefault()
                },
                IsDone = true,
            };
        }
        public async Task<BaseResponse> AddCitizenLitigationRequestForReportReply(Guid requestId, MinutesOfMeetingApprovalStatus requestMinutesOfMeetingApprovalStatus, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            bool isDone = false;
            if (litigationRequest != null)
            {
                if (litigationRequest.ApplicantCitizen != null)
                {
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        if (!litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                        {
                            litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus = requestMinutesOfMeetingApprovalStatus;
                            litigationRequest.MintuesOfMeetingApprovalPlaintiffStatusOn = DateTime.Now;
                            isDone = true;
                        }
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            if (!litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                            {
                                litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus = requestMinutesOfMeetingApprovalStatus;
                                litigationRequest.MintuesOfMeetingApprovalPlaintiffStatusOn = DateTime.Now;
                                isDone = true;
                            }
                        }
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            if (!litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                            {
                                litigationRequest.MintuesOfMeetingApprovalDefendantStatus = requestMinutesOfMeetingApprovalStatus;
                                litigationRequest.MintuesOfMeetingApprovalDefendantStatusOn = DateTime.Now;
                                isDone = true;
                            }
                        }
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyFacility != null)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility.CitizenId == referenceId)
                        {
                            if (!litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                            {
                                litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus = requestMinutesOfMeetingApprovalStatus;
                                litigationRequest.MintuesOfMeetingApprovalPlaintiffStatusOn = DateTime.Now;
                                isDone = true;
                            }
                        }
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyFacility != null)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility.CitizenId == referenceId)
                        {
                            if (!litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                            {
                                litigationRequest.MintuesOfMeetingApprovalDefendantStatus = requestMinutesOfMeetingApprovalStatus;
                                litigationRequest.MintuesOfMeetingApprovalDefendantStatusOn = DateTime.Now;
                                isDone = true;
                            }
                        }
                    }
                }
            }

            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue & litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
            {
                litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestPartiesDecisionAdded;
            }

            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue & litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
            {
                if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus == MinutesOfMeetingApprovalStatus.Approve & litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus == MinutesOfMeetingApprovalStatus.Approve)
                {
                    litigationRequest.InternalStatusId = RequestStatus.AcceptanceOfTheReconciliationReport;
                    litigationRequest.ExternalStatusId = RequestStatus.AcceptanceOfTheReconciliationReport;
                }
                else
                {
                    litigationRequest.InternalStatusId = RequestStatus.RejectionOfTheReconciliationReport;
                    litigationRequest.ExternalStatusId = RequestStatus.RejectionOfTheReconciliationReport;
                }
            }

            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            return await GetCitizenLitigationRequestForReportReply(requestId, cancellationToken);

        }
        #endregion

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
                        Message = "طلب غير موجود"
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
                    }
                }

                LitigationRequestReformerAssigningRequest litigationRequestReformerAssigningRequest = new LitigationRequestReformerAssigningRequest();
                litigationRequestReformerAssigningRequest.CitizenId = assignRequestPayload.ReformerId;
                litigationRequestReformerAssigningRequest.LitigationRequestId = litigationRequest.Id;
                litigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.None;
                await _repository.AddAsync<LitigationRequestReformerAssigningRequest, Guid>(litigationRequestReformerAssigningRequest);

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
                    break;
                case LitigationRequestDecision.ReconciliationWasMade:
                    litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasMade;
                    litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasMade;
                    litigationRequests.FirstOrDefault().IsActive = false;
                    litigationRequests.FirstOrDefault().IsFinished = true;
                    litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReformerDecisionAdded;
                    break;
                case LitigationRequestDecision.WaiverOfTheLitigation:
                    litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.WaiverOfTheLitigation;
                    litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.WaiverOfTheLitigation;
                    litigationRequests.FirstOrDefault().IsActive = false;
                    litigationRequests.FirstOrDefault().IsFinished = true;
                    litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestReformerDecisionAdded;
                    break;
                case LitigationRequestDecision.ReconciliationWasNotMade:

                    if (litigationRequests.FirstOrDefault().LitigationRequestCategoryId == LitigationRequestCategory.ReconciliationAndArbitration)
                    {
                        litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasNotMadeAndMoveToArbitration;
                        litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasNotMadeAndMoveToArbitration;
                        litigationRequests.FirstOrDefault().StartArbitrationDate = DateTime.Now;
                        litigationRequests.FirstOrDefault().LitigationRequestStage = LitigationRequestStage.Arbitration;
                        litigationRequests.FirstOrDefault().LitigationRequestSubStage = LitigationRequestSubStage.ArbitratorSelectionPeriod;
                    }
                    else
                    {
                        litigationRequests.FirstOrDefault().InternalStatusId = RequestStatus.ReconciliationWasNotMade;
                        litigationRequests.FirstOrDefault().ExternalStatusId = RequestStatus.ReconciliationWasNotMade;
                        litigationRequests.FirstOrDefault().IsActive = false;
                        litigationRequests.FirstOrDefault().IsFinished = true;
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
