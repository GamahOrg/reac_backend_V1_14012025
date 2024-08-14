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
        public async Task<BaseResponse> AddLitigationRequestArbitratorsChairmanNominationList(ArbitratorNominationListRequestPayload arbitratorNominationListRequestPayload)
        {
            try
            {
                if (arbitratorNominationListRequestPayload.Arbitrators.Count == 0)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لابد من إضافة مرشحى رئيس هيئة المحكمين"
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
                        if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNomination)
                            allowAdd = false;
                    }
                }
                if (!litigationRequest.LitigationRequestArbitratorNominationLists.Any())
                    allowAdd = false;

                if (allowAdd)
                {

                    arbitratorNominationListRequestPayload.Arbitrators.ForEach(async t =>
                    {
                        LitigationRequestArbitratorsChairmanNominationList litigationRequestArbitratorNominationList = new LitigationRequestArbitratorsChairmanNominationList();
                        litigationRequestArbitratorNominationList.ArbitratorChairmanCitizenId = t;
                        litigationRequestArbitratorNominationList.LitigationRequestId = arbitratorNominationListRequestPayload.RequestId;
                        litigationRequestArbitratorNominationList.Id = Guid.NewGuid();
                        await _repository.AddAsync<LitigationRequestArbitratorsChairmanNominationList, Guid>(litigationRequestArbitratorNominationList);
                    });
                    litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNominationDone;
                    litigationRequest.ArbitratorsChairmanNominationListCreationDateTime = DateTime.Now;

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;
                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة عدد رؤساء هيئة المحكمين"
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
            return await GetLitigationRequestForAddArbitratorsChairmanNominationList(arbitratorNominationListRequestPayload.RequestId);
        }
        public async Task<BaseResponse> GetLitigationRequestForAddArbitratorsChairmanNominationList(Guid requestId)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                           .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.LitigationRequestArbitratorsChairmanNominationListValues)

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
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNomination)
                        allowAdd = false;
                }
            }
            LitigationRequestForAllArbitratorCharmanNominationListDto litigationRequestForAllArbitratorNominationListDto = new LitigationRequestForAllArbitratorCharmanNominationListDto();
            litigationRequestForAllArbitratorNominationListDto.LitigationRequestShortDto = request;
            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorCharmanShortStatusDtos = new List<CitizenArbitratorCharmanShortStatusDto>();

            if (litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Any())
            {
                allowAdd = false;
                foreach (var litigationRequestArbitratorNominationList in litigationRequest.LitigationRequestArbitratorsChairmanNominationLists)
                {
                    CitizenArbitratorCharmanShortStatusDto citizenArbitratorShortStatusChairmanDto = new CitizenArbitratorCharmanShortStatusDto();
                    citizenArbitratorShortStatusChairmanDto.FullName = litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen.FullName;
                    citizenArbitratorShortStatusChairmanDto.Mobile = litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen.Mobile;
                    citizenArbitratorShortStatusChairmanDto.Email = litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen.Email;

                    citizenArbitratorShortStatusChairmanDto.ArbitratorClass =
                        litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.SysArbitratorClass?.ArbitratorClassName;

                    citizenArbitratorShortStatusChairmanDto.Brief =
                        litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                    citizenArbitratorShortStatusChairmanDto.ExperienceYears =
                        litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.ExperienceYears;

                    citizenArbitratorShortStatusChairmanDto.TotalPriority = litigationRequestArbitratorNominationList.LitigationRequestArbitratorsChairmanNominationListValues.Sum(x => x.Priority);

                    citizenArbitratorShortStatusChairmanDto.RequestsCount = litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen?.ArbitratorLitigationRequests.Count() + litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen.LitigationRequestArbitratorLists.Count;
                    citizenArbitratorShortStatusChairmanDto.IdentityNumber = litigationRequestArbitratorNominationList.ArbitratorChairmanCitizen.IdentityNumber;

                    var assingingRequest = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(t => t.LitigationRequestId == litigationRequest.Id
                                                           & t.CitizenId == litigationRequestArbitratorNominationList.ArbitratorChairmanCitizenId & t.ArbitratorAssigningRequestTypeId == ArbitratorAssigningRequestType.ArbitratorsChairman);

                    if (assingingRequest != null)
                    {
                        if (assingingRequest.Any())
                        {
                            if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Approve)
                                citizenArbitratorShortStatusChairmanDto.Status = "تم الموافقة";
                            else
                            {
                                if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Reject)
                                    citizenArbitratorShortStatusChairmanDto.Status = "تم الرفض";
                                else
                                {
                                    if (assingingRequest.LastOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Cancelled)
                                        citizenArbitratorShortStatusChairmanDto.Status = "تم الإلغاء";
                                    else
                                    {
                                        citizenArbitratorShortStatusChairmanDto.Status = "قيد الرد";
                                    }
                                }
                            }
                        }
                    }
                    litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorCharmanShortStatusDtos.Add(citizenArbitratorShortStatusChairmanDto);
                }
                return new DataResponse<LitigationRequestForAllArbitratorCharmanNominationListDto>()
                {
                    Data = litigationRequestForAllArbitratorNominationListDto,
                    IsDone = true
                };
            }

            return new DataResponse<LitigationRequestForAllArbitratorNominationListDto>()
            {
                Data = new LitigationRequestForAllArbitratorNominationListDto()
                {
                    ListCount = (await _repository.GetQueryable<SysArbitratorNominationListCount, Guid>()).Where(x => x.ArbitratorCount == litigationRequest.ArbitratorsCount).FirstOrDefault()?.ArbitratorChairmanNominationListCount,
                    AllowAdd = allowAdd,
                    LitigationRequestShortDto = request
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForAddArbitratorsChairmanNominationList(int pageIndex, int pageSize)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                            .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.LitigationRequestArbitratorsChairmanNominationListValues)
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

                            if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNomination)
                                allowAdd = false;
                        }
                    }
                    if (litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Count > 0)
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
        public async Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsChairmanNominationListReply(int pageIndex, int pageSize)
        {

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);


            PagedResult<LitigationRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
               .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.LitigationRequestArbitratorsChairmanNominationListValues)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType);


            litigationRequests = entitiesQuerable.Where(
                                      x => x.RequestCode != null
                                      & x.LitigationRequestArbitratorLists.Where(t => t.ArbitratorCitizenId == referenceId).Count() > 0
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

                    LitigationRequestMircoDto litigationRequestMircoDto = new LitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.Status = litigationRequest.ExternalStatus.RequestStatus;

                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;

                    if (litigationRequest.LitigationRequestArbitratorsChairmanNominationLists
                        .Where(x => x.LitigationRequestArbitratorsChairmanNominationListValues.Count(x => x.ArbitratorCitizenId == referenceId) > 0).Count() > 0)
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
        public async Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsChairmanNominationListReply(Guid requestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                             .Include(x => x.Area).Include(x => x.City)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.ArbitratorCitizen)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.LitigationRequestArbitratorsChairmanNominationListValues)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                            .ThenInclude(x => x.ArbitratorChairmanCitizen)
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
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNominationDone)
                        allowAdd = false;

                }

            }
            if (litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Where(x => x.LitigationRequestArbitratorsChairmanNominationListValues
                                                     .Where(x => x.ArbitratorCitizenId == referenceId).Count() > 0).Count() > 0)
                allowAdd = false;

            LitigationRequestForAllArbitratorNominationListWithPartiesValueDto litigationRequestForAllArbitratorNominationListDto = new LitigationRequestForAllArbitratorNominationListWithPartiesValueDto();
            litigationRequestForAllArbitratorNominationListDto.LitigationRequestShortDto = request;
            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos = new List<CitizenArbitratorShortStatusWithValueDto>();
            if (litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Any())
            {
                foreach (var chairmanNominationList in litigationRequest.LitigationRequestArbitratorsChairmanNominationLists)
                {
                    CitizenArbitratorShortStatusWithValueDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusWithValueDto();
                    citizenArbitratorShortStatusDto.Id = chairmanNominationList.Id;
                    citizenArbitratorShortStatusDto.CitizenId = chairmanNominationList.ArbitratorChairmanCitizenId;
                    citizenArbitratorShortStatusDto.FullName = chairmanNominationList.ArbitratorChairmanCitizen.FullName;
                    citizenArbitratorShortStatusDto.Brief =
                        chairmanNominationList.ArbitratorChairmanCitizen?.CitizenRequests?
                        .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                        .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                    citizenArbitratorShortStatusDto.Value = chairmanNominationList.LitigationRequestArbitratorsChairmanNominationListValues.Where(x => x.ArbitratorCitizenId == referenceId).FirstOrDefault()?.Priority;
                    litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
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
        public async Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsChairmanNominationListReply(ArbitratorNominationListRequestReplyPayload arbitratorNominationListRequestReplyPayload)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                          .Include(x => x.LitigationRequestArbitratorNominationLists)
                          .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                                                    .Include(x => x.LitigationRequestArbitratorLists)

                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Where(x => x.Id == arbitratorNominationListRequestReplyPayload.RequestId).ToList().FirstOrDefault();

            bool isDone = false;

            arbitratorNominationListRequestReplyPayload?.ArbitratorNominationListRequestItems.ToList().ForEach(item =>
            {
                var valueItem = litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Where(x => x.Id == item.ArbitratorListId).FirstOrDefault();
                if (valueItem != null)
                {
                    _repository.AddAsync<LitigationRequestArbitratorsChairmanNominationListValue, Guid>(new LitigationRequestArbitratorsChairmanNominationListValue()
                    {
                        Id = Guid.NewGuid(),
                        ArbitratorChairmanCitizenId = valueItem.ArbitratorChairmanCitizenId,
                        LitigationRequestArbitratorsChairmanNominationListId = valueItem.Id,
                        IsSent = false,
                        ArbitratorCitizenId = referenceId,
                        Priority = item.Priority
                    });
                }
            });
            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            int total = litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.Count * litigationRequest.LitigationRequestArbitratorLists.Count;

            litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                          .Include(x => x.LitigationRequestArbitratorNominationLists)
                          .Include(x => x.LitigationRequestArbitratorsChairmanNominationLists)
                                                    .ThenInclude(x => x.LitigationRequestArbitratorsChairmanNominationListValues)
                          .Include(x => x.ApplicantCitizen)
                          .Where(x => x.Id == arbitratorNominationListRequestReplyPayload.RequestId).ToList().FirstOrDefault();

            if (litigationRequest?.LitigationRequestArbitratorsChairmanNominationLists != null)
            {
                litigationRequest?.LitigationRequestArbitratorsChairmanNominationLists.ToList().ForEach(chairmanNominationList =>
                {
                    total -= chairmanNominationList.LitigationRequestArbitratorsChairmanNominationListValues.Count;
                });

                litigationRequest?.LitigationRequestArbitratorsChairmanNominationLists.ToList().ForEach(chairmanNominationList =>
                {
                    chairmanNominationList.TotalPriority = chairmanNominationList.LitigationRequestArbitratorsChairmanNominationListValues.Sum(x => x.Priority);
                });



                if (total == 0)
                {
                    litigationRequest.ArbitratorsChairmanNominationListSelectionDoneDateTime = DateTime.Now;

                    LitigationRequestArbitratorAssigningRequest litigationRequestArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                    litigationRequestArbitratorAssigningRequest.RequestOn = DateTime.Now;
                    litigationRequestArbitratorAssigningRequest.Id = Guid.NewGuid();
                    litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.ArbitratorsChairman;
                    litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                    //litigationRequestArbitratorAssigningRequest.LitigationRequestArbitratorNominationListId =
                    //    litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.OrderBy(x => x.TotalPriority).ToList()[0].Id;
                    litigationRequestArbitratorAssigningRequest.CitizenId =
                        litigationRequest.LitigationRequestArbitratorsChairmanNominationLists.OrderBy(x => x.TotalPriority).ToList()[0].ArbitratorChairmanCitizenId;
                    litigationRequestArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                    await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(litigationRequestArbitratorAssigningRequest);
                    repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;
                }

            }
            return await GetCitizenLitigationRequestForArbitratorsChairmanNominationListReply(arbitratorNominationListRequestReplyPayload.RequestId);

        }
        #endregion
    }
}
