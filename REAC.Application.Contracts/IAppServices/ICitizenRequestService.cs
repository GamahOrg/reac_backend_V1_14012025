using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.CitizenRequests;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.IAppServices
{
    public interface ICitizenRequestService
    {
        Task<BaseResponse> GetCitizenRequest();
        Task<BaseResponse> GetCitizenRequestById(Guid citizenRequestId);
        Task<BaseResponse> SaveCitizenRequest(CitizenRequestCreateUpdateDto citizenRequestCreateUpdateDto);
        Task<BaseResponse> ExecuteAction(CitizenRequestAction citizenRequestAction, Guid requestId, string? notes);

    }
}
