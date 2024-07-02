using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.System.LitigationRequestContractTypeDtos;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.System
{
    public class SysLitigationRequestContractTypeService : BasicService<SysLitigationRequestContractType, LitigationRequestContractType>, ISysLitigationRequestContractTypeService
    {
        private readonly IUserProviderService _contractservice;
        public SysLitigationRequestContractTypeService(IUserProviderService contractservice, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)
        {
            _contractservice = contractservice;
        }
        public async Task<BaseResponse> GetListAsync(CancellationToken cancellationToken)
        {
            return await base.GetListAsync<SysLitigationRequestContractTypeGetDto>(cancellationToken);
        }
        public async Task<BaseResponse> GetTermsConditionsAsync(string contractNumber, CancellationToken cancellationToken)
        {
            var contract = await _contractservice.GetContract(contractNumber);
            var quarble = await GetQueryableAsync();
            var result = quarble.SingleOrDefault(x => x.Id == contract.LitigationRequestContractTypeId);

            result.AcknowledgmentContent = $"{result.AcknowledgmentContent.Replace("{اسم المدعي}", contract.FirstPartyCitizen.FullName).Replace("{رقم هوية المدعي}", contract.FirstPartyCitizen.IdentityNumber).Replace("{مصدر الهوية}", contract.FirstPartyCitizen.District).Replace("{تاريخ الميلادي لليوم}", DateTime.Now.ToString())}";
            return new DataResponse<SysLitigationRequestContractTypeDto> { Data = new SysLitigationRequestContractTypeDto
            {
                AcknowledgmentContent= result.AcknowledgmentContent,
                TypeName = result.CodingPrefixSymbol,
                TermsAndConditions = result.TermsAndConditions
            }, IsDone = true };
        }
    }
}
