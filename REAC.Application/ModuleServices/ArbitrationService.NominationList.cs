using Core.GenericRepository.Shared.Pagging;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Arbitration;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Arbitration;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.ModuleServices
{
    public partial class ArbitrationService
    {
        #region Nomination List
        public async Task<BaseResponse> AddLitigationRequestArbitratorsNominationList(ArbitratorNominationListRequestPayload arbitratorNominationListRequestPayload, CancellationToken cancellationToken)
        {
            try
            {
                if (arbitratorNominationListRequestPayload.Arbitrators.Count == 0)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لابد من إضافة إعضاء هيئة المحكمين"
                    };

                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestArbitratorNominationLists)
                        .Where(x => x.Id == arbitratorNominationListRequestPayload.RequestId)
                        .SingleOrDefault();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };


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
                    if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                    {
                        allowAdd = false;
                    }
                    else
                    {
                        if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorListNomination)
                            allowAdd = false;
                    }
                }
                if (litigationRequest.LitigationRequestArbitratorNominationLists.Any())
                    allowAdd = false;

                if (allowAdd)
                {

                    arbitratorNominationListRequestPayload.Arbitrators.ForEach(async t =>
                    {
                        LitigationRequestArbitratorNominationList litigationRequestArbitratorNominationList = new LitigationRequestArbitratorNominationList();
                        litigationRequestArbitratorNominationList.ArbitratorCitizenId = t;
                        litigationRequestArbitratorNominationList.LitigationRequestId = arbitratorNominationListRequestPayload.RequestId;
                        litigationRequestArbitratorNominationList.Id = Guid.NewGuid();
                        await _repository.AddAsync<LitigationRequestArbitratorNominationList, Guid>(litigationRequestArbitratorNominationList);
                    });
                    litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitratorListNominationDone;
                    litigationRequest.ArbitratorsNominationListCreationDateTime = DateTime.Now;

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;
                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة عدد المحكمين"
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
            return await GetLitigationRequestForAddArbitratorsNominationList(arbitratorNominationListRequestPayload.RequestId, cancellationToken);
        }
        public async Task<BaseResponse> GetLitigationRequestForAddArbitratorsNominationList(Guid requestId, CancellationToken cancellationToken)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                            .Include(x => x.LitigationRequestArbitratorAssigningRequests)

                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

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
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                {
                    allowAdd = false;
                }
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorListNomination)
                        allowAdd = false;
                }
            }
            LitigationRequestForAllArbitratorNominationListDto litigationRequestForAllArbitratorNominationListDto = new LitigationRequestForAllArbitratorNominationListDto();
            litigationRequestForAllArbitratorNominationListDto.LitigationRequestShortDto = request;
            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos = new List<CitizenArbitratorShortStatusDto>();

            if (litigationRequest.LitigationRequestArbitratorNominationLists.Any())
            {
                allowAdd = false;
                foreach (var litigationRequestArbitratorNominationList in litigationRequest.LitigationRequestArbitratorNominationLists)
                {
                    CitizenArbitratorShortStatusDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusDto();
                    citizenArbitratorShortStatusDto.FullName = litigationRequestArbitratorNominationList.ArbitratorCitizen.FullName;
                    citizenArbitratorShortStatusDto.Mobile = litigationRequestArbitratorNominationList.ArbitratorCitizen.Mobile;
                    citizenArbitratorShortStatusDto.Email = litigationRequestArbitratorNominationList.ArbitratorCitizen.Email;
                    citizenArbitratorShortStatusDto.PlaintiffPriority = litigationRequestArbitratorNominationList.PlaintiffPriority;
                    citizenArbitratorShortStatusDto.DefendantPriority = litigationRequestArbitratorNominationList.DefendantPriority;

                    citizenArbitratorShortStatusDto.ArbitratorClass =
                        litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.SysArbitratorClass?.ArbitratorClassName;

                    citizenArbitratorShortStatusDto.Brief =
                        litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                    citizenArbitratorShortStatusDto.ExperienceYears =
                        litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.ExperienceYears;
                    citizenArbitratorShortStatusDto.RequestsCount = litigationRequestArbitratorNominationList.ArbitratorCitizen?.ArbitratorLitigationRequests.Count() + litigationRequestArbitratorNominationList.ArbitratorCitizen.LitigationRequestArbitratorLists.Count;
                    citizenArbitratorShortStatusDto.IdentityNumber = litigationRequestArbitratorNominationList.ArbitratorCitizen.IdentityNumber;

                    var assingingRequest = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(t => t.LitigationRequestId == litigationRequest.Id
                                                            & t.CitizenId == litigationRequestArbitratorNominationList.ArbitratorCitizenId & t.ArbitratorAssigningRequestTypeId == ArbitratorAssigningRequestType.Arbitrator);

                    if (assingingRequest != null)
                    {
                        if (assingingRequest.Any())
                        {
                            if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Approve)
                                citizenArbitratorShortStatusDto.Status = "تم الموافقة";
                            else
                            {
                                if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Reject)
                                    citizenArbitratorShortStatusDto.Status = "تم الرفض";
                                else
                                {
                                    if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Cancelled)
                                        citizenArbitratorShortStatusDto.Status = "تم الإلغاء";
                                    else
                                    {
                                        citizenArbitratorShortStatusDto.Status = "قيد الرد";
                                    }
                                }
                            }
                        }
                    }
                    litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
                }
                return new DataResponse<LitigationRequestForAllArbitratorNominationListDto>()
                {
                    Data = litigationRequestForAllArbitratorNominationListDto,
                    IsDone = true,
                };
            }

            return new DataResponse<LitigationRequestForAllArbitratorNominationListDto>()
            {
                Data = new LitigationRequestForAllArbitratorNominationListDto()
                {
                    ListCount = (await _repository.GetQueryable<SysArbitratorNominationListCount, Guid>()).Where(x => x.ArbitratorCount == litigationRequest.ArbitratorsCount).FirstOrDefault()?.ArbitratorNominationListCount,
                    AllowAdd = allowAdd,
                    LitigationRequestShortDto = request
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForAddArbitratorsNominationList(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                            .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType);


            litigationRequests = entitiesQuerable.Where(x => x.RequestCode != null
            & x.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation & x.LitigationRequestStage == LitigationRequestStage.Arbitration)
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
                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;

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
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                            allowAdd = false;
                        else
                        {

                            if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorListNomination)
                            {
                                allowAdd = false;
                            }
                        }
                    }
                    if (litigationRequest.LitigationRequestArbitratorNominationLists.Count > 0)
                        allowAdd = false;


                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
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


        public async Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsNominationListReply(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);


            PagedResult<LitigationRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
               .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType);


            litigationRequests = entitiesQuerable.Where(x => x.RequestCode != null
                                      & x.ApplicantCitizenId == referenceId
                                      & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                      & x.LitigationRequestArbitratorNominationLists.Count > 0
                                      | x.RequestCode != null
                                      & x.Contract.FirstPartyCitizenId == referenceId
                                      & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                      & x.LitigationRequestArbitratorNominationLists.Count > 0
                                      | x.RequestCode != null
                                      & x.Contract.SecondPartyCitizenId == referenceId
                                      & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                      & x.LitigationRequestArbitratorNominationLists.Count > 0
                                      | x.RequestCode != null
                                      & x.Contract.FirstPartyFacility.CitizenId == referenceId
                                      & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                      & x.LitigationRequestArbitratorNominationLists.Count > 0
                                      | x.RequestCode != null
                                      & x.Contract.SecondPartyFacility.CitizenId == referenceId
                                      & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                      & x.LitigationRequestArbitratorNominationLists.Count > 0)
                                       .OrderByDescending(x => x.CreatedOn)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .GetPaged(pageIndex, pageSize);

            List<LitigationRequestMircoDto> litigationRequestMircoDtos = new List<LitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    bool allowAdd = true;

                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                            allowAdd = false;

                    }
                    if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                    }
                    if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                    }

                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;

                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
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
        public async Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsNominationListReply(Guid requestId, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Where(x => x.Id == requestId).ToList().FirstOrDefault();


            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

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
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                {
                    allowAdd = false;
                }
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorListNominationDone)
                        allowAdd = false;
                    else
                    {
                        if (litigationRequest.ApplicantCitizenId == referenceId)
                        {
                            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                        if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                    }
                }

            }

            LitigationRequestForAllArbitratorNominationListWithPartiesValueDto litigationRequestForAllArbitratorNominationListDto = new LitigationRequestForAllArbitratorNominationListWithPartiesValueDto();
            litigationRequestForAllArbitratorNominationListDto.LitigationRequestShortDto = request;
            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos = new List<CitizenArbitratorShortStatusWithValueDto>();

            if (litigationRequest.LitigationRequestArbitratorNominationLists.Any())
            {
                foreach (var litigationRequestArbitratorNominationList in litigationRequest.LitigationRequestArbitratorNominationLists)
                {
                    if (allowAdd)
                    {
                        CitizenArbitratorShortStatusWithValueDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusWithValueDto();
                        citizenArbitratorShortStatusDto.Id = litigationRequestArbitratorNominationList.Id;
                        citizenArbitratorShortStatusDto.CitizenId = litigationRequestArbitratorNominationList.ArbitratorCitizenId;
                        citizenArbitratorShortStatusDto.FullName = litigationRequestArbitratorNominationList.ArbitratorCitizen.FullName;

                        citizenArbitratorShortStatusDto.Brief =
                            litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                            .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                            .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                        if (litigationRequest.ApplicantCitizenId == referenceId)
                            citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;


                        if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;
                        }
                        if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                        }

                        if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;

                        }
                        if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                        }

                        litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
                    }
                    else
                    {
                        if (litigationRequest.LitigationRequestArbitratorLists.Count(x => x.ArbitratorCitizenId == litigationRequestArbitratorNominationList.ArbitratorCitizenId) > 0)
                        {
                            CitizenArbitratorShortStatusWithValueDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusWithValueDto();
                            citizenArbitratorShortStatusDto.Id = litigationRequestArbitratorNominationList.Id;
                            citizenArbitratorShortStatusDto.CitizenId = litigationRequestArbitratorNominationList.ArbitratorCitizenId;
                            citizenArbitratorShortStatusDto.FullName = litigationRequestArbitratorNominationList.ArbitratorCitizen.FullName;

                            citizenArbitratorShortStatusDto.Brief =
                                litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                                .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                                .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                            if (litigationRequest.ApplicantCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;


                            if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                            {
                                if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;
                            }
                            if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                            {
                                if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                            }

                            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                            {
                                if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;

                            }
                            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                            {
                                if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                            }

                            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
                        }
                    }
                }
                litigationRequestForAllArbitratorNominationListDto.AllowAdd = allowAdd;
                return new DataResponse<LitigationRequestForAllArbitratorNominationListWithPartiesValueDto>()
                {
                    Data = litigationRequestForAllArbitratorNominationListDto,
                    IsDone = true,
                };
            }

            return new BaseResponse()
            {
                IsDone = false,
                Message = "خطأ ما يرجى الرجوع لمدير النظام"
            };

        }
        public async Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsNominationListReply(ArbitratorNominationListRequestReplyPayload arbitratorNominationListRequestReplyPayload, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                          .Include(x => x.LitigationRequestArbitratorNominationLists)
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Where(x => x.Id == arbitratorNominationListRequestReplyPayload.RequestId).ToList().FirstOrDefault();

            bool isDone = false;
            LitigationRequestParty litigationRequestParty = LitigationRequestParty.None;
            if (litigationRequest != null)
            {
                if (litigationRequest.ApplicantCitizen != null)
                {
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                        litigationRequestParty = LitigationRequestParty.Plaintiff;
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Defendant;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyFacility != null)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility.CitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyFacility != null)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility.CitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Defendant;
                    }
                }
            }

            if (litigationRequest?.LitigationRequestArbitratorNominationLists != null)
            {

                switch (litigationRequestParty)
                {
                    case LitigationRequestParty.Plaintiff:
                        litigationRequest.IsPlaintiffArbitratorNominationListDone = true;
                        break;
                    case LitigationRequestParty.Defendant:
                        litigationRequest.IsDefendantArbitratorNominationListDone = true;
                        break;
                }

                foreach (var item in litigationRequest?.LitigationRequestArbitratorNominationLists.ToList())
                {
                    switch (litigationRequestParty)
                    {
                        case LitigationRequestParty.Plaintiff:
                            item.PlaintiffPriority = arbitratorNominationListRequestReplyPayload.ArbitratorNominationListRequestItems.Where(x => x.ArbitratorListId == item.Id).FirstOrDefault()?.Priority;
                            if (item.DefendantPriority.HasValue)
                                item.TotalPriority = item.PlaintiffPriority + item.DefendantPriority;
                            break;
                        case LitigationRequestParty.Defendant:
                            item.DefendantPriority = arbitratorNominationListRequestReplyPayload.ArbitratorNominationListRequestItems.Where(x => x.ArbitratorListId == item.Id).FirstOrDefault()?.Priority;
                            if (item.DefendantPriority.HasValue)
                                item.TotalPriority = item.DefendantPriority + item.PlaintiffPriority;
                            break;
                    }
                }
            }
            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue & litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
            {
                litigationRequest.ArbitratorsNominationListSelectionDoneDateTime = DateTime.Now;

                var arbitratorsList = litigationRequest.LitigationRequestArbitratorNominationLists.OrderBy(x => x.TotalPriority).ThenBy(x => x.DefendantPriority).ToList();
                for (int record = 0; record < litigationRequest.ArbitratorsCount - 1; record++)
                {
                    if (record == arbitratorsList.Count)
                        break;
                    LitigationRequestArbitratorAssigningRequest litigationRequestArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                    litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                    litigationRequestArbitratorAssigningRequest.RequestOn = DateTime.Now;
                    litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.Arbitrator;
                    litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                    litigationRequestArbitratorAssigningRequest.LitigationRequestArbitratorNominationListId = arbitratorsList[record].Id;
                    litigationRequestArbitratorAssigningRequest.CitizenId = arbitratorsList[record].ArbitratorCitizenId;
                    litigationRequestArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                    await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(litigationRequestArbitratorAssigningRequest);
                }
            }
            repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            return await GetCitizenLitigationRequestForArbitratorsNominationListReply(arbitratorNominationListRequestReplyPayload.RequestId, cancellationToken);

        }
        #endregion
    }
}
