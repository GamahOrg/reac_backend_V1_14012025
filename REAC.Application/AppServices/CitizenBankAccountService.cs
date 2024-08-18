using AutoMapper;
using Core.Contracts;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices
{
    public class CitizenBankAccountService : BasicService<CitizenBankAccount, Guid>, ICitizenBankAccountService
    {
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ISysSettingService _sysSetting;
        private readonly IMailService _emailService;
        private readonly ISMSService _smsService;


        public CitizenBankAccountService(IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, IMailService emailService, ISMSService smsService) : base(repository, mapper, loggerManager, jwtReader)
        {
            _mapper = mapper;
            _jwtReader = jwtReader;
            _emailService = emailService;
            _smsService = smsService;
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
            var citizenInfo = (await _repository.GetQueryable<Citizen, Guid>())
                    .Where(c => c.Id == referenceId)
                    .Select(c => new { Email = c.Email, Mobile = c.Mobile })
                    .FirstOrDefault();
            var msg = "";
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
                msg = _sysSetting.GetBySettingKey(SysSettingKeyConstants.Citizen_AddBankAccount).Result.Data?.Value;
                var emailResult = await _emailService.SendMail(new List<string>() { citizenInfo.Email }, "Bank Account Added", msg);
                var smsResult = _smsService.SendSMS(new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                {
                    message_text = msg,
                    recipients = new List<string>() { citizenInfo.Mobile },
                    notification_type = "SMS",

                });
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
                

                msg = _sysSetting.GetBySettingKey(SysSettingKeyConstants.Citizen_EditBankAccount).Result.Data?.Value;
                var emailResult = await _emailService.SendMail(new List<string>() { citizenInfo.Email }, "Bank Account Updated", msg);
                var smsResult = _smsService.SendSMS(new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                {
                    message_text = msg,
                    recipients = new List<string>() { citizenInfo.Mobile },
                    notification_type = "SMS",

                });
                return await GetCitizenBankAccount();
            }
        }
    }
}
