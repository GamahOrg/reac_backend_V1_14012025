using Core.GenericRepository.Shared.Pagging;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Arbitration;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Arbitration;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Application.ModuleServices
{


    public partial class ArbitrationService
    {
        #region RecusalRequest
        public async Task<BaseResponse> AddArbitrationArbitratorRecusalRequest(LitigationRequestArbitrationArbitratorRecusalRequestCreateDto requestPayload)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                                      .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                                      .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
                                      .Include(x => x.LitigationRequestArbitrationReports)
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
                                      .Where(x => x.Id == requestPayload.LitigationRequestId).SingleOrDefault();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                bool allowAdd = true;

                LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;

                if (litigationRequest.LitigationRequestArbitrationReports.Count() == 0)
                    allowAdd = false;
                else
                {
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                    if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.LitigationRequestArbitrationReportExplanationRequests != null)
                    {
                        if (litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.LitigationRequestArbitrationReportExplanationRequests.Count(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty) > 0)
                            allowAdd = false;

                    }
                }

                if (allowAdd)
                {
                    if (litigationRequestArbitrationReportExplanationRequestParty == LitigationRequestParty.None)
                    {
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "غير متاح"
                        };
                    }
                    LitigationRequestArbitratorRecusalRequest litigationRequestArbitratorRecusalRequest = new LitigationRequestArbitratorRecusalRequest()
                    {
                        Id = Guid.NewGuid(),
                        RequestOn = DateTime.Now,
                        Reason = requestPayload.Reason,
                        RequestParty = litigationRequestArbitrationReportExplanationRequestParty,
                        ArbitratorCitizenId = requestPayload.ArbitratorCitizenId,
                        LitigationRequestId = requestPayload.LitigationRequestId
                    };

                    var recusalRequest = await _repository.AddAsync<LitigationRequestArbitratorRecusalRequest, Guid>(litigationRequestArbitratorRecusalRequest);

                    var saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone)
                        return saveResponse;

                    requestPayload.LitigationRequestArbitrationArbitratorRecusalRequestFiles.ForEach(t =>
                    {
                        _repository.AddAsync<LitigationRequestArbitratorRecusalRequestFile, Guid>(new LitigationRequestArbitratorRecusalRequestFile()
                        {
                            Id = Guid.NewGuid(),
                            RecusalRequestFileId = t,
                            LitigationRequestArbitratorRecusalRequestId = litigationRequestArbitratorRecusalRequest.Id
                        });
                    });

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
            return await GetLitigationRequestForArbitratorRecusalRequest(requestPayload.LitigationRequestId);
        }
        public async Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequest(Guid requestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                     .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                         .ThenInclude(x => x.ArbitratorCitizen)
                     .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                         .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
                         .ThenInclude(x => x.RecusalRequestFile)
                         .Include(x => x.ApplicantCitizen)
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
                return new BaseResponse() { IsDone = false, Message = "طلب غير موجود" };

            bool isDone = false;
            bool allowAdd = true;

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
            if (litigationRequest.ApplicantCitizenId == referenceId)
            {
                if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            }
            if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }
            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }

            var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitratorRecusalRequests
                            .Where(x => x.RequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();

            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestArbitratorRecusalRequests.Count > 0)
                        allowAdd = false;
                    //if (litigationRequest?.LitigationRequestSubStage == LitigationRequestSubStage.JudgmentDecisionIssued)
                    //    allowAdd = false;
                    //else
                    //{
                    //    if (requestArbitrationReportExplanationRequest != null)
                    //        allowAdd = false;
                    //}
                }
            }
            var reportExplanationRequest = _mapper.Map<LitigationRequestArbitratorRecusalRequestGetDto>(requestArbitrationReportExplanationRequest);



            return new DataResponse<LitigationRequestForArbitratorRecusalRequestDto>()
            {
                Data = new LitigationRequestForArbitratorRecusalRequestDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    request = reportExplanationRequest
                },
                IsDone = true,
            };

        }
        public async Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequest(int pageIndex, int pageSize)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                                   .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                                      .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
                                     .Include(x => x.LitigationRequestArbitrationReports)
                                     .ThenInclude(x => x.LitigationRequestArbitrationReportExplanationRequests)
                                     .Include(x => x.ApplicantCitizen)
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
                                     .Include(x => x.ExternalStatus);

            litigationRequests = entitiesQuerable
                .Where(x => x.ArbitratorCitizenId == referenceId
                                     & x.RequestCode != null
                                     & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                                     & x.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation)
                                        .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.SessionDatetime)
                                        .LastOrDefault().SessionDatetime)
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

                    LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                    if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }

                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }
                    var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitrationReports.FirstOrDefault()?
                         .LitigationRequestArbitrationReportExplanationRequests
                         .Where(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();

                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                            allowAdd = false;
                        else
                        {
                            if (litigationRequest?.ArbitratorCitizenId != null)
                                allowAdd = false;

                            if (litigationRequest?.LitigationRequestArbitratorRecusalRequests.Count > 0)
                                allowAdd = false;
                        }
                    }
                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.ExternalStatus.RequestStatus;
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


        public async Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequestForArbitrator(int pageIndex, int pageSize)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequest> litigationRequests = null;
            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequest, Guid>())
                                                      .Include(x => x.Area)
                         .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                                      .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
                         .Include(x => x.ApplicantCitizen)
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
                         .Include(x => x.InternalStatus);

            litigationRequests = entitiesQuerable
                .Where(x => x.ArbitratorCitizenId == referenceId
                & x.RequestCode != null & x.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation
                & x.LitigationRequestStage == LitigationRequestStage.Arbitration
                & (x.LitigationRequestArbitratorRecusalRequests.Count() > 0))
                                        .OrderByDescending(x => x.LitigationRequestSessions.OrderBy(x => x.SessionDatetime)
                                        .LastOrDefault().SessionDatetime)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .GetPaged(pageIndex, pageSize);



            List<LitigationRequestMircoExplanationRequestDto> litigationRequestMircoDtos = new List<LitigationRequestMircoExplanationRequestDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    LitigationRequestMircoExplanationRequestDto litigationRequestMircoDto = new LitigationRequestMircoExplanationRequestDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    bool allowAdd = true;
                    bool isDone = false;

                    LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                    if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                        {
                            if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }

                    if (litigationRequest.IsFinished.HasValue)
                    {
                        if (litigationRequest.IsFinished.Value)
                        {
                            allowAdd = false;
                            isDone = true;
                        }
                    }

                    var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitrationReports.FirstOrDefault()?
                         .LitigationRequestArbitrationReportExplanationRequests
                         .Where(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();

                    if (!isDone)
                    {
                        if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                            allowAdd = false;
                        else
                        {
                            if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.JudgmentDecisionIssued)
                                allowAdd = false;
                            else
                            {
                                if (requestArbitrationReportExplanationRequest != null)
                                    allowAdd = false;
                            }
                        }
                    }
                    if (requestArbitrationReportExplanationRequest != null)
                    {
                        if (string.IsNullOrEmpty(requestArbitrationReportExplanationRequest.Response))
                            litigationRequestMircoDto.IsResponseDone = "--";
                        else
                            litigationRequestMircoDto.IsResponseDone = "تم";
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);

                }
            }
            return new DataResponse<PagedResult<LitigationRequestMircoExplanationRequestDto>>
            {
                Data =
                  new PagedResult<LitigationRequestMircoExplanationRequestDto>
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
        public async Task<BaseResponse> GetLitigationRequestForArbitratorRecusalRequestForArbitratorByRequestId(Guid requestId)
        {
            #region old code
            //Guid referenceId = Guid.Empty;
            //_jwtReader.GetReferenceId(ref referenceId);

            //var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
            //              .Include(x => x.LitigationRequestArbitratorRecusalRequests)
            //                          .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
            //             .Include(x => x.ApplicantCitizen)
            //             .Include(x => x.Contract)
            //             .Include(x => x.Contract.ContractFile)
            //             .Include(x => x.Contract.AssociationRegistrationCertificateFile)
            //             .Include(x => x.Contract.FoundingLetterFile)
            //             .Include(x => x.Contract.InternalRegulationsFile)
            //             .Include(x => x.Contract.FirstPartyFacility)
            //             .Include(x => x.Contract.FirstPartyCitizen)
            //             .Include(x => x.Contract.SecondPartyFacility)
            //             .Include(x => x.Contract.SecondPartyCitizen)
            //             .Include(x => x.Contract.LitigationRequestContractType)
            //             .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            //if (litigationRequest == null)
            //    return new BaseResponse() { IsDone = false, Message = "طلب غير موجود" };

            //bool isDone = false;
            //bool allowAdd = true;

            //var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            //LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
            //if (litigationRequest.ApplicantCitizenId == referenceId)
            //{
            //    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
            //        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            //}
            //if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
            //{
            //    if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
            //    {
            //        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
            //            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            //    }
            //}
            //if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
            //{
            //    if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
            //    {
            //        if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
            //            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
            //    }
            //}
            //if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
            //{
            //    if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
            //    {
            //        if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
            //            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            //    }
            //}
            //if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
            //{
            //    if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
            //    {
            //        if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
            //            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
            //    }
            //}

            //if (litigationRequest.IsFinished.HasValue)
            //{
            //    if (litigationRequest.IsFinished.Value)
            //    {
            //        allowAdd = false;
            //        isDone = true;
            //    }
            //}

            //var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitrationReports.FirstOrDefault()?
            //    .LitigationRequestArbitrationReportExplanationRequests
            //    .Where(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();

            //if (!isDone)
            //{
            //    if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
            //        allowAdd = false;
            //    else
            //    {
            //        if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.JudgmentDecisionIssued)
            //            allowAdd = false;
            //        else
            //        {
            //            if (requestArbitrationReportExplanationRequest != null)
            //            {
            //                if (requestArbitrationReportExplanationRequest.Response != null)
            //                    allowAdd = false;
            //            }
            //        }
            //    }
            //}
            //var reportExplanationRequest = _mapper.Map<LitigationRequestArbitrationReportExplanationRequestGetDto>(requestArbitrationReportExplanationRequest);

            //return new DataResponse<LitigationRequestForArbitrationReportExplanationRequestDto>()
            //{
            //    Data = new LitigationRequestForArbitrationReportExplanationRequestDto()
            //    {
            //        LitigationRequestShortDto = request,
            //        AllowAdd = allowAdd,
            //        Report = reportExplanationRequest
            //    },
            //    IsDone = true,
            //}; 
            #endregion

            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                     .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                         .ThenInclude(x => x.ArbitratorCitizen)
                     .Include(x => x.LitigationRequestArbitratorRecusalRequests)
                         .ThenInclude(x => x.LitigationRequestArbitratorRecusalRequestFiles)
                         .ThenInclude(x => x.RecusalRequestFile)
                         .Include(x => x.ApplicantCitizen)
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
                return new BaseResponse() { IsDone = false, Message = "طلب غير موجود" };

            bool isDone = false;
            bool allowAdd = true;

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
            if (litigationRequest.ApplicantCitizenId == referenceId)
            {
                if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            }
            if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }
            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                {
                    if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus.HasValue)
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }

            var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitratorRecusalRequests
                            .Where(x => x.RequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();



            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.JudgmentDecisionIssued)
                        allowAdd = false;
                    else
                    {
                        if (requestArbitrationReportExplanationRequest != null)
                            allowAdd = false;
                    }
                }
            }
            var reportExplanationRequest = _mapper.Map<LitigationRequestArbitratorRecusalRequestGetDto>(requestArbitrationReportExplanationRequest);



            return new DataResponse<LitigationRequestForArbitratorRecusalRequestDto>()
            {
                Data = new LitigationRequestForArbitratorRecusalRequestDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    request = reportExplanationRequest
                },
                IsDone = true,
            };
        }
        public async Task<BaseResponse> AddArbitrationReportArbitratorRecusalForArbitrator(LitigationRequestArbitrationRecusalRequestResponseDto requestPayload)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var arbitrationReportExplanationRequest = (await _repository.GetQueryable<LitigationRequestArbitratorRecusalRequest, Guid>())
                                      .Where(x => x.Id == requestPayload.Id).SingleOrDefault();

                if (arbitrationReportExplanationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                arbitrationReportExplanationRequest.ResponseOn = DateTime.Now;
                arbitrationReportExplanationRequest.ApproveRejectStatus = requestPayload.Response;

                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestForArbitratorRecusalRequestForArbitratorByRequestId(requestPayload.LitigationRequestId);
        }
        #endregion
    }
}
