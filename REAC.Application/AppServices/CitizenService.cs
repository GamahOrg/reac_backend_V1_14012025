using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices;
using REAC.EntityFrameworkCore.Entities;
using REAC.EntityFrameworkCore.Entities.System;

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

        public async Task<ICollection<Facility>> GetRepresentativeCitizens(Guid? userId) // بجيب هنا كل المدعي عليهم اللي انا ممثل عنهم
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Where(x => x.LitigationCompanyRepresentativeId == userId).SelectMany(x => x.Facilities).ToList();
            return result;
        }
        public async Task<(Guid, bool)> CheckCitizenExist(string identityNumber)
        {
            var quarable = await GetQueryableAsync();
            Guid citizenId = new Guid("00000000-0000-0000-0000-000000000000");
            bool citizenExist = true;
            var exist = quarable.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
            if (exist != null)
            {
                citizenId = exist.Id;
            }
            else
                citizenExist = false;

            return (citizenId, citizenExist);
        }
    }
}
