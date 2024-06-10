using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.IAppServices;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace REAC.Application.AppServices
{
    public class CitizenLitigationRequestLawyerAgentService : BasicService<CitizenLitigationRequestLawyerAgent, Guid>, ICitizenLitigationRequestLawyerAgentService
    {
        private readonly IMapper _mapper;
        public CitizenLitigationRequestLawyerAgentService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _mapper = mapper;
        }
        public async Task<List<string>> GetAgentCitizens(Guid? userId) // بجيب هنا كل المدعي عليهم اللي انا موكل عنهم
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Where(x => x.LitigationRequestLawyerAgentId == userId && x.LitigationRequestLawyerAgent.LawLicenseNumber == null).Select(x => x.Citizen.IdentityNumber).ToList();
            return result;
        }
        public async Task<List<string>> GetLawyerCitizens(Guid? userId) // بجيب هنا كل المدعي عليهم اللي انا محامي عنهم
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Where(x => x.LitigationRequestLawyerAgentId == userId && x.LitigationRequestLawyerAgent.LawLicenseNumber != null).Select(x => x.Citizen.IdentityNumber).ToList();
            return result;
        }
    }
}
