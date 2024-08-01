using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.System.AreaDto;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.System;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.System
{
    public partial class SysArbitratorClassService : BasicService<SysArbitratorClass, Guid>, ISysArbitratorClassService
    {
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ISysFileManagerService _fileService;
        public SysArbitratorClassService(ISysFileManagerService fileService, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _fileService = fileService;
            _jwtReader = jwtReader;
            _mapper = mapper;
            _citizen = citizen;
        }


        public async Task<BaseResponse> GetArbitratorClasses(int experienceYears)
        {
            var sysArbitratorClasses = (await _repository.GetQueryable<SysArbitratorClass, Guid>()).Where(x => experienceYears >= x.YearsFrom && experienceYears <= x.YearsTo).ToList();
            if (sysArbitratorClasses.Any())
                return new DataResponse<List<SysArbitratorClassDto>> { IsDone = true, Data = _mapper.Map<List<SysArbitratorClassDto>>(sysArbitratorClasses) };

            return new DataResponse<List<SysArbitratorClassDto>> { IsDone = true, Data = new List<SysArbitratorClassDto>() };
        }

        public async Task<BaseResponse> GetArbitratorClass(int experienceYears)
        {
            var sysArbitratorClasses = (await _repository.GetQueryable<SysArbitratorClass, Guid>()).FirstOrDefault(x => experienceYears >= x.YearsFrom && experienceYears <= x.YearsTo);
            if (sysArbitratorClasses != null)
                return new DataResponse<SysArbitratorClassDto> { IsDone = true, Data = _mapper.Map<SysArbitratorClassDto>(sysArbitratorClasses) };

            return new BaseResponse();
        }

    }
}
