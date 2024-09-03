using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;

namespace REAC.Application.ModuleServices
{
    public partial class ReconciliationService
    {
        #region Report
        public async Task<BaseResponse> AddReconciliationReport(WritingMinutesOfTheMeetingRequestPayload requestPayload, CancellationToken cancellationToken)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestReconciliationReports)
                        .Include(x => x.ArbitratorCitizen)
                        .Include(x => x.ReformerCitizen)
                        .Include(x => x.ApplicantCitizen)
                        .Include(x => x.Contract)
                        .Include(x => x.Contract.FirstPartyFacility).ThenInclude(x => x.Citizen)
                        .Include(x => x.Contract.FirstPartyCitizen)
                        .Include(x => x.Contract.SecondPartyFacility).ThenInclude(x => x.Citizen)
                        .Include(x => x.Contract.SecondPartyCitizen)
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

                    await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        RequestId = litigationRequest.Id,
                        ShowForApplicant = false,
                        LogMessage = "تم إضافة محصر الصلح"
                    });


                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;
                    string msg = (await _sysSettingService.GetBySettingKey(SysSettingKeyConstants.Reconciliation_Report_Creation)).Data?.ToString();

                    // Send Notification
                    // تم إضافة محضر الصلح
                    try
                    {
                        msg = _sysSettingService.GetBySettingKey(SysSettingKeyConstants.Reconciliation_Request_Added).Result.Data?.Value;
                        msg = msg.Replace("{LitigationCode}", litigationRequest.RequestCode);
                        SendNotificationMSWithAllTypes(msg, litigationRequest, "Reconciliation Request Added");
                    }
                    catch (Exception e)
                    {


                    }
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

            try
            {
                int LitigationRequestReconciliationReportDecisionPeriod = int.Parse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReconciliationReportDecisionPeriod).Result.Data?.Value);
                double totalHours = 0;
                totalHours += (DateTime.Now - litigationRequest.LitigationRequestReconciliationReports.FirstOrDefault().ReportDate).TotalHours;
                if (totalHours >= LitigationRequestReconciliationReportDecisionPeriod)
                {
                    allowDecision = true;
                }
            }
            catch { }

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

                                      .Where(x => x.ReformerCitizenId == referenceId
                                      & x.LitigationRequestSessions.Where(t => t.LitigationRequestSessionRequestTypeId == LitigationRequestSessionRequestType.Reconciliation).Count() > 0
                                      & x.RequestCode != null
                                      & x.InternalStatusId != RequestStatus.Rejected
                                      & x.LitigationRequestCategoryId != LitigationRequestCategory.Arbitration).
                                      OrderByDescending(x => x.CreatedOn).ToList();

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
                    try
                    {
                        int LitigationRequestReconciliationReportDecisionPeriod = int.Parse(_sysSettingService.GetBySettingKey(SysSettingKeyConstants.SLA_LitigationRequestReconciliationReportDecisionPeriod).Result.Data?.Value);
                        double totalHours = 0;
                        totalHours += (DateTime.Now - litigationRequest.LitigationRequestReconciliationReports.FirstOrDefault().ReportDate).TotalHours;
                        if (totalHours >= LitigationRequestReconciliationReportDecisionPeriod)
                        {
                            allowAdd = true;
                        }
                    }
                    catch { }

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

    }
}
