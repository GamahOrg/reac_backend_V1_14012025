using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Delegator;

namespace REAC.Application.Contracts.IAppServices
{
    public interface IDelegatorService
    {
        Task<BaseResponse> GetDelegators();
        Task<BaseResponse> GetDelegatorById(Guid delegatorId);

        Task<BaseResponse> SaveDelegator(DelegatorDto citizenDelegatorCreateUpdateDto);
    }
}
