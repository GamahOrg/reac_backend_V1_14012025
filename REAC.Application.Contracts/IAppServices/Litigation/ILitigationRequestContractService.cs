using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Integration;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.Dtos.Litigation.Create.Defendant;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Application.Contracts.IAppServices.Litigation
{
    public interface IUserProviderService : IBasicService<LitigationRequestContract, Guid>
    {
        Task<BaseResponse> AddRentContract(RentCreateUpdateDto model);
        Task<BaseResponse> AddDefendant(DefendantCreateUpdateDto model);
        Task<RentIntegrationResultDto> IntegrationVerification(string logdenUserIdentityNumber = "12122nnnnnnnnn", string contractNumber = "1234abcd");
        Task<BaseResponse> AddRealEstateContract(RealEstateBrokerageCreateUpdateDto model);
    }
}
