using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Reconciliation;



namespace REAC.Application.ModuleServices
{
    public interface IReconciliationService
    {
        Task<BaseResponse> AddLitigationRequestDecision(LitigationRequestAction requestPayload, CancellationToken cancellationToken);

        Task<BaseResponse> GetLitigationRequestForSession(CancellationToken cancellationToken);

        Task<BaseResponse> AddReconciliationSession(ScheduleSessionProcessRequestPayload requestPayload, CancellationToken cancellationToken);

        Task<BaseResponse> GetLitigationRequestForSession(Guid requestId, CancellationToken cancellationToken);

        Task<BaseResponse> AddReconciliationReport(WritingMinutesOfTheMeetingRequestPayload requestPayload, CancellationToken cancellationToken);

        Task<BaseResponse> GetLitigationRequestForReport(Guid requestId, CancellationToken cancellationToken);

        Task<BaseResponse> GetLitigationRequestForReport(CancellationToken cancellationToken);

        #region Citizen Report Reply
        Task<BaseResponse> GetCitizenLitigationRequestForReportReply(CancellationToken cancellationToken);
        Task<BaseResponse> GetCitizenLitigationRequestForReportReply(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> AddCitizenLitigationRequestForReportReply(Guid requestId, MinutesOfMeetingApprovalStatus requestMinutesOfMeetingApprovalStatus, CancellationToken cancellationToken);
        #endregion


        #region Extend Period Request
        Task<BaseResponse> AddReconciliationExtendPeriod(LitigationRequestExtendPeriodRequestCreateUpdateDto requestPayload, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriod(Guid requestId, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestExtendPeriod(CancellationToken cancellationToken);
        #endregion


    }
}