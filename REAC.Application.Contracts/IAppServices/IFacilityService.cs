using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Facility;
using REAC.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.IAppServices
{
    public interface IFacilityService
    {
        Task<BaseResponse> GetAllFacilitiesAsync(int pageIndex, int pageSize, string? searchKey);
        Task<BaseResponse> GetFacilityByIdAsync(Guid facilityId);
        Task<BaseResponse> EditFacilityAsync(EditFacilityRequestDto input);
        Task<BaseResponse> ValidateAndAddFacility(string crNumber);
    }
}
