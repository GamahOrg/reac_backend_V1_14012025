using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Security.Entities;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.Dtos.UserManage;
using REAC.Application.Contracts.IAppServices.UserManage;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

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

            var isExistUser = _userManager.Users.Any(x => x.UserName == userRegisterDto.UserInfo.NationalId);
            if (isExistUser)
                return new BaseResponse() { IsDone = false, Message = "" };

            User user = new User { UserName = userRegisterDto.UserInfo.NationalId.Trim(), Email = userRegisterDto.Email.Trim(), EmailConfirmed = true };
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

                FullName = userRegisterDto.UserInfo.FullName,
                ShortName = userRegisterDto.UserInfo.FirstName,
                IdentityNumber = userRegisterDto.UserInfo.NationalId,
                IdentityExpiryDate = userRegisterDto.UserInfo.NationalIdExpiryDate,
                GenderId = (int)userRegisterDto.UserInfo.Gender,
                NationalityId = userRegisterDto.UserInfo.NationalityId,
                IdentityTypeId = (int)(userRegisterDto.UserInfo.NationalId.StartsWith("10") ? IdentityType.NationalIdentity : userRegisterDto.UserInfo.NationalId.StartsWith("20") ? IdentityType.Residence : IdentityType.GulfIdentity),
                PassAccountId = Guid.NewGuid(),
                Employer = string.Empty,
                BirthDate = userRegisterDto.UserInfo.DateOfBirth,
            };

            var citizen = _mapper.Map<Citizen>(citizenCreateUpdateDto);
            await _repository.AddAsync<Citizen, Guid>(citizen);
            var result = await _repository.SaveChangesAsync();

            user.ReferenceId = citizen.Id;
            var updateUser = await _userManager.UpdateAsync(user);
            if (!identityUser.Succeeded || identityUser.Errors.Any())
                return new BaseResponse() { IsDone = false, Message = "" };

            if (!result.IsDone) return result;

            return new BaseResponse() { IsDone = true };
        }
    }
}
