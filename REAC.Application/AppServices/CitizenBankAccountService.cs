using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.IAppServices;
using REAC.EntityFrameworkCore.Entities.Actor;

namespace REAC.Application.AppServices
{
    public class CitizenBankAccountService : BasicService<CitizenBankAccount, Guid>, ICitizenBankAccountService
    {
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;


        public CitizenBankAccountService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _mapper = mapper;
            _jwtReader = jwtReader;
        }
        public async Task<BaseResponse> GetCitizenBankAccount()
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var citizenBankAccount = (await _repository.GetQueryable<CitizenBankAccount, Guid>())
                    .Include(x => x.SysBank)
                    .AsNoTracking().Where(x => x.CitizenId == referenceId)
                              .ToList();

            if (citizenBankAccount.ToList().Count == 0)
                return new DataResponse<CitizenBankAccountCreateUpdateDto>()
                {
                    IsDone = true,
                    Data = new CitizenBankAccountCreateUpdateDto()
                };

            var citizenBankAccountdto = _mapper.Map<CitizenBankAccountCreateUpdateDto>(citizenBankAccount.FirstOrDefault());
            return new DataResponse<CitizenBankAccountCreateUpdateDto>()
            {
                IsDone = true,
                Data = citizenBankAccountdto
            };
        }

        public async Task<BaseResponse> UpdateCitizenBankAccount(CitizenBankAccountCreateUpdateDto citizenBankAccountCreateUpdateDto)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            if (citizenBankAccountCreateUpdateDto.Id == Guid.Empty)
            {
                CitizenBankAccount citizenBankAccount = new CitizenBankAccount();
                citizenBankAccount.Id = Guid.NewGuid();
                citizenBankAccount.CitizenId = referenceId;
                citizenBankAccount.BankId = citizenBankAccountCreateUpdateDto.BankId;
                citizenBankAccount.IBan = citizenBankAccountCreateUpdateDto.IBan;
                citizenBankAccount.AccountOwnerName = citizenBankAccountCreateUpdateDto.AccountOwnerName;
                await _repository.AddAsync<CitizenBankAccount, Guid>(citizenBankAccount);
                var saveResponse = await _repository.SaveChangesAsync();
                if (!saveResponse.IsDone)
                    return saveResponse;

                // Send Notification
                // تم إضافة بيانات حساب البنك 

                return await GetCitizenBankAccount();
            }
            else
            {
                var response = (await _repository.GetQueryable<CitizenBankAccount, Guid>()).AsNoTracking()
                    .FirstOrDefault(c => c.CitizenId == referenceId);

                if (response == null)
                {
                    CitizenBankAccount citizenBankAccount = new CitizenBankAccount();
                    citizenBankAccount.Id = Guid.NewGuid();
                    citizenBankAccount.CitizenId = referenceId;
                    citizenBankAccount.BankId = citizenBankAccountCreateUpdateDto.BankId;
                    citizenBankAccount.IBan = citizenBankAccountCreateUpdateDto.IBan;
                    citizenBankAccount.AccountOwnerName = citizenBankAccountCreateUpdateDto.AccountOwnerName;
                    await _repository.AddAsync<CitizenBankAccount, Guid>(citizenBankAccount);
                    var saveNewResponse = await _repository.SaveChangesAsync();
                    if (!saveNewResponse.IsDone)
                        return saveNewResponse;

                    return await GetCitizenBankAccount();
                }

                var newCitizenBankAccount = _mapper.Map<CitizenBankAccount>(citizenBankAccountCreateUpdateDto);
                newCitizenBankAccount.CitizenId = referenceId;
                var saveResponse = _repository.Update<CitizenBankAccount, Guid>(newCitizenBankAccount);
                if (saveResponse.IsDone != true)
                    return new DataResponse<CitizenBankAccount> { IsDone = true, Data = response };

                var saveExistResponse = await _repository.SaveChangesAsync();
                if (!saveExistResponse.IsDone)
                    return saveExistResponse;

                // Send Notification
                // تم تعديل بيانات حساب البنك 

                return await GetCitizenBankAccount();
            }
        }
    }
}
