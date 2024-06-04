using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.IAppServices;
using REAC.EntityFrameworkCore.Entities.System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace REAC.Application.AppServices
{
    public class CitizenService : BasicService<Citizen, Guid>, ICitizenService
    {
        private readonly IMapper _mapper;
        public CitizenService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _mapper = mapper;
        }
        public async Task<DateTime?> GetIdentityExpirationDate(Guid? userId)
        {
            var quarable = await GetQueryableAsync();
            var identityExpirationDate = quarable.FirstOrDefault(x => x.Id == userId)?.IdentityExpiryDate;
            return identityExpirationDate;
        }

        public async Task<List<string>> GetRepresentativeCitizens(Guid? userId) // بجيب هنا كل المدعي عليهم اللي انا ممثل عنهم
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Where(x => x.LitigationCompanyRepresentativeId == userId).Select(x => x.IdentityNumber).ToList();
            return result;
        }
    }
}
