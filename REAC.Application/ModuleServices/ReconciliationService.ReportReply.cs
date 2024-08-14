using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Application.ModuleServices
{
    public partial class ReconciliationService
    {
        #region Report Reply
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
                                      .Where(x => x.RequestCode != null & x.ApplicantCitizenId == referenceId & x.LitigationRequestReconciliationReports.Count > 0
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

                         .Where(x => x.Id == requestId & x.LitigationRequestReconciliationReports.Count > 0).ToList().FirstOrDefault();

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
            if (litigationRequest.LitigationRequestReconciliationReports.Count == 0)
            {
                allowAdd = false;
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
        public async Task<BaseResponse> AddCitizenLitigationRequestForReportReply(Guid requestId, ApproveRejectStatus requestMinutesOfMeetingApprovalStatus, CancellationToken cancellationToken)
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
                if (litigationRequest.MintuesOfMeetingApprovalDefendantStatus == ApproveRejectStatus.Approve & litigationRequest.MintuesOfMeetingApprovalPlaintiffStatus == ApproveRejectStatus.Approve)
                {
                    litigationRequest.InternalStatusId = RequestStatus.ReconciliationWasMade;
                    litigationRequest.ExternalStatusId = RequestStatus.ReconciliationWasMade;
                }
                else
                {
                    litigationRequest.InternalStatusId = RequestStatus.ReconciliationWasNotMade;
                    litigationRequest.ExternalStatusId = RequestStatus.ReconciliationWasNotMade;
                    if (litigationRequest.LitigationRequestCategoryId != LitigationRequestCategory.Reconciliation)
                    {
                        litigationRequest.LitigationRequestStage = LitigationRequestStage.Arbitration;
                        litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.ArbitratorSelectionDuration;

                    }
                }
            }

            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            return await GetCitizenLitigationRequestForReportReply(requestId, cancellationToken);

        }
        #endregion

    }
}
