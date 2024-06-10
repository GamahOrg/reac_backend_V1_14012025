using AutoMapper;
using Core.GenericRepository.IBaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.Identity.Client;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;

using REAC.EntityFrameworkCore.Entities.Litigation;
using System.Reflection;
using static Dapper.SqlMapper;
using REAC.Domain.Shared.Helpers;
using REAC.Domain.Shared.Enum;
using REAC.Application.Contracts.Dtos.Litigation;
using System.Threading;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.Application.Contracts.Dtos.Litigation.Create.Defendant;
using System.IdentityModel.Tokens.Jwt;
using REAC.Application.Contracts.IAppServices.System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Core.Localizations;
using REAC.Application.Contracts.Dtos.Integration;

namespace REAC.Application.AppServices.Litigation
{
    public partial class LitigationRequestContractService : BasicService<LitigationRequestContract, Guid>, IUserProviderService
    {
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ICitizenLitigationRequestLawyerAgentService _citizenLawyerAgent;
        private readonly ISysFileManagerService _fileService;
        public LitigationRequestContractService(ICitizenLitigationRequestLawyerAgentService citizenLawyerAgent, ISysFileManagerService fileService, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _fileService = fileService;
            _jwtReader = jwtReader;
            _mapper = mapper;
            _citizen = citizen;
            _citizenLawyerAgent = citizenLawyerAgent;
        }
        // 5 Add APIs for every Type 
        #region Add Rent Contract
        public async Task<BaseResponse> AddRentContract(RentCreateUpdateDto model)
        {
            // Add FirstPartyCitizenId 
            var logdenUser = _jwtReader.GetUserId();
            var contracts = await GetQueryableAsync();
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidateRentAsync(contracts, model, FirstPartyIdentityExpirationDate);
            if (customErrors.Count != 0)
                throw new ExceptionResponse(customErrors);
            #region Attachments

            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractKey);
            await FileHelper.Move(model.ContractAttachmentId, rootFolder);
            model.LeaseContractFileId = await _fileService.GetLatestFile();
            #endregion
            if (model.Id == Guid.Empty)
                return await base.AddAsync(model, default);
            else

                return await base.UpdateAsync(model);
        }
        #endregion

        #region RealState Contract
        public async Task<BaseResponse> AddRealEstateContract(RealEstateBrokerageCreateUpdateDto model)
        {

            var logdenUser = _jwtReader.GetUserId();
            var contracts = await GetQueryableAsync();
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidateRealStateAsync(contracts, model, FirstPartyIdentityExpirationDate);
            if (customErrors.Count != 0)
                throw new ExceptionResponse(customErrors);
            #region Attachments

            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "RealState", model.ContractKey);
            await FileHelper.Move(model.ContractAttachmentId, rootFolder);
            model.LeaseContractFileId = await _fileService.GetLatestFile();
            #endregion
            if (model.Id == Guid.Empty) 
                return await base.AddAsync(model, default);
            else

                return await base.UpdateAsync(model);
        }
        #endregion

        #region Add Defendant 
        public async Task<BaseResponse> AddDefendant(DefendantCreateUpdateDto model)
        {
            return await base.UpdateAsync(model);
        }
        #endregion

        #region Integration Simulation
        public async Task<RentIntegrationResultDto> IntegrationVerification(string logdenUserIdentityNumber = "12122nnnnnnnnn", string contractNumber = "1234abcd") // return to front
        {
            var rentIntegrateResult = RentIntegrate();
            LitigationRequestOwnerType OwnerType = 0;
            var logdenUser = _jwtReader.GetUserId();
            var agentCitizenList = _citizenLawyerAgent.GetAgentCitizens(logdenUser); // اللي ال logdin user موكل عنهم
            var lawyerCitizenList = _citizenLawyerAgent.GetLawyerCitizens(logdenUser); // اللي ال logdin user موكل عنهم
            var representativeCitizenList = _citizen.GetRepresentativeCitizens(logdenUser);
            //List<string> delegatorsIdentityList = null;
            //if (contractNumber == "1234abcd")
            //    delegatorsIdentityList = new List<string> { "123abcdooooo", "987wwwccccccc" };

            //else if (contractNumber == "222kkkdd")
            //    delegatorsIdentityList = new List<string> { "123abcdooooo" };

            if (logdenUserIdentityNumber == rentIntegrateResult.FirstPartyIdentityNo || logdenUserIdentityNumber == rentIntegrateResult.SecondPartyIdentityNo) // لو هو واحد من اللي جاي يبقي عن نفسه
            {
                OwnerType = LitigationRequestOwnerType.Personal;
            }
            else if (agentCitizenList.Result.Contains(rentIntegrateResult.SecondPartyIdentityNo))
            {
                OwnerType = LitigationRequestOwnerType.Agent;
            }
            else if (lawyerCitizenList.Result.Contains(rentIntegrateResult.SecondPartyIdentityNo))
            {
                OwnerType = LitigationRequestOwnerType.Lawyer;
            }
            else if (representativeCitizenList.Result.Contains(rentIntegrateResult.SecondPartyIdentityNo))
            {
                OwnerType = LitigationRequestOwnerType.CompanyRepresentative;
            }
            else if (rentIntegrateResult != null && rentIntegrateResult.FirstPartyIdentityNo != logdenUserIdentityNumber &&
                 rentIntegrateResult.SecondPartyIdentityNo != logdenUserIdentityNumber)
            {
                throw new ExceptionResponse(Localizations.YourIdentityNumberNotRelatedToAnyOfContractParties);
            }
            else
                throw new ExceptionResponse(Localizations.YourDelegatorsListIdentityNumbersNotContainAnyOfContractPartiesIdentityNumbers);

            return new RentIntegrationResultDto
            {

                SecondPartyIdentityNo = rentIntegrateResult.SecondPartyIdentityNo,
                FirstPartyIdentityNo = rentIntegrateResult.SecondPartyIdentityNo,
                FirstPartyBirthDate = rentIntegrateResult.FirstPartyBirthDate,
                FirstPartyEmail = rentIntegrateResult.FirstPartyEmail,
                FirstPartyMobile = rentIntegrateResult.FirstPartyMobile,
                SecondPartyBirthDate = rentIntegrateResult.SecondPartyBirthDate,
                SecondPartyEmail = rentIntegrateResult.SecondPartyEmail,
                SecondPartyMobile = rentIntegrateResult.SecondPartyMobile,
                OwnerType = OwnerType
            };
        }

        private static RentIntegrationResult RentIntegrate(string contractNumber = "")
        {
            return new RentIntegrationResult
            {
                FirstPartyIdentityNo = "123abcdooooo",
                SecondPartyIdentityNo = "987wwwccccccc",
                FirstPartyMobile = "12345678",
                FirstPartyEmail = "first@gmail.com",
                FirstPartyBirthDate = "1/1/1999",
                SecondPartyMobile = "321654987",
                SecondPartyEmail = "Second@gmail.com",
                SecondPartyBirthDate = "2/2/1888"
            };
        }
        #endregion
    }
}
