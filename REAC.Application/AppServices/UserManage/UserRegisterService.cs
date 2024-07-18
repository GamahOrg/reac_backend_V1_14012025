using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Security.Entities;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using REAC.Application.Contracts.Dtos.UserManage;
using REAC.Application.Contracts.IAppServices.UserManage;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.AppServices.UserManage
{
    public class UserRegisterService : IUserRegisterService
    {
        private readonly IEntityRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserRegisterService(IEntityRepository repository, UserManager<User> userManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Register(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null)
                return new BaseResponse() { IsDone = false, Message = "" };

            var isExistUser = _userManager.Users.Any(x => x.UserName == userRegisterDto.UserInfo.id);
            if(isExistUser)
                return new BaseResponse() { IsDone = false, Message = "" };

            User user = new User { UserName = userRegisterDto.UserInfo.id.Trim(), Email = userRegisterDto.Email.Trim(), EmailConfirmed = true };
            var identityUser = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (!identityUser.Succeeded || identityUser.Errors.Any())
                return new BaseResponse() { IsDone = false, Message = "" };

            CitizenCreateUpdateDto citizenCreateUpdateDto = new CitizenCreateUpdateDto()
            {
                AreaId = userRegisterDto.AreaId,
                CityId = userRegisterDto.CityId,
                BuildingNumber = userRegisterDto.BuildingNumber,
                District = userRegisterDto.District,
                Email = userRegisterDto.Email,
                Mobile = userRegisterDto.MobileNumber,
                Street = userRegisterDto.Street,
                UnitNumber = userRegisterDto.UnitNumber,
                UserId = user.Id,

                FullName = userRegisterDto.UserInfo.full_name_ar,
                ShortName = userRegisterDto.UserInfo.two_names_ar,
                IdentityNumber = userRegisterDto.UserInfo.id,
                IdentityExpiryDate = DateTime.Parse(userRegisterDto.UserInfo.id_expiry_date_g),
                GenderId = (int)(userRegisterDto.UserInfo.gender == "M" ? Gender.Male : Gender.Female),
                NationalityId = (await _repository.GetQueryable<SysNationality, Guid>()).FirstOrDefault(x => x.NationalityCode == userRegisterDto.UserInfo.nationality).Id,
                IdentityTypeId = (int)(userRegisterDto.UserInfo.id.StartsWith("10") ? IdentityType.NationalIdentity : userRegisterDto.UserInfo.id.StartsWith("20") ? IdentityType.Residence : IdentityType.GulfIdentity),
                PassAccountId = Guid.NewGuid(),
                Employer = string.Empty,
                BirthDate = DateTime.Parse(userRegisterDto.UserInfo.dob_g),
            };

            var citizen = _mapper.Map<Citizen>(citizenCreateUpdateDto);
            await _repository.AddAsync<Citizen, Guid>(citizen);
            var result = await _repository.SaveChangesAsync();
            if(!result.IsDone) return result;

            return new BaseResponse() { IsDone = true };
        }
    }
}
