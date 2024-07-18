using Core.Shared.Responses;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.Contracts.IAppServices
{
    public interface IDelegatorService
    {
        Task<BaseResponse> SaveFacilityDelegator(FacilityDelegatorCreateUpdateDto facilityDelegatorCreateUpdateDto);
        Task<BaseResponse> SaveCitizenDelegator(CitizenDelegatorCreateUpdateDto citizenDelegatorCreateUpdateDto);
    }
}
