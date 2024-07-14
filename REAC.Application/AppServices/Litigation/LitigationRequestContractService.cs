using AutoMapper;
using Core.GenericRepository.BaseRepository;
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
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Contract;
using System.Diagnostics.Contracts;
using System.Linq;
using REAC.Application.Contracts.Dtos.Integration.Rent;
using REAC.Application.Contracts.Dtos.Integration.Wafi;
using REAC.Application.Contracts.Dtos.Integration.Properties;
using REAC.Application.Contracts.Dtos.Integration.RealState;
using REAC.Application.Contracts.Dtos.Integration.ReRealStatent;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using REAC.Application.Contracts.Dtos.Integration.Party;

namespace REAC.Application.AppServices.Litigation
{
    public partial class LitigationRequestContractService : BasicService<LitigationRequestContract, Guid>, IUserProviderService
    {
        private REACDBContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ICitizenLitigationRequestLawyerAgentService _citizenLawyerAgent;
        private readonly ISysFileManagerService _fileService;
        public LitigationRequestContractService(REACDBContext context, ICitizenLitigationRequestLawyerAgentService citizenLawyerAgent, ISysFileManagerService fileService, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _context = context;
            _fileService = fileService;
            _jwtReader = jwtReader;
            _mapper = mapper;
            _citizen = citizen;
            _citizenLawyerAgent = citizenLawyerAgent;
        }


        //Get Contract by contractKey
        public async Task<LitigationRequestContract> GetContract(string contractKey)
        {
            var quarable = await GetQueryableAsync();
            var result = quarable.Include(x => x.FirstPartyCitizen)
                .SingleOrDefault(x => x.ContractKey == contractKey);
            return result;
        }

        // 5 Add APIs for every Type 
        #region Add Rent Contract
        public async Task<BaseResponse> AddRentContract(RentCreateUpdateDto model)
        {
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            var contracts = await GetQueryableAsync();
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidateRentAsync(contracts, model, FirstPartyIdentityExpirationDate);
            //if (customErrors.Count != 0)
            //    throw new ExceptionResponse(customErrors);
            #region Attachments

            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractKey);
            await FileHelper.Move(model.ContractAttachmentId, rootFolder);

