using Core.Shared.Responses;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.IAppServices.Litigation
{
    public interface ILitigationRequestReformerAssigningRequest
    {
        Task<BaseResponse> GetLitigationRequestReformerAssigningRequestsAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<BaseResponse> GetLitigationRequestReformerAssigningRequests(CancellationToken cancellationToken);
        Task<BaseResponse> AddLitigationRequestReformerAssigningRequestsResponse(Guid litigationRequestReformerAssigningRequestId, LitigationRequestReformerAssigningRequestStatus reponse, CancellationToken cancellationToken);
    }

}
