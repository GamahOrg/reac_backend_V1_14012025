using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Application.Contracts.IAppServices.Litigation
{
    public interface ILitigationRequestService : IBasicService<LitigationRequest, Guid>
    {
        Task<BaseResponse> Add(LitigationCreateUpdateDto model);
    }
}