            if (model.AgancyAttachmentId != null)
            {
                string agancyRootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractKey, "AgancyAttachment");
                await FileHelper.CopyFiles(model.AgancyAttachmentId.Value, agancyRootFolder);
            }
            if (model.EstablishmentContractId != null)
            {
                string establishRootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractKey, "EstablishmentContract");
                await FileHelper.CopyFiles(model.EstablishmentContractId.Value, establishRootFolder);
            }
            if (model.LawLicenseAttachmentId != null)
            {
                string establishRootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractKey, "LawLicenseAttachment");
                await FileHelper.CopyFiles(model.LawLicenseAttachmentId.Value, establishRootFolder);
            }
            #endregion
            // model.LeaseContractFileId = await _fileService.GetLatestFile();
            var record = await GetQueryableAsync();
            var contractRow = record.Where(x => x.Id == model.Id).SingleOrDefault();
            contractRow.LeaseContractFileId = await _fileService.GetLatestFile();
            var contractUpdateDto = new LitigationRequestContractCreateUpdateDto
            {
                Id = contractRow.Id,
                LitigationRequestContractTypeId = (int)contractRow.LitigationRequestContractTypeId,
                ContractKey = contractRow.ContractKey,
                VersionNumber = contractRow.VersionNumber,
                LeaseContractFileId = contractRow.LeaseContractFileId,
                //FirstPartyAccountId = contractRow.FirstPartyAccountId,
                FirstPartyFacilityId = contractRow.FirstPartyFacilityId ?? null,
                FirstPartyCapacity = contractRow.FirstPartyCapacity,
                //SecondPartyAccountId = contractRow.SecondPartyAccountId,
                SecondPartyFacilityId = contractRow.SecondPartyFacilityId,
                SecondPartyCapacity = contractRow.SecondPartyCapacity,
                SecondPartyName = contractRow.SecondPartyName,
                // SecondPartyPhoneNumber = contractRow.SecondPartyPhoneNumber,
                // RegistrationRequestId = contractRow.RegistrationRequestId,
                // DelegationOwnerName = contractRow.DelegationOwnerName,
                //IdentityType = contractRow.IdentityType,
                //IdentityNumber = contractRow.IdentityNumber,
                //IdentityExpirationDate = contractRow.IdentityExpirationDate,
                //IdentityPhoto = contractRow.IdentityPhoto

            };
            return await base.UpdateAsync(contractUpdateDto);
        }
        #endregion

        #region RealState Contract
        public async Task<BaseResponse> AddRealEstateContract(RealEstateBrokerageCreateUpdateDto model)
        {

            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            var contracts = await GetQueryableAsync();
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidateRealStateAsync(contracts, model, FirstPartyIdentityExpirationDate);
            //if (customErrors.Count != 0)
            //    throw new ExceptionResponse(customErrors);
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

        #region Add Other Contract 
        public async Task<BaseResponse> AddOtherContract(OtherCreateUpdateDto model)
        {
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            var firstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var errors = ValidateOtherAsync(firstPartyIdentityExpirationDate);
            //if (errors.Count != 0)
            //    throw new ExceptionResponse(errors);

            #region Attachments

            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Other", model.ContractKey);
            await FileHelper.Move(model.ContractAttachmentId, rootFolder);
            await FileHelper.Move(model.AgancyAttachmentId, rootFolder);
            await FileHelper.Move(model.LawLicenseAttachmentId, rootFolder);
            await FileHelper.Move(model.EstablishmentContractId, rootFolder);
            model.LeaseContractFileId = await _fileService.GetLatestFile();
            #endregion
            if (model.Id == Guid.Empty)
                return await base.AddAsync(model, default);
            else

                return await base.UpdateAsync(model);
        }
        #endregion

        #region Add Wafi Contract
        public async Task<BaseResponse> AddWafiContract(WafiCreateUpdateDto model)
        {
            // Add FirstPartyCitizenId 
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            var contracts = await GetQueryableAsync();
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidateWafiAsync(contracts, model, FirstPartyIdentityExpirationDate);
            //if (customErrors.Count != 0)
            //    throw new ExceptionResponse(customErrors);
            #region Attachments

            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Wafi", model.ContractKey);
            await FileHelper.Move(model.WafiContractAttachmentId, rootFolder);
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

        #region Rent Integration Simulation
        public async Task<BaseResponse> RentIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string contractNumber, string versionNumber)
        {
            var rentIntegrateDto = new RentIntegrateDto
            {
                ContractKey = "1234abcd",
                VersionNumber = "7777",
                FirstPartyDto = new PartyDTO
                {
                    FullName = "FirstPartyName",
                    IdentityNumber = "123abcdooooo",
                    NationalAddress = "testAddress",
                    Nationality = "Egyption",
                    FacilityNumber = null,
                    CommercialRegistrationNo = null,
                    FacilityName = null,
                    Capacity = "مستاجر",
                    Mobile = "12345678",
                    Email = "first@gmail.com",
                    BirthDate = new DateTime(1999, 1, 1)
                },
                SecondPartyDto = new PartyDTO
                {
                    Nationality = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Egyption",
                    FullName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "SecondPArtyName",
                    IdentityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "987wwwccccccc",
                    NationalAddress = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "testAddress",
                    FacilityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "123987" : null,
                    CommercialRegistrationNo = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "9999999" : null,
                    FacilityName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "FacilityName" : null,
                    Capacity = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "مالك",
                    Mobile = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "321654987",
                    Email = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Second@gmail.com",
                    BirthDate = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? DateTime.MinValue : new DateTime(1999, 2, 2)

                },
                FirstPartyCapacity = FirstPartyCapacity,
                ContractAddress = "contractAddressTest"
            };
           // var contractExist = CheckContractExist(contractNumber, versionNumber);
            //if (contractExist.Result.Item1 == false)
            //{
                #region Insert Contract and Parties
                var contract = new LitigationRequestContract
                {

                    ContractKey = rentIntegrateDto.ContractKey,
                    FirstPartyCapacity = rentIntegrateDto.FirstPartyCapacity.ToString(),
                    LitigationRequestContractTypeId = LitigationRequestContractType.Rent,
                    SecondPartyCapacity = rentIntegrateDto.SecondPartyDto.Capacity,
                    SecondPartyEmail = rentIntegrateDto.SecondPartyDto.Email,
                    LeaseContractFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                    SecondPartyIdentityNumber = rentIntegrateDto.SecondPartyDto.IdentityNumber,
                    SecondPartyMobile = rentIntegrateDto.SecondPartyDto.FullName,
                    VersionNumber = rentIntegrateDto.VersionNumber,
                    SecondPartyName = rentIntegrateDto.SecondPartyDto.FullName,

                };
                var checkFirstParty = _citizen.CheckCitizenExist(rentIntegrateDto.FirstPartyDto.IdentityNumber);
                var checkSecondParty = _citizen.CheckCitizenExist(rentIntegrateDto.SecondPartyDto.IdentityNumber);
                if (checkFirstParty.Result.Item2 == true)
                {
                    contract.FirstPartyCitizenId = checkFirstParty.Result.Item1;
                }
                else
                {
                    contract.FirstPartyCitizen = new Citizen
                    {

                        Nationality = rentIntegrateDto.FirstPartyDto.Nationality,
                        NationalAddress = "NationalAddress",
                        Mobile = "01255855852",
                        BirthDate = rentIntegrateDto.FirstPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = rentIntegrateDto.FirstPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = rentIntegrateDto.FirstPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                    };
                }
                if (checkSecondParty.Result.Item2 == true)
                {
                    contract.SecondPartyCitizenId = checkSecondParty.Result.Item1;
                }
                else
                {
                    contract.SecondPartyCitizen = new Citizen
                    {

                        NationalAddress = "NationalAddress",
                        Mobile = "01025588558",
                        BirthDate = rentIntegrateDto.SecondPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = rentIntegrateDto.SecondPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = rentIntegrateDto.SecondPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                        Nationality = rentIntegrateDto.SecondPartyDto.Nationality,
                    };
                }
                await base.AddAsync(contract, default);
                rentIntegrateDto.FirstPartyDto.CitizenId = contract.FirstPartyCitizen == null ? checkFirstParty.Result.Item1 : contract.FirstPartyCitizen.Id;
                rentIntegrateDto.SecondPartyDto.CitizenId = contract.SecondPartyCitizen == null ? checkSecondParty.Result.Item1 : contract.SecondPartyCitizen.Id;
                rentIntegrateDto.Id = contract.Id;
                #endregion
           // }
            //else
            //{
            //    rentIntegrateDto.Id = contractExist.Result.Item4;
            //    rentIntegrateDto.FirstPartyDto.FirstPartyCitizenId = contractExist.Result.Item2;
            //    rentIntegrateDto.SecondPartyDto.SecondPartyCitizenId = contractExist.Result.Item3;
            //}
            return await RentIntegrationValidation(rentIntegrateDto, FirstPartyCapacity, contractNumber, versionNumber);
        }

        public async Task<(bool, Guid, Guid, Guid)> CheckContractExist(string contractKey, string versionNumber)
        {
            var quarable = await GetQueryableAsync();
            bool contractExist = false;
            Guid firstPartyCitizenId = new Guid();
            Guid secondPartyCitizenId = new Guid();
            Guid Id = new Guid();
            var exist = quarable.Where(x => x.ContractKey == contractKey && x.VersionNumber == versionNumber).FirstOrDefault();
            if (exist != null)
            {
                contractExist = true;
                firstPartyCitizenId = exist.FirstPartyCitizenId;
                secondPartyCitizenId = exist.SecondPartyCitizenId.Value;
                Id = exist.Id;

            }
            return (contractExist, firstPartyCitizenId, secondPartyCitizenId, Id);
        }

        public async Task<BaseResponse> RentIntegrationValidation(RentIntegrateDto rentIntegrateDto, LitigationRequestOwnerType ownerType, string contractNumber, string versionNumber)
        {
            if (rentIntegrateDto.ContractKey == contractNumber && rentIntegrateDto.VersionNumber == versionNumber && ownerType == LitigationRequestOwnerType.Lawyer)
            {
                var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
                string logdenUserIdentityNumber = "12122nnnnnnnnn";

                switch (ownerType)
                {
                    case LitigationRequestOwnerType.Personal:
                        if (logdenUserIdentityNumber == rentIntegrateDto.FirstPartyDto.IdentityNumber)
                            rentIntegrateDto.FirstPartyDto.IsPretender = true;
                        else if (logdenUserIdentityNumber == rentIntegrateDto.SecondPartyDto.IdentityNumber)
                            rentIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.CompanyRepresentative:
                        var representativeCitizenList = await _citizen.GetRepresentativeCitizens(logdenUser);
                        if (representativeCitizenList.Any(x => x.Facility700Number == rentIntegrateDto.FirstPartyDto.FacilityNumber && x.CommercialRegisterNumber == rentIntegrateDto.FirstPartyDto.CommercialRegistrationNo))
                            rentIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (representativeCitizenList.Any(x => x.Facility700Number == rentIntegrateDto.SecondPartyDto.FacilityNumber && x.CommercialRegisterNumber == rentIntegrateDto.SecondPartyDto.CommercialRegistrationNo))
                            rentIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.Lawyer:
                        var lawyerCitizenList = await _citizenLawyerAgent.GetLawyerCitizens(logdenUser);
                        if (lawyerCitizenList.Contains(rentIntegrateDto.FirstPartyDto.IdentityNumber))
                            rentIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (lawyerCitizenList.Contains(rentIntegrateDto.SecondPartyDto.IdentityNumber))
                            rentIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.Agent:
                        var agentCitizenList = await _citizenLawyerAgent.GetAgentCitizens(logdenUser);
                        if (agentCitizenList.Contains(rentIntegrateDto.FirstPartyDto.IdentityNumber))
                            rentIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (agentCitizenList.Contains(rentIntegrateDto.SecondPartyDto.IdentityNumber))
                            rentIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    default:
                        throw new Exception("");
                        //throw new ExceptionResponse(Localizations.NoPermissionToViewLitigation);
                }
                if (rentIntegrateDto.FirstPartyDto.IsPretender == true)
                {
                    rentIntegrateDto.FirstPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                    rentIntegrateDto.FirstPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                    rentIntegrateDto.FirstPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                    rentIntegrateDto.FirstPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    rentIntegrateDto.FirstPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    rentIntegrateDto.FirstPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                }
                if (rentIntegrateDto.SecondPartyDto.IsPretender == true)
                {
                    rentIntegrateDto.SecondPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                    rentIntegrateDto.SecondPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                    rentIntegrateDto.SecondPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                    rentIntegrateDto.SecondPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    rentIntegrateDto.SecondPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    rentIntegrateDto.SecondPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                }


                return new DataResponse<RentIntegrateDto> { IsDone = true, Message = Localizations.RetrievedSuccefully, Data = rentIntegrateDto };
            }
            else
                return new DataResponse<RentIntegrateDto> { IsDone = false, Message = Localizations.ContractNotFound, Data = new RentIntegrateDto() };


        }
        #endregion

        #region Real Integration Simulation
        public async Task<BaseResponse> RealStateIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string contractNumber = "")
        {
            var realStateIntegrateDto = new RealStateIntegrateDto
            {
                ContractKey = "1234abcd",
                FirstPartyDto = new PartyDTO
                {
                    FullName = "FirstPartyName",
                    IdentityNumber = "123abcdooooo",
                    NationalAddress = "testAddress",
                    Nationality = "Egyption",
                    FacilityNumber = null,
                    CommercialRegistrationNo = null,
                    FacilityName = null,
                    Capacity = "مستاجر",
                    Mobile = "12345678",
                    Email = "first@gmail.com",
                    BirthDate = new DateTime(1999, 1, 1)
                },
                SecondPartyDto = new PartyDTO
                {
                    Nationality = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Egyption",
                    FullName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "SecondPArtyName",
                    IdentityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "987wwwccccccc",
                    NationalAddress = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "testAddress",
                    FacilityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "123987" : null,
                    CommercialRegistrationNo = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "9999999" : null,
                    FacilityName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "FacilityName" : null,
                    Capacity = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "مالك",
                    Mobile = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "321654987",
                    Email = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Second@gmail.com",
                    BirthDate = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? DateTime.MinValue : new DateTime(1999, 2, 2)

                },
                FirstPartyCapacity = FirstPartyCapacity,
                ContractAddress = "ContractAddressTest"
            };
         //   var contractExist = CheckRealStateContractExist(contractNumber);
            //if (contractExist.Result.Item1 == false)
            //{
                #region Insert Contract and Parties
                var contract = new LitigationRequestContract
                {

                    ContractKey = realStateIntegrateDto.ContractKey,
                    FirstPartyCapacity = realStateIntegrateDto.FirstPartyCapacity.ToString(),
                    LitigationRequestContractTypeId = LitigationRequestContractType.RealEstateBrokerage,
                    SecondPartyCapacity = realStateIntegrateDto.SecondPartyDto.Capacity,
                    SecondPartyEmail = realStateIntegrateDto.SecondPartyDto.Email,
                    LeaseContractFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                    SecondPartyIdentityNumber = realStateIntegrateDto.SecondPartyDto.IdentityNumber,
                    SecondPartyMobile = realStateIntegrateDto.SecondPartyDto.FullName,
                    SecondPartyName = realStateIntegrateDto.SecondPartyDto.FullName,

                };
                var checkFirstParty = _citizen.CheckCitizenExist(realStateIntegrateDto.FirstPartyDto.IdentityNumber);
                var checkSecondParty = _citizen.CheckCitizenExist(realStateIntegrateDto.SecondPartyDto.IdentityNumber);
                if (checkFirstParty.Result.Item2 == true)
                {
                    contract.FirstPartyCitizenId = checkFirstParty.Result.Item1;
                }
                else
                {
                    contract.FirstPartyCitizen = new Citizen
                    {

                        Nationality = realStateIntegrateDto.FirstPartyDto.Nationality,
                        NationalAddress = "NationalAddress",
                        Mobile = "01255855852",
                        BirthDate = realStateIntegrateDto.FirstPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = realStateIntegrateDto.FirstPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = realStateIntegrateDto.FirstPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                    };
                }
                if (checkSecondParty.Result.Item2 == true)
                {
                    contract.SecondPartyCitizenId = checkSecondParty.Result.Item1;
                }
                else
                {
                    contract.SecondPartyCitizen = new Citizen
                    {

                        NationalAddress = "NationalAddress",
                        Mobile = "01025588558",
                        BirthDate = realStateIntegrateDto.SecondPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = realStateIntegrateDto.SecondPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = realStateIntegrateDto.SecondPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                        Nationality = realStateIntegrateDto.SecondPartyDto.Nationality,
                    };
                }
                await base.AddAsync(contract, default);
                realStateIntegrateDto.FirstPartyDto.CitizenId = contract.FirstPartyCitizen == null ? checkFirstParty.Result.Item1 : contract.FirstPartyCitizen.Id;
                realStateIntegrateDto.SecondPartyDto.CitizenId = contract.SecondPartyCitizen == null ? checkSecondParty.Result.Item1 : contract.SecondPartyCitizen.Id;
                realStateIntegrateDto.Id = contract.Id;
                #endregion
        //    }
            //else
            //{
            //    realStateIntegrateDto.Id = contractExist.Result.Item4;
            //    realStateIntegrateDto.FirstPartyDto.FirstPartyCitizenId = contractExist.Result.Item2;
            //    realStateIntegrateDto.SecondPartyDto.SecondPartyCitizenId = contractExist.Result.Item3;
            //}
            return await RealStatentegrationValidation(realStateIntegrateDto, FirstPartyCapacity, contractNumber);
        }

        public async Task<(bool, Guid, Guid, Guid)> CheckRealStateContractExist(string contractKey)
        {
            var quarable = await GetQueryableAsync();
            bool contractExist = false;
            Guid firstPartyCitizenId = new Guid();
            Guid secondPartyCitizenId = new Guid();
            Guid Id = new Guid();
            var exist = quarable.Where(x => x.ContractKey == contractKey).FirstOrDefault();
            if (exist != null)
            {
                contractExist = true;
                firstPartyCitizenId = exist.FirstPartyCitizenId;
                secondPartyCitizenId = exist.SecondPartyCitizenId.Value;
                Id = exist.Id;

            }
            return (contractExist, firstPartyCitizenId, secondPartyCitizenId, Id);
        }

    
        public async Task<BaseResponse> RealStatentegrationValidation(RealStateIntegrateDto realStateIntegrateDto, LitigationRequestOwnerType ownerType, string contractNumber)
        {
        if (realStateIntegrateDto.ContractKey == contractNumber && ownerType == LitigationRequestOwnerType.Lawyer)
        {
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            string logdenUserIdentityNumber = "12122nnnnnnnnn";

            switch (ownerType)
            {
                case LitigationRequestOwnerType.Personal:
                    if (logdenUserIdentityNumber == realStateIntegrateDto.FirstPartyDto.IdentityNumber)
                        realStateIntegrateDto.FirstPartyDto.IsPretender = true;
                    else if (logdenUserIdentityNumber == realStateIntegrateDto.SecondPartyDto.IdentityNumber)
                        realStateIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.CompanyRepresentative:
                    var representativeCitizenList = await _citizen.GetRepresentativeCitizens(logdenUser);
                    if (representativeCitizenList.Any(x => x.Facility700Number == realStateIntegrateDto.FirstPartyDto.FacilityNumber && x.CommercialRegisterNumber == realStateIntegrateDto.FirstPartyDto.CommercialRegistrationNo))
                        realStateIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (representativeCitizenList.Any(x => x.Facility700Number == realStateIntegrateDto.SecondPartyDto.FacilityNumber && x.CommercialRegisterNumber == realStateIntegrateDto.SecondPartyDto.CommercialRegistrationNo))
                        realStateIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.Lawyer:
                    var lawyerCitizenList = await _citizenLawyerAgent.GetLawyerCitizens(logdenUser);
                    if (lawyerCitizenList.Contains(realStateIntegrateDto.FirstPartyDto.IdentityNumber))
                        realStateIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (lawyerCitizenList.Contains(realStateIntegrateDto.SecondPartyDto.IdentityNumber))
                        realStateIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.Agent:
                    var agentCitizenList = await _citizenLawyerAgent.GetAgentCitizens(logdenUser);
                    if (agentCitizenList.Contains(realStateIntegrateDto.FirstPartyDto.IdentityNumber))
                        realStateIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (agentCitizenList.Contains(realStateIntegrateDto.SecondPartyDto.IdentityNumber))
                        realStateIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                default:
                    throw new Exception("");
                    //throw new ExceptionResponse(Localizations.NoPermissionToViewLitigation);
            }
            if (realStateIntegrateDto.FirstPartyDto.IsPretender == true)
            {
                realStateIntegrateDto.FirstPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                realStateIntegrateDto.FirstPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                realStateIntegrateDto.FirstPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                realStateIntegrateDto.FirstPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                realStateIntegrateDto.FirstPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                realStateIntegrateDto.FirstPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
            }
            if (realStateIntegrateDto.SecondPartyDto.IsPretender == true)
            {
                realStateIntegrateDto.SecondPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                realStateIntegrateDto.SecondPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                realStateIntegrateDto.SecondPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                realStateIntegrateDto.SecondPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                realStateIntegrateDto.SecondPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                realStateIntegrateDto.SecondPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
            }
            return new DataResponse<RealStateIntegrateDto> { IsDone = true, Message = Localizations.RetrievedSuccefully, Data = realStateIntegrateDto };
        }
        else
            return new DataResponse<RealStateIntegrateDto> { IsDone = false, Message = Localizations.ContractNotFound, Data = new RealStateIntegrateDto() };
    }
        #endregion

        #region Wafi Integration Simulation
        public async Task<BaseResponse> WafiIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string WafiLicenseNo,WafiRequestOwnerType firstPartType,WafiRequestOwnerType secondPartyType )
        {
            var wafiIntegrateDto = new WafiIntegrateDto
            {
                FirstPartyDto = new PartyDTO
                {
                    Nationality = "Egyption",
                    IdentityNumber = "987wwwccccccc",
                    FullName = "SecondPartyName",
                    NationalAddress = "testAddress",
                    FacilityNumber = null,
                    CommercialRegistrationNo = null,
                    FacilityName = null,
                    Capacity = "مطور عقاري",
                    Mobile = "321654987",
                    Email = "Second@gmail.com",
                    BirthDate = DateTime.Parse("5-5-1980")
                },
                SecondPartyDto = new PartyDTO
                {
                    Nationality = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Egyption",
                    IdentityNumber = "987www",
                    FullName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "SecondPartyName",
                    NationalAddress = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "testAddress",
                    FacilityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "123987" : null,
                    CommercialRegistrationNo = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "9999999" : null,
                    FacilityName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "FacilityName" : null,
                    Capacity = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "محاسب قانوني",
                    Mobile = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "321654987",
                    Email = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Second@gmail.com",
                    BirthDate =DateTime.Parse("2/2/1888")
                },
                //ThirdPartyDto = new PartyDTO
                //{

                //    Nationality = "Egyption",
                //    IdentityNumber = "987wwwccccccc",
                //    FullName = "SecondPartyName",
                //    NationalAddress = "testAddress",
                //    FacilityNumber = null,
                //    CommercialRegistrationNo = null,
                //    FacilityName = null,
                //    Capacity = "استشاري هتدسي",
                //    Mobile = "321654987",
                //    Email = "Second@gmail.com",
                //    BirthDate = DateTime.Parse("5-5-1980")

                //},
                FirstPartyCapacity = FirstPartyCapacity,
                ContractAddress = "contractAddressTest"
            };
            return await WafiIntegrationValidation(wafiIntegrateDto, FirstPartyCapacity);
        }
        public async Task<BaseResponse> WafiIntegrationValidation(WafiIntegrateDto wafiIntegrateDto, LitigationRequestOwnerType ownerType)
        {
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            string logdenUserIdentityNumber = "12122nnnnnnnnn";

            switch (ownerType)
            {
                case LitigationRequestOwnerType.Personal:
                    if (logdenUserIdentityNumber == wafiIntegrateDto.FirstPartyDto.IdentityNumber)
                        wafiIntegrateDto.FirstPartyDto.IsPretender = true;
                    else if (logdenUserIdentityNumber == wafiIntegrateDto.SecondPartyDto.IdentityNumber)
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;
                    else if (logdenUserIdentityNumber == wafiIntegrateDto.ThirdPartyDto.IdentityNumber)
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.CompanyRepresentative:
                    var representativeCitizenList = await _citizen.GetRepresentativeCitizens(logdenUser);
                    if (representativeCitizenList.Any(x => x.Facility700Number == wafiIntegrateDto.FirstPartyDto.FacilityNumber && x.CommercialRegisterNumber == wafiIntegrateDto.FirstPartyDto.CommercialRegistrationNo))
                        wafiIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (representativeCitizenList.Any(x => x.Facility700Number == wafiIntegrateDto.SecondPartyDto.FacilityNumber && x.CommercialRegisterNumber == wafiIntegrateDto.SecondPartyDto.CommercialRegistrationNo))
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.Lawyer:
                    var lawyerCitizenList = await _citizenLawyerAgent.GetLawyerCitizens(logdenUser);
                    if (lawyerCitizenList.Contains(wafiIntegrateDto.FirstPartyDto.IdentityNumber))
                        wafiIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (lawyerCitizenList.Contains(wafiIntegrateDto.SecondPartyDto.IdentityNumber))
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;

                    if (lawyerCitizenList.Contains(wafiIntegrateDto.ThirdPartyDto.IdentityNumber))
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                case LitigationRequestOwnerType.Agent:
                    var agentCitizenList = await _citizenLawyerAgent.GetAgentCitizens(logdenUser);
                    if (agentCitizenList.Contains(wafiIntegrateDto.FirstPartyDto.IdentityNumber))
                        wafiIntegrateDto.FirstPartyDto.IsPretender = true;

                    if (agentCitizenList.Contains(wafiIntegrateDto.SecondPartyDto.IdentityNumber))
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;

                    if (agentCitizenList.Contains(wafiIntegrateDto.ThirdPartyDto.IdentityNumber))
                        wafiIntegrateDto.SecondPartyDto.IsPretender = true;
                    break;
                default:
                    throw new Exception("");
                    //throw new ExceptionResponse(Localizations.NoPermissionToViewLitigation);
            }

            if (wafiIntegrateDto.FirstPartyDto.IsPretender == true)
            {
                wafiIntegrateDto.FirstPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                wafiIntegrateDto.FirstPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                wafiIntegrateDto.FirstPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                wafiIntegrateDto.FirstPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                wafiIntegrateDto.FirstPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                wafiIntegrateDto.FirstPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
            }
            if (wafiIntegrateDto.SecondPartyDto.IsPretender == true)
            {
                wafiIntegrateDto.SecondPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                wafiIntegrateDto.SecondPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                wafiIntegrateDto.SecondPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                wafiIntegrateDto.SecondPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                wafiIntegrateDto.SecondPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                wafiIntegrateDto.SecondPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
            }

            return new DataResponse<WafiIntegrateDto> { IsDone = true, Message = Localizations.RetrievedSuccefully, Data = wafiIntegrateDto };
        }
        //public async Task<BaseResponse> WafiCapacties(WafiIntegrateDto wafiIntegrateDto, WafiRequestOwnerType firstPartType, WafiRequestOwnerType secondPartyType)
        //{

        //    return new DataResponse<WafiIntegrateDto> { IsDone = true, Message = Localizations.RetrievedSuccefully, Data = wafiIntegrateDto };

        //}
        #endregion

        public async Task<BaseResponse> AddPropertiesContract(PropertiesCreateUpdateDto model)
        {
            var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
            var FirstPartyIdentityExpirationDate = await _citizen.GetIdentityExpirationDate(logdenUser);
            var customErrors = ValidatePropertiesAsync(FirstPartyIdentityExpirationDate);
            //if (customErrors.Count != 0)
            //    throw new ExceptionResponse(customErrors);
            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Properties", model.ContractKey);
            await FileHelper.Move(model.AssociationInternalRegulationsId, rootFolder);
            await FileHelper.Move(model.IncorporationAssociationLetterId, rootFolder);
            await FileHelper.Move(model.AssociationRegistrationCertificateId, rootFolder);
            model.LeaseContractFileId = await _fileService.GetLatestFile();

            if (model.Id == Guid.Empty)
                return await base.AddAsync(model, default);
            else

                return await base.UpdateAsync(model);
        }
        public async Task<BaseResponse> PropertiesIntegrate(LitigationRequestOwnerType FirstPartyCapacity, string associationCode , string associationName)
        {
            var PropertiesIntegrateDto = new PropertiesIntegrateDto
            {
                AssociationCode = "1234abcd",
                AssociationName = "7777",
                FirstPartyDto = new PartyDTO
                {
                    FullName = "FirstPartyName",
                    IdentityNumber = "123abcdooooo",
                    NationalAddress = "testAddress",
                    Nationality = "Egyption",
                    FacilityNumber = null,
                    CommercialRegistrationNo = null,
                    FacilityName = null,
                    Capacity = "مستاجر",
                    Mobile = "12345678",
                    Email = "first@gmail.com",
                    BirthDate = new DateTime(1999, 1, 1)

                },
                SecondPartyDto = new PartyDTO
                {
                    FullName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "SecondPArtyName",
                    IdentityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "987wwwccccccc",
                    NationalAddress = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "testAddress",
                    FacilityNumber = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "123987" : null,
                    CommercialRegistrationNo = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "9999999" : null,
                    Nationality = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Egyption",
                    FacilityName = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? "FacilityName" : null,
                    Capacity = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "مالك",
                    Mobile = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "321654987",
                    Email = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? null : "Second@gmail.com",
                    BirthDate = FirstPartyCapacity == LitigationRequestOwnerType.CompanyRepresentative ? DateTime.MinValue : new DateTime(1999, 2, 2)
                },
                FirstPartyCapacity = FirstPartyCapacity,
                ContractAddress = "ContractAddressTest"
            };
            var contractExist = CheckPropertiesContractExist(associationCode, associationName);
            if (contractExist.Result.Item1 == false)
            {
                #region Insert Contract and Parties
                var contract = new LitigationRequestContract
                {

                    ContractKey = PropertiesIntegrateDto.AssociationCode,
                    FirstPartyCapacity = PropertiesIntegrateDto.FirstPartyCapacity.ToString(),
                    LitigationRequestContractTypeId = LitigationRequestContractType.Properties,
                    SecondPartyCapacity = PropertiesIntegrateDto.SecondPartyDto.Capacity,
                    SecondPartyEmail = PropertiesIntegrateDto.SecondPartyDto.Email,
                    LeaseContractFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                    SecondPartyIdentityNumber = PropertiesIntegrateDto.SecondPartyDto.IdentityNumber,
                    SecondPartyMobile = PropertiesIntegrateDto.SecondPartyDto.FullName,
                    VersionNumber = PropertiesIntegrateDto.AssociationName,
                    SecondPartyName = PropertiesIntegrateDto.SecondPartyDto.FullName,

                };
                var checkFirstParty = _citizen.CheckCitizenExist(PropertiesIntegrateDto.FirstPartyDto.IdentityNumber);
                var checkSecondParty = _citizen.CheckCitizenExist(PropertiesIntegrateDto.SecondPartyDto.IdentityNumber);
                if (checkFirstParty.Result.Item2 == true)
                {
                    contract.FirstPartyCitizenId = checkFirstParty.Result.Item1;
                }
                else
                {
                    contract.FirstPartyCitizen = new Citizen
                    {

                        Nationality = PropertiesIntegrateDto.FirstPartyDto.Nationality,
                        NationalAddress = "NationalAddress",
                        Mobile = "01255855852",
                        BirthDate = PropertiesIntegrateDto.FirstPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = PropertiesIntegrateDto.FirstPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = PropertiesIntegrateDto.FirstPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                    };
                }
                if (checkSecondParty.Result.Item2 == true)
                {
                    contract.SecondPartyCitizenId = checkSecondParty.Result.Item1;
                }
                else
                {
                    contract.SecondPartyCitizen = new Citizen
                    {

                        NationalAddress = "NationalAddress",
                        Mobile = "01025588558",
                        BirthDate = PropertiesIntegrateDto.SecondPartyDto.BirthDate,
                        BuildingNumber = "1234567891",
                        City = "City",
                        District = "District",
                        Email = "firstparty@gmail.com",
                        Employer = "Employer",
                        FullName = PropertiesIntegrateDto.SecondPartyDto.FullName,
                        GenderId = Gender.Male,
                        IdentityExpiryDate = DateTime.Now.AddDays(5),
                        IdentityNumber = PropertiesIntegrateDto.SecondPartyDto.IdentityNumber,
                        IdentityFileId = Guid.Parse("61a90c64-69c5-4c85-a95f-ad31333abec7"),
                        IdentityTypeId = IdentityType.NationalIdentity,
                        Street = "Street",
                        UnitNumber = "UnitNumber",
                        ShortName = "ShortName",
                        Nationality = PropertiesIntegrateDto.SecondPartyDto.Nationality,
                    };
                }
                await base.AddAsync(contract, default);
                PropertiesIntegrateDto.FirstPartyDto.CitizenId = contract.FirstPartyCitizen == null ? checkFirstParty.Result.Item1 : contract.FirstPartyCitizen.Id;
                PropertiesIntegrateDto.SecondPartyDto.CitizenId = contract.SecondPartyCitizen == null ? checkSecondParty.Result.Item1 : contract.SecondPartyCitizen.Id;
                PropertiesIntegrateDto.Id = contract.Id;
                #endregion
            }
            else
            {
                PropertiesIntegrateDto.Id = contractExist.Result.Item4;
                PropertiesIntegrateDto.FirstPartyDto.CitizenId = contractExist.Result.Item2;
                PropertiesIntegrateDto.SecondPartyDto.CitizenId = contractExist.Result.Item3;
            }
            return await PropertiesIntegrationValidation(PropertiesIntegrateDto, FirstPartyCapacity, associationCode, associationName);
        }

        public async Task<(bool, Guid, Guid, Guid)> CheckPropertiesContractExist(string associationCode, string associationName)
        {
            var quarable = await GetQueryableAsync();
            bool contractExist = false;
            Guid firstPartyCitizenId = new Guid();
            Guid secondPartyCitizenId = new Guid();
            Guid Id = new Guid();
            var exist = quarable.Where(x => x.ContractKey == associationCode && x.VersionNumber == associationName).FirstOrDefault();
            if (exist != null)
            {
                contractExist = true;
                firstPartyCitizenId = exist.FirstPartyCitizenId;
                secondPartyCitizenId = exist.SecondPartyCitizenId.Value;
                Id = exist.Id;

            }
            return (contractExist, firstPartyCitizenId, secondPartyCitizenId, Id);

        }

        public async Task<BaseResponse> PropertiesIntegrationValidation(PropertiesIntegrateDto PropertiesIntegrateDto, LitigationRequestOwnerType ownerType, string associationCode, string associationName)
        {
            if (PropertiesIntegrateDto.AssociationCode == associationCode && PropertiesIntegrateDto.AssociationName == associationName && ownerType == LitigationRequestOwnerType.Lawyer)
            {
                var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/
                string logdenUserIdentityNumber = "12122nnnnnnnnn";

                switch (ownerType)
                {
                    case LitigationRequestOwnerType.Personal:
                        if (logdenUserIdentityNumber == PropertiesIntegrateDto.FirstPartyDto.IdentityNumber)
                            PropertiesIntegrateDto.FirstPartyDto.IsPretender = true;
                        else if (logdenUserIdentityNumber == PropertiesIntegrateDto.SecondPartyDto.IdentityNumber)
                            PropertiesIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.CompanyRepresentative:
                        var representativeCitizenList = await _citizen.GetRepresentativeCitizens(logdenUser);
                        if (representativeCitizenList.Any(x => x.Facility700Number == PropertiesIntegrateDto.FirstPartyDto.FacilityNumber && x.CommercialRegisterNumber == PropertiesIntegrateDto.FirstPartyDto.CommercialRegistrationNo))
                            PropertiesIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (representativeCitizenList.Any(x => x.Facility700Number == PropertiesIntegrateDto.SecondPartyDto.FacilityNumber && x.CommercialRegisterNumber == PropertiesIntegrateDto.SecondPartyDto.CommercialRegistrationNo))
                            PropertiesIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.Lawyer:
                        var lawyerCitizenList = await _citizenLawyerAgent.GetLawyerCitizens(logdenUser);
                        if (lawyerCitizenList.Contains(PropertiesIntegrateDto.FirstPartyDto.IdentityNumber))
                            PropertiesIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (lawyerCitizenList.Contains(PropertiesIntegrateDto.SecondPartyDto.IdentityNumber))
                            PropertiesIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    case LitigationRequestOwnerType.Agent:
                        var agentCitizenList = await _citizenLawyerAgent.GetAgentCitizens(logdenUser);
                        if (agentCitizenList.Contains(PropertiesIntegrateDto.FirstPartyDto.IdentityNumber))
                            PropertiesIntegrateDto.FirstPartyDto.IsPretender = true;

                        if (agentCitizenList.Contains(PropertiesIntegrateDto.SecondPartyDto.IdentityNumber))
                            PropertiesIntegrateDto.SecondPartyDto.IsPretender = true;
                        break;
                    default:
                        throw new Exception("");
                        //throw new ExceptionResponse(Localizations.NoPermissionToViewLitigation);
                }
                if (PropertiesIntegrateDto.FirstPartyDto.IsPretender == true)
                {
                    PropertiesIntegrateDto.SecondPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                    PropertiesIntegrateDto.FirstPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                    PropertiesIntegrateDto.FirstPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                    PropertiesIntegrateDto.FirstPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    PropertiesIntegrateDto.FirstPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    PropertiesIntegrateDto.FirstPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                }
                if (PropertiesIntegrateDto.SecondPartyDto.IsPretender == true)
                {
                    PropertiesIntegrateDto.SecondPartyDto.AgencyNumber = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? "123456789" : null;
                    PropertiesIntegrateDto.SecondPartyDto.LawLicenseExpirationDate = ownerType == LitigationRequestOwnerType.Lawyer ? DateTime.Now.AddDays(5) : null;
                    PropertiesIntegrateDto.SecondPartyDto.LawLicenseNumber = ownerType == LitigationRequestOwnerType.Lawyer ? "789456123" : null;
                    PropertiesIntegrateDto.SecondPartyDto.EstablishmentContract = ownerType == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    PropertiesIntegrateDto.SecondPartyDto.AgancyAttachment = ownerType == LitigationRequestOwnerType.Agent || ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                    PropertiesIntegrateDto.SecondPartyDto.LawLicenseAttachment = ownerType == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                }


                return new DataResponse<PropertiesIntegrateDto> { IsDone = true, Message = Localizations.RetrievedSuccefully, Data = PropertiesIntegrateDto };
            }
            else
                return new DataResponse<PropertiesIntegrateDto> { IsDone = false, Message = Localizations.ContractNotFound, Data = new PropertiesIntegrateDto() };
        }

        public async Task<int> GetContractCountByContractType(LitigationRequestContractType litigationRequestContractTypeId)
        {
            var quarable = await GetQueryableAsync();
            int count = 0;
            count =  quarable.Where(l => l.LitigationRequestContractTypeId == litigationRequestContractTypeId).Count();
            return count;
        }
    }
}
