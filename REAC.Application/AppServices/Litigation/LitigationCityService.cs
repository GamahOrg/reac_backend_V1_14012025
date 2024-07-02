using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.System.LitigationRequestAmountDtos;
using REAC.Application.Contracts.Dtos.Litigation;

namespace REAC.Application.AppServices.Litigation
{
    public partial class LitigationCityService : BasicService<LitigationCity, Guid>, ILitigationCityService
    {
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ICitizenLitigationRequestLawyerAgentService _citizenLawyerAgent;
        private readonly ISysFileManagerService _fileService;
        public LitigationCityService(ICitizenLitigationRequestLawyerAgentService citizenLawyerAgent, ISysFileManagerService fileService, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _fileService = fileService;
            _jwtReader = jwtReader;
            _mapper = mapper;
            _citizen = citizen;
            _citizenLawyerAgent = citizenLawyerAgent;
        }
    }
}
