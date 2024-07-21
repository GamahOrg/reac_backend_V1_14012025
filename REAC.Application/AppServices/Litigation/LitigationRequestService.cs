using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using REAC.Application.Contracts.Dtos.Integration.Party;
using REAC.Application.Contracts.Dtos.Integration.Rent;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Get;
using REAC.Application.Contracts.Dtos.Litigation.List;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Common;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.System;
using REAC.Application.Contracts.IModuleServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using REAC.EntityFrameworkCore.Seed.Integration;
using static REAC.Application.Contracts.IAppServices.Litigation.ILitigationRequestService;



namespace REAC.Application.AppServices.Litigation
{
    public partial class LitigationRequestService : BasicService<LitigationRequest, Guid>, ILitigationRequestService
    {
        private readonly REACDBContext _context;
        private readonly IUserProviderService _contractService;
        private readonly ISysLitigationRequestAmountService _amountService;
        private readonly IJwtReader _jwtReader;
        private readonly ISysFileManagerService _fileService;
        private readonly ILitigationRequestRequestService _requestsService;
        private readonly ILitigationRequestRequestFileService _requestFileService;
        private readonly ILitigationRequestGroundService _groundService;
        private readonly ILitigationRequestGroundFileService _groundFileService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IJsonStringLocalizer _localization;


        public LitigationRequestService(REACDBContext context, ILitigationRequestGroundFileService groundFileService, IServiceProvider service,
            ILitigationRequestGroundService groundService, ILitigationRequestRequestFileService requestFileService, ILitigationRequestRequestService requestsService,
            ISysFileManagerService fileService, ISysLitigationRequestAmountService amountService, IUserProviderService contractService, IEntityRepository repository,
            IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader
            , IJsonStringLocalizer localization) : base(repository, mapper, loggerManager, jwtReader)

        {
            // _workFlowLitigation = workFlowLitigation;
            _context = context;
            _jwtReader = jwtReader;
            _contractService = contractService;
            _amountService = amountService;
            _fileService = fileService;
            _requestsService = requestsService;
            _requestFileService = requestFileService;
            _groundService = groundService;
            _groundFileService = groundFileService;
            _serviceProvider = service;
            _localization = localization;
        }

        public async Task<BaseResponse> AddLitigation(LitigationCreateUpdateDto model)
        {
            return await Add(model);
        }
        public async Task<BaseResponse> Add(LitigationCreateUpdateDto model)
        {
            try
            {
                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.Contract)
                          .Include(x => x.LitigationRequestRequests)
                          .Include(x => x.LitigationRequestGrounds)
                          .ThenInclude(x => x.LitigationRequestGroundFiles)

                          .Where(x => x.Contract.Id == model.ContractId).ToList().FirstOrDefault();


                litigationRequest.Title = model.Title;
                litigationRequest.Description = model.Description;
                litigationRequest.IsFinancialLitigation = model.IsFinancialLitigation;

                if (model.LitigationRequestAmountId != Guid.Empty)
                    litigationRequest.LitigationRequestAmountId = model.LitigationRequestAmountId;
                litigationRequest.AreaId = model.AreaId;
                litigationRequest.CityId = model.CityId;

                litigationRequest.IsExemptionReduction = model.ExemptionReductionCategory.IsExemptionReductionCategory;

                if (model.ExemptionReductionCategory.AttachmentId != Guid.Empty)
                    litigationRequest.ExemptionReductionFileId = model.ExemptionReductionCategory.AttachmentId;


                if (model.ExemptionReductionCategory.Id != Guid.Empty)
                    litigationRequest.ExemptionReductionCategoryId = model.ExemptionReductionCategory.Id;

                litigationRequest.LitigationRequestCategoryId = model.LitigationRequestCategoryId;


                foreach (var request in model.Requests)
                {
                    litigationRequest.LitigationRequestRequests.Add(new EntityFrameworkCore.Entities.Litigation.LitigationRequestRequest()
                    {
                        Id = Guid.NewGuid(),
                        LitigationRequestId = litigationRequest.Id,
                        RequestContent = request.LitigationPurpose
                    });
                }

                foreach (var reference in model.References)
                {
                    LitigationRequestGround litigationRequestGround = new LitigationRequestGround();
                    litigationRequestGround.Id = Guid.NewGuid();
                    litigationRequestGround.LitigationRequestId = litigationRequest.Id;
                    litigationRequestGround.Title = reference.ReferenceTitle;
                    foreach (var file in reference.ReferenceAttachments)
                    {
                        litigationRequestGround.LitigationRequestGroundFiles.Add(new LitigationRequestGroundFile()
                        {
                            LitigationRequestGroundId = litigationRequestGround.Id,
                            Id = Guid.NewGuid(),
                            FileManagerId = file
                        });
                    }

                    litigationRequest.LitigationRequestGrounds.Add(litigationRequestGround);
                }


                if (model.IsSent)
                {
                    var litigationRequestList = (await _repository.GetQueryable<LitigationRequest, Guid>())
                     .Where(x => x.RequestCode != null).ToList();

                    string requestCode = "REQ_00000001";

                    if (litigationRequestList == null)
                    {
                        requestCode = "REQ_00000001";
                    }
                    else
                    {
                        requestCode = "REQ_" + (litigationRequestList.Count() + 1).ToString("D8");
                    }
                    litigationRequest.IsSent = model.IsSent;
                    litigationRequest.SentOn = DateTime.Now;
                    litigationRequest.RequestCode = requestCode;
                    if (litigationRequest.InternalStatusId == RequestStatus.SaveAsDraft)
                    {
                        litigationRequest.ExternalStatusId = RequestStatus.New;
                        litigationRequest.InternalStatusId = RequestStatus.New;
                    }
                }



                var saveResponse = _repository.SaveChangesAsync().Result;
                if (!saveResponse.IsDone)
                    return saveResponse;



                if (model.IsSent)
                {
                    var _bpfService = _serviceProvider.GetRequiredService<IBpfService>();
                    var _stepService = _serviceProvider.GetRequiredService<IWorkFlowStepService>();

                    var processingLog = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())
                        .Include(t => t.WorkFlowStep)
                         .Include(t => t.WorkFlowStep.WorkFlowStepEvents)
                    .AsNoTracking()
                          .Where(x => x.Id == litigationRequest.Id).ToList();

                    if (processingLog.Count > 0)
                    {
                        var currentProcessingLog = processingLog.Where(t => t.IsFinished == false).ToList();
                        if (processingLog.Count == 1)
                        {
                            ProcessRequestPayload processRequestPayload = new ProcessRequestPayload();
                            processRequestPayload.RequestId = litigationRequest.Id;
                            processRequestPayload.ProcessingLogId = currentProcessingLog.FirstOrDefault().Id;
                            processRequestPayload.WorkflowStepEventId = currentProcessingLog.FirstOrDefault().WorkFlowStep.WorkFlowStepEvents.FirstOrDefault().Id;
                            processRequestPayload.Notes = "";
                            processRequestPayload.UploadedFilesIds = new List<Guid>();
                            await _bpfService.ProcessWorkFlow(processRequestPayload, default);
                            return new DataResponse<LitigationCreateUpdateDto>()
                            {
                                Data = model,
                                IsDone = true,
                                Message = "08888888"
                            };
                        }
                    }
                    else
                        await _bpfService.ExcuteWorkFlow(litigationRequest.Id, default);
                    return new DataResponse<LitigationCreateUpdateDto>()
                    {
                        Data = model,
                        IsDone = true,
                        Message = "0844444444"
                    };
                }

                return new DataResponse<LitigationCreateUpdateDto>()
                {
                    Data = model,
                    IsDone = true,
                    Message = "0111111111"
                };

            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Exception = ex.Message + "\n" + ex.ToString(),
                    IsDone = false,

                };

            }

        }
        public async Task<Guid> GetAddedLitigation()
        {
            var quarable = await GetQueryableAsync();
            var latestAdded = quarable.OrderByDescending(x => x.CreatedOn).First();
            return latestAdded.Id;
        }

        public async Task<BaseResponse> List(int pageIndex, int pageSize, string? searchKey, LitigationListDto model)
        {

            var litigations = await GetQueryableAsync();
            litigations = litigations.Include(x => x.Contract);
            litigations = litigations.Include(x => x.Contract.LitigationRequestContractType);
            litigations = litigations.Include(x => x.Contract.FirstPartyCitizen);
            litigations = litigations.Include(x => x.Contract.SecondPartyCitizen);
            litigations = litigations.Include(x => x.Contract.FirstPartyFacility);
            litigations = litigations.Include(x => x.Contract.SecondPartyFacility);
            if (!string.IsNullOrEmpty(searchKey))
            {
                litigations = litigations.Where(l => l.RequestCode.Contains(searchKey) || l.Contract.SecondPartyName.Contains(searchKey));
            }

            if (model.InternalStatusId.HasValue)
            {
                litigations = litigations.Where(l => l.InternalStatusId == model.InternalStatusId.Value);
            }

            if (model.LitigationRequestContractTypeId.HasValue)
            {
                litigations = litigations.Where(l => l.Contract.LitigationRequestContractTypeId == model.LitigationRequestContractTypeId.Value);
            }

            litigations = litigations.OrderByDescending(x => x.CreatedOn);
            var list = _mapper.Map<List<LitigationListDto>>(litigations);



            var result = await PaginatedList<LitigationListDto>.CreateAsync(list, pageIndex, pageSize);
            return new DataResponse<PaginatedList<LitigationListDto>>()
            {
                Data = result,
                IsDone = true
            };
        }

        public async Task<BaseResponse> AuditngList(int pageIndex, int pageSize, string? searchKey, AuditingListDto model)
        {
            //var logdenUser = Guid.Parse("1c1b6018-69c8-43f5-b5b1-72cea64d0f9f");/*_jwtReader.GetUserId();*/

            var litigations = await GetQueryableAsync();
            litigations = litigations.Include(x => x.Contract);
            litigations = litigations.Include(x => x.Contract.LitigationRequestContractType);
            litigations = litigations.Include(x => x.Contract.FirstPartyCitizen);
            litigations = litigations.Include(x => x.Contract.SecondPartyCitizen);
            litigations = litigations.Include(x => x.Contract.FirstPartyFacility);
            litigations = litigations.Include(x => x.Contract.SecondPartyFacility);
            if (!string.IsNullOrEmpty(searchKey))
            {
                litigations = litigations.Where(l => l.RequestCode.Contains(searchKey) || l.Contract.SecondPartyName.Contains(searchKey));
            }

            if (model.InternalStatusId.HasValue)
            {
                litigations = litigations.Where(l => l.InternalStatusId == model.InternalStatusId.Value);
            }

            if (model.LitigationRequestContractTypeId.HasValue)
            {
                litigations = litigations.Where(l => l.Contract.LitigationRequestContractTypeId == model.LitigationRequestContractTypeId.Value);
            }

            /*   if (model.AuditngRemaningDays.HasValue)
               {
                   litigations = litigations.Where(l => l.AuditngRemaningDays == model.AuditngRemaningDays.Value);
               }
            */
            litigations = litigations.OrderByDescending(x => x.CreatedOn);
            var list = _mapper.Map<List<AuditingListDto>>(litigations).Where(x => x.InternalStatusId != RequestStatus.SaveAsDraft).ToList();
            var result = await PaginatedList<AuditingListDto>.CreateAsync(list, pageIndex, pageSize);
            return new DataResponse<PaginatedList<AuditingListDto>>()
            {
                Data = result,
                IsDone = true
            };

        }

        public async Task<BaseResponse> Get(Guid litigationId)
        {
            var quarble = await GetQueryableAsync();
            var result = quarble.Include(x => x.Contract)
                .Include(x => x.LitigationRequestCategory)
                .Include(x => x.Area).Include(x => x.City)
                .Include(x => x.ApplicantCitizen)
                .Include(x => x.Contract.SecondPartyCitizen)
                .Include(x => x.Contract.FirstPartyCitizen)
                 .Include(x => x.Contract.FirstPartyFacility)
                .Include(x => x.Contract.SecondPartyFacility)
                .Include(x => x.LitigationRequestGrounds)
                .Include(x => x.LitigationRequestRequests)
                .FirstOrDefault(x => x.Id == litigationId);
            return new DataResponse<LitigationRequest> { Data = result, IsDone = true };
        }
        public async Task<int> GetLitigationsCountByType(LitigationRequestContractType contractType)
        {
            var quarble = await GetQueryableAsync();
            int count = 0;

            switch (contractType)
            {
                case LitigationRequestContractType.Rent:
                    count = quarble.Where(x => x.RequestCode.StartsWith('R')).Count();
                    break;

                case LitigationRequestContractType.Properties:
                    count = quarble.Where(x => x.RequestCode.StartsWith('O')).Count();
                    break;

                case LitigationRequestContractType.Wafi:
                    count = quarble.Where(x => x.RequestCode[0] == 'W').Count();
                    break;

                case LitigationRequestContractType.RealEstateBrokerage:
                    count = quarble.Where(x => x.RequestCode[0] == 'B').Count();
                    break;

                case LitigationRequestContractType.OtherServices:
                    count = quarble.Where(x => x.RequestCode[0] == 'N').Count();
                    break;
            }
            return count;
        }

        public async Task<BaseResponse> GetLitigationPartiesDetails(Guid litigationId)
        {
            var quarble = await GetQueryableAsync();
            var result = quarble.Include(x => x.Contract)
                .Include(x => x.LitigationRequestCategory)
                .Include(x => x.Area)
                .Include(x => x.City)
                .Include(x => x.ApplicantCitizen)
                .Include(x => x.Contract.SecondPartyCitizen)
                                .Include(x => x.Contract.SecondPartyCitizen.Nationality)

                .Include(x => x.Contract.FirstPartyCitizen)
                                .Include(x => x.Contract.FirstPartyCitizen.Nationality)

                .Include(x => x.Contract.SecondPartyFacility)
                .Include(x => x.Contract.FirstPartyFacility)
                .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                .Include(x => x.Contract.ContractFile)
                .Include(x => x.LitigationRequestRequests)
                .Include(x => x.SysExemptionReductionCategory)
                .Include(x => x.LitigationRequestGrounds)
                .ThenInclude(x => x.LitigationRequestGroundFiles)
                .FirstOrDefault(x => x.Id == litigationId);

            if (result == null)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            LitigationPartiesDetailsDto litigationPartiesDetailsDto = new LitigationPartiesDetailsDto();
            litigationPartiesDetailsDto.FirstPartyDto = new PartyDTO();
            litigationPartiesDetailsDto.SecondPartyDto = new PartyDTO();

            if (result.Contract.FirstPartyCitizenId.HasValue)
            {

                litigationPartiesDetailsDto.FirstPartyDto.Nationality = result.Contract.FirstPartyCitizen.Nationality.NationalityName;
                litigationPartiesDetailsDto.FirstPartyDto.FullName = result.Contract.FirstPartyCitizen.FullName;

                litigationPartiesDetailsDto.FirstPartyDto.Mobile = result.Contract.FirstPartyCitizen.Mobile;
                litigationPartiesDetailsDto.FirstPartyDto.Email = result.Contract.FirstPartyCitizen.Email;
                litigationPartiesDetailsDto.FirstPartyDto.FullName = result.Contract.FirstPartyCitizen.FullName;
                litigationPartiesDetailsDto.FirstPartyDto.AgencyNumber = "1234567890";

                litigationPartiesDetailsDto.FirstPartyDto.CitizenId = result.Contract.FirstPartyCitizen.Id;
                litigationPartiesDetailsDto.FirstPartyDto.BirthDate = result.Contract.FirstPartyCitizen.BirthDate;
                litigationPartiesDetailsDto.FirstPartyDto.IdentityNumber = result.Contract.FirstPartyCitizen.IdentityNumber;
                litigationPartiesDetailsDto.FirstPartyDto.IsPretender = true;
                litigationPartiesDetailsDto.FirstPartyDto.Capacity = result.Contract.FirstPartyCapacity;
                litigationPartiesDetailsDto.FirstPartyDto.NationalAddress = result.Contract.FirstPartyCitizen.NationalAddress;

                litigationPartiesDetailsDto.FirstPartyDto.AgancyAttachment = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                litigationPartiesDetailsDto.FirstPartyDto.LawLicenseAttachment = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? ((result.ApplicantCitizen.LawLicenseFile != null) ? new SysFileManagerCreateUpdateDto { Id = result.ApplicantCitizen.LawLicenseFile.Id, FileDescription = result.ApplicantCitizen.LawLicenseFile.FileDescription, FilePath = result.ApplicantCitizen.LawLicenseFile.FilePath, FileType = result.ApplicantCitizen.LawLicenseFile.FileType } : null) : null;
                litigationPartiesDetailsDto.FirstPartyDto.EstablishmentContract = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
            }
            if (result.Contract.FirstPartyFacilityId.HasValue)
            {
                litigationPartiesDetailsDto.FirstPartyDto.IsPretender = true;
                litigationPartiesDetailsDto.FirstPartyDto.LawLicenseExpirationDate = result.ApplicantCitizen.LawyerLicenseExpirationDate;
                litigationPartiesDetailsDto.FirstPartyDto.LawLicenseNumber = result.ApplicantCitizen.LawyerLicenseNumber;
                litigationPartiesDetailsDto.FirstPartyDto.Capacity = result.Contract.FirstPartyCapacity;
                litigationPartiesDetailsDto.FirstPartyDto.FacilityName = result.Contract.FirstPartyFacility.ArabicFacilityName;
                litigationPartiesDetailsDto.FirstPartyDto.FacilityNumber = result.Contract.FirstPartyFacility.Facility700Number;
                litigationPartiesDetailsDto.FirstPartyDto.CommercialRegistrationNo = result.Contract.FirstPartyFacility.CommercialRegisterNumber;
                litigationPartiesDetailsDto.FirstPartyDto.AgencyNumber = "1234567890";
            }


            if (result.Contract.SecondPartyCitizenId.HasValue)
            {
                litigationPartiesDetailsDto.SecondPartyDto.Nationality = result.Contract.SecondPartyCitizen.Nationality.NationalityName;
                litigationPartiesDetailsDto.SecondPartyDto.FullName = result.Contract.SecondPartyCitizen.FullName;

                litigationPartiesDetailsDto.SecondPartyDto.Mobile = result.Contract.SecondPartyCitizen.Mobile;
                litigationPartiesDetailsDto.SecondPartyDto.Email = result.Contract.SecondPartyCitizen.Email;
                litigationPartiesDetailsDto.SecondPartyDto.FullName = result.Contract.SecondPartyCitizen.FullName;
                litigationPartiesDetailsDto.SecondPartyDto.AgencyNumber = "1234567890";

                litigationPartiesDetailsDto.SecondPartyDto.CitizenId = result.Contract.SecondPartyCitizen.Id;
                litigationPartiesDetailsDto.SecondPartyDto.BirthDate = result.Contract.SecondPartyCitizen.BirthDate;
                litigationPartiesDetailsDto.SecondPartyDto.IdentityNumber = result.Contract.SecondPartyCitizen.IdentityNumber;
                litigationPartiesDetailsDto.SecondPartyDto.IsPretender = false;
                litigationPartiesDetailsDto.SecondPartyDto.Capacity = result.Contract.SecondPartyCapacity;
                litigationPartiesDetailsDto.SecondPartyDto.NationalAddress = result.Contract.SecondPartyCitizen.NationalAddress;

                litigationPartiesDetailsDto.SecondPartyDto.AgancyAttachment = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;
                litigationPartiesDetailsDto.SecondPartyDto.LawLicenseAttachment = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? ((result.ApplicantCitizen.LawLicenseFile != null) ? new SysFileManagerCreateUpdateDto { Id = result.ApplicantCitizen.LawLicenseFile.Id, FileDescription = result.ApplicantCitizen.LawLicenseFile.FileDescription, FilePath = result.ApplicantCitizen.LawLicenseFile.FilePath, FileType = result.ApplicantCitizen.LawLicenseFile.FileType } : null) : null;
                litigationPartiesDetailsDto.SecondPartyDto.EstablishmentContract = result.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null;

            }
            if (result.Contract.SecondPartyFacilityId.HasValue)
            {
                litigationPartiesDetailsDto.FirstPartyDto.IsPretender = false;
                litigationPartiesDetailsDto.SecondPartyDto.LawLicenseExpirationDate = result.ApplicantCitizen.LawyerLicenseExpirationDate;
                litigationPartiesDetailsDto.SecondPartyDto.LawLicenseNumber = result.ApplicantCitizen.LawyerLicenseNumber;
                litigationPartiesDetailsDto.SecondPartyDto.Capacity = result.Contract.SecondPartyCapacity;
                litigationPartiesDetailsDto.SecondPartyDto.FacilityName = result.Contract.SecondPartyFacility.ArabicFacilityName;
                litigationPartiesDetailsDto.SecondPartyDto.FacilityNumber = result.Contract.SecondPartyFacility.Facility700Number;
                litigationPartiesDetailsDto.SecondPartyDto.CommercialRegistrationNo = result.Contract.SecondPartyFacility.CommercialRegisterNumber;
                litigationPartiesDetailsDto.SecondPartyDto.AgencyNumber = "1234567890";
            }

            litigationPartiesDetailsDto.Key = result.Contract.ContractKey;
            litigationPartiesDetailsDto.SubKey = result.Contract.VersionNumber;


            litigationPartiesDetailsDto.ContractAddress = result.ContractAddress;
            //litigationPartiesDetailsDto.AssociationName = "جمعية";
            litigationPartiesDetailsDto.Id = result.ContractId;

            litigationPartiesDetailsDto.LitigationRequestOwnerType = result.LitigationRequestOwnerTypeId;
            if (result.Contract.ContractFile != null)
            {
                litigationPartiesDetailsDto.ContractFile = new SysFileManagerCreateUpdateDto()
                {
                    Id = result.Contract.ContractFile.Id,
                    FileType = result.Contract.ContractFile.FileType,
                    FileDescription = result.Contract.ContractFile.FileDescription,
                    FilePath = result.Contract.ContractFile.FilePath,
                };
            }
            if (result.Contract.AssociationRegistrationCertificateFile != null)
            {
                litigationPartiesDetailsDto.AssociationRegistrationCertificateFile = new SysFileManagerCreateUpdateDto()
                {
                    Id = result.Contract.AssociationRegistrationCertificateFile.Id,
                    FileType = result.Contract.AssociationRegistrationCertificateFile.FileType,
                    FileDescription = result.Contract.AssociationRegistrationCertificateFile.FileDescription,
                    FilePath = result.Contract.AssociationRegistrationCertificateFile.FilePath,
                };
            }
            if (result.Contract.FoundingLetterFile != null)
            {
                litigationPartiesDetailsDto.FoundingLetterFile = new SysFileManagerCreateUpdateDto()
                {
                    Id = result.Contract.FoundingLetterFile.Id,
                    FileType = result.Contract.FoundingLetterFile.FileType,
                    FileDescription = result.Contract.FoundingLetterFile.FileDescription,
                    FilePath = result.Contract.FoundingLetterFile.FilePath,
                };
            }
            if (result.Contract.InternalRegulationsFile != null)
            {
                litigationPartiesDetailsDto.InternalRegulationsFile = new SysFileManagerCreateUpdateDto()
                {
                    Id = result.Contract.InternalRegulationsFile.Id,
                    FileType = result.Contract.InternalRegulationsFile.FileType,
                    FileDescription = result.Contract.InternalRegulationsFile.FileDescription,
                    FilePath = result.Contract.InternalRegulationsFile.FilePath,
                };
            }


            foreach (var item in result.LitigationRequestRequests)
            {
                litigationPartiesDetailsDto.Requests.Add(item);
            }
            foreach (var item in result.LitigationRequestGrounds)
            {
                litigationPartiesDetailsDto.References.Add(item);
            }


            return new DataResponse<LitigationPartiesDetailsDto>() { Data = litigationPartiesDetailsDto, IsDone = true };
        }

        public async Task<BaseResponse> GetLitigationDetails(Guid litigationId)
        {
            try
            {
                var quarble = await GetQueryableAsync();
                var result = quarble.Include(x => x.Contract)
                    .Include(x => x.LitigationRequestCategory)
                    .Include(x => x.Area)
                    .Include(x => x.City)
                    .Include(x => x.ExemptionReductionFile)
                    .Include(x => x.ApplicantCitizen)
                    .Include(x => x.Contract.SecondPartyCitizen)
                                        .Include(x => x.SysExemptionReductionCategory)

                    .Include(x => x.ExemptionReductionFile)
                    .Include(x => x.Contract.FirstPartyCitizen).Include(x => x.LitigationRequestRequests)
                    .Include(x => x.LitigationRequestGrounds).ThenInclude(x => x.LitigationRequestGroundFiles)
                    .FirstOrDefault(x => x.Id == litigationId);

                var mapped = _mapper.Map<LitigationDetailsDto>(result);
                if (result.LitigationRequestRequests != null)
                {
                    foreach (var item in result.LitigationRequestRequests)
                    {
                        mapped.Requests.Add(new LitigationRequestRequestCreateUpdateDto()
                        {
                            Id = item.Id,
                            RequestContent = item.RequestContent
                        });

                    }
                }
                mapped.References = new List<LitigationRequestGroundCreateUpdateDto>();
                if (result.LitigationRequestGrounds != null)
                {
                    foreach (var item in result.LitigationRequestGrounds)
                    {
                        LitigationRequestGroundCreateUpdateDto litigationRequestGroundCreateUpdateDto = new LitigationRequestGroundCreateUpdateDto();
                        litigationRequestGroundCreateUpdateDto.Id = item.Id;
                        litigationRequestGroundCreateUpdateDto.Title = item.Title;
                        litigationRequestGroundCreateUpdateDto.ReferenceFiles = new List<SysFileManagerCreateUpdateDto>();
                        if (item.LitigationRequestGroundFiles != null)
                        {
                            foreach (var fileItem in item.LitigationRequestGroundFiles)
                            {
                                if (fileItem.FileManager != null)
                                {
                                    litigationRequestGroundCreateUpdateDto.ReferenceFiles.Add(new SysFileManagerCreateUpdateDto()
                                    {
                                        Id = fileItem.FileManager.Id,
                                        FileDescription = fileItem.FileManager.FileDescription,
                                        FileType = fileItem.FileManager.FileType,
                                        FilePath = fileItem.FileManager.FilePath,
                                    });
                                }
                            }
                        }
                        mapped.References.Add(litigationRequestGroundCreateUpdateDto);
                    }
                }
                if (result.SysExemptionReductionCategory != null)
                {
                    mapped.ExemptionReductionCategory = new Contracts.Dtos.System.LitigationRequestCategoryDtos.SysExemptionReductionCategoryCreateUpdateDto()
                    {
                        Id = result.SysExemptionReductionCategory.Id,
                        ExemptionReductionCategoryName = result.SysExemptionReductionCategory.ExemptionReductionCategoryName
                    };
                }

                if (result.ExemptionReductionFile != null)
                {
                    mapped.ExemptionReductionFile = new SysFileManagerCreateUpdateDto()
                    {
                        Id = result.ExemptionReductionFile.Id,
                        FileType = result.ExemptionReductionFile.FileType,
                        FileDescription = result.ExemptionReductionFile.FileDescription,
                        FilePath = result.ExemptionReductionFile.FilePath,
                    };
                }

                return new DataResponse<LitigationDetailsDto> { Data = mapped, IsDone = true };
            }
            catch (Exception ex) { return new ExceptionResponse() { Exception = ex.Message + "\b" + ex.ToString(), IsDone = false }; };
        }

        public async Task<BaseResponse> CreateLitigationRequest(LitigationRequestDto litigationRequestDto)
        {
            Guid citizenId = Guid.Empty;
            _jwtReader.GetReferenceId(ref citizenId);
            switch (litigationRequestDto.ContractType)
            {
                case LitigationRequestContractType.Rent:
                    return await ValidateRentContract(litigationRequestDto, citizenId);
                case LitigationRequestContractType.Properties:
                    return await ValidatePropertiesContract(litigationRequestDto, citizenId);
                case LitigationRequestContractType.Wafi:
                    return await ValidateWafiContract(litigationRequestDto, citizenId);
                case LitigationRequestContractType.RealEstateBrokerage:
                    return await ValidateRealEstateBrokerageContract(litigationRequestDto, citizenId);
                case LitigationRequestContractType.OtherServices:
                    return await ValidateOtherServicesContract(litigationRequestDto, citizenId);
                default:
                    throw new NotImplementedException();
            }

        }

        public async Task<BaseResponse> ValidateRentContract(LitigationRequestDto litigationRequestDto, Guid userId)
        {
            var contracts = ContractIntegration.GetContracts().Where(t => t.ContractKey == litigationRequestDto.key && t.VersionNumber == litigationRequestDto.SubKey && t.LitigationRequestContractTypeId == LitigationRequestContractType.Rent).ToList();
            if (!contracts.Any())
                return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };

            LitigationRequestContract litigationRequestContract = new LitigationRequestContract()
            {
                Id = Guid.NewGuid(),
                LitigationRequestContractTypeId = LitigationRequestContractType.Rent,
                ContractKey = litigationRequestDto.key,
                VersionNumber = litigationRequestDto.SubKey,

            };
            if (!(await ValidateContractParties(litigationRequestDto, userId, contracts, litigationRequestContract)).IsDone)
                return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };

            LitigationRequest litigationRequest = new LitigationRequest()
            {
                Id = Guid.NewGuid(),
                ContractId = litigationRequestContract.Id,
                ApplicantCitizenId = userId,
                LitigationRequestOwnerTypeId = litigationRequestDto.LitigationRequestOwnerType,
                InternalStatusId = RequestStatus.SaveAsDraft,
                ExternalStatusId = RequestStatus.SaveAsDraft,
                Fees = 0,
                IsPaid = false,
                CreatedOn = DateTime.Now,
                IsSent = false,
                ContractAddress = "شارع القصر - الرياض"
            };
            await _repository.AddAsync<LitigationRequestContract, Guid>(litigationRequestContract);

            await _repository.AddAsync<LitigationRequest, Guid>(litigationRequest);

            var saveResponse = await _repository.SaveChangesAsync();
            if (saveResponse.IsDone)
            {
                litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.ApplicantCitizen.LawLicenseFile)
                          .Include(x => x.ApplicantCitizen.IdentityFile)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.FirstPartyCitizen.Nationality)
                          .Include(x => x.Contract.FirstPartyCitizen.FromDelegators)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Include(x => x.Contract.SecondPartyCitizen.Nationality)

                          .Include(x => x.Contract.SecondPartyCitizen.FromDelegators)

                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyFacility.Delegators)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyFacility.Delegators)

                          .AsNoTracking()
                          .Where(x => x.Id == litigationRequest.Id).ToList().FirstOrDefault();

                return ReturnLitigationRequestDto(litigationRequest);
            }
            else
                return saveResponse;
        }

        private static BaseResponse ReturnLitigationRequestDto(LitigationRequest litigationRequest)
        {

            RentIntegrateDto rentIntegrateDto = new RentIntegrateDto()
            {
                FirstPartyDto = new PartyDTO()
                {

                    IdentityNumber = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.IdentityNumber,
                    NationalAddress = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.NationalAddress,
                    CommercialRegistrationNo = (litigationRequest.Contract.FirstPartyFacility != null) ? litigationRequest.Contract.FirstPartyFacility.CommercialRegisterNumber : null,
                    FacilityName = (litigationRequest.Contract.FirstPartyFacility == null) ? null : litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName,
                    Nationality = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.Nationality.NationalityName,
                    FullName = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.FullName,
                    Mobile = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.Mobile,
                    Email = (litigationRequest.Contract.FirstPartyCitizen == null) ? null : litigationRequest.Contract.FirstPartyCitizen.Email,
                    FacilityNumber = (litigationRequest.Contract.FirstPartyFacility != null) ? litigationRequest.Contract.FirstPartyFacility.Facility700Number : null,
                    CommercialRegisterNumber = (litigationRequest.Contract.FirstPartyFacility != null) ? litigationRequest.Contract.FirstPartyFacility.CommercialRegisterNumber : null,

                    BirthDate = (litigationRequest.Contract.FirstPartyCitizen != null) ? litigationRequest.Contract.FirstPartyCitizen.BirthDate : null,
                    Capacity = litigationRequest.Contract.FirstPartyCapacity,
                    IsPretender = true,
                    LawLicenseNumber = litigationRequest.ApplicantCitizen.LawyerLicenseNumber,
                    LawLicenseExpirationDate = litigationRequest.ApplicantCitizen.LawyerLicenseExpirationDate,
                    AgencyNumber = "1234567890", // (litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer) || (litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Agent ) ? litigationRequest.Contract. : null,
                    AgancyAttachment = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null,
                    LawLicenseAttachment = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? ((litigationRequest.ApplicantCitizen.LawLicenseFile != null) ? new SysFileManagerCreateUpdateDto { Id = litigationRequest.ApplicantCitizen.LawLicenseFile.Id, FileDescription = litigationRequest.ApplicantCitizen.LawLicenseFile.FileDescription, FilePath = litigationRequest.ApplicantCitizen.LawLicenseFile.FilePath, FileType = litigationRequest.ApplicantCitizen.LawLicenseFile.FileType } : null) : null,
                    EstablishmentContract = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null,
                },
                SecondPartyDto = new PartyDTO()
                {
                    IdentityNumber = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.IdentityNumber,
                    NationalAddress = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.NationalAddress,
                    CommercialRegistrationNo = (litigationRequest.Contract.SecondPartyFacility != null) ? litigationRequest.Contract.SecondPartyFacility.CommercialRegisterNumber : null,
                    FacilityName = (litigationRequest.Contract.SecondPartyFacility != null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : null,
                    Nationality = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.Nationality.NationalityName,
                    FullName = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.FullName,
                    Mobile = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.Mobile,
                    Email = (litigationRequest.Contract.SecondPartyCitizen == null) ? null : litigationRequest.Contract.SecondPartyCitizen.Email,
                    FacilityNumber = (litigationRequest.Contract.SecondPartyFacility != null) ? litigationRequest.Contract.SecondPartyFacility.Facility700Number : null,
                    CommercialRegisterNumber = (litigationRequest.Contract.SecondPartyFacility != null) ? litigationRequest.Contract.SecondPartyFacility.CommercialRegisterNumber : null,

                    BirthDate = (litigationRequest.Contract.SecondPartyCitizen != null) ? litigationRequest.Contract.SecondPartyCitizen.BirthDate : null,
                    Capacity = litigationRequest.Contract.SecondPartyCapacity,
                    IsPretender = true,
                    LawLicenseNumber = litigationRequest.ApplicantCitizen.LawyerLicenseNumber,
                    LawLicenseExpirationDate = litigationRequest.ApplicantCitizen.LawyerLicenseExpirationDate,
                    AgencyNumber = "1234567890", // (litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer) || (litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Agent ) ? litigationRequest.Contract. : null,
                    AgancyAttachment = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null,
                    LawLicenseAttachment = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.Lawyer ? ((litigationRequest.ApplicantCitizen.LawLicenseFile != null) ? new SysFileManagerCreateUpdateDto { Id = litigationRequest.ApplicantCitizen.LawLicenseFile.Id, FileDescription = litigationRequest.ApplicantCitizen.LawLicenseFile.FileDescription, FilePath = litigationRequest.ApplicantCitizen.LawLicenseFile.FilePath, FileType = litigationRequest.ApplicantCitizen.LawLicenseFile.FileType } : null) : null,
                    EstablishmentContract = litigationRequest.LitigationRequestOwnerTypeId == LitigationRequestOwnerType.CompanyRepresentative ? new SysFileManagerCreateUpdateDto { Id = Guid.Parse("e86953a3-c747-49f4-b817-21592334321f"), FileDescription = "FileDescription", FilePath = "reac/e86953a3-c747-49f4-b817-21592334321f.png", FileType = "Pdf" } : null,
                },
                Key = litigationRequest.Contract.ContractKey,
                SubKey = litigationRequest.Contract.VersionNumber,
                LitigationRequestOwnerType = litigationRequest.LitigationRequestOwnerTypeId,
                ContractAddress = litigationRequest.ContractAddress,
                Id = litigationRequest.ContractId
            };
            return new DataResponse<RentIntegrateDto>()
            {
                IsDone = true,
                Data = rentIntegrateDto
            };
        }

        private async Task<BaseResponse> ValidateContractParties(LitigationRequestDto litigationRequestDto, Guid userId, List<Contract> contracts, LitigationRequestContract litigationRequestContract)
        {
            switch (litigationRequestDto.LitigationRequestOwnerType)
            {
                case LitigationRequestOwnerType.Personal:

                    if (contracts.FirstOrDefault()?.FirstPartyCitizenId == userId)
                    {
                        litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                        litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                        litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                        if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                            litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                        else
                            litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;
                    }
                    else
                    {
                        if (contracts.FirstOrDefault()?.SecondPartyCitizenId == userId)
                        {
                            litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                            litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                            litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                            if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                            else
                                litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                        }
                        else
                            return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };
                    }
                    break;
                case LitigationRequestOwnerType.Agent:
                    bool goOn1 = true;
                    if (contracts.FirstOrDefault().FirstPartyCitizenId.HasValue)
                    {
                        var firstCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                            .Include(x => x.FromDelegators)
                            .AsNoTracking()
                            .Where(x => x.Id == contracts.FirstOrDefault().FirstPartyCitizenId).ToList();
                        foreach (var delegator in firstCitizen.FirstOrDefault().FromDelegators)
                        {
                            if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Agent)
                            {
                                litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                                litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                    litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                else
                                    litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;

                                goOn1 = false;
                            }
                        }

                    }
                    if (goOn1)
                    {
                        if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                        {
                            var secondCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                            .Include(x => x.FromDelegators)
                            .AsNoTracking()
                            .Where(x => x.Id == contracts.FirstOrDefault().SecondPartyCitizenId).ToList();

                            foreach (var delegator in secondCitizen.FirstOrDefault().FromDelegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Agent)
                                {
                                    litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                    if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;
                                }
                            }
                        }
                    }
                    if (goOn1)
                    {
                        if (contracts.FirstOrDefault().FirstPartyFacilityId.HasValue)
                        {
                            var firstFacility = (await _repository.GetQueryable<Facility, Guid>())
                                .Include(x => x.Delegators)
                                .AsNoTracking()
                                .Where(x => x.Id == contracts.FirstOrDefault().FirstPartyFacilityId).ToList();

                            foreach (var delegator in firstFacility.FirstOrDefault().Delegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Agent)
                                {
                                    litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                    if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;

                                    goOn1 = false;
                                }
                            }

                        }
                    }
                    if (goOn1)
                    {
                        if (contracts.FirstOrDefault().SecondPartyFacilityId.HasValue)
                        {
                            var secondFacility = (await _repository.GetQueryable<Facility, Guid>())
                                .Include(x => x.Delegators)
                                .AsNoTracking()
                                .Where(x => x.Id == contracts.FirstOrDefault().SecondPartyFacilityId).ToList();

                            foreach (var delegator in secondFacility.FirstOrDefault().Delegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Agent)
                                {
                                    litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;

                                    if (contracts.FirstOrDefault().FirstPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;

                                    goOn1 = false;
                                }
                            }
                        }
                    }
                    if (goOn1)
                        return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };
                    break;
                case LitigationRequestOwnerType.Lawyer:
                    bool goOn2 = true;
                    if (contracts.FirstOrDefault().FirstPartyCitizenId.HasValue)
                    {
                        var firstCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                            .Include(x => x.FromDelegators)
                            .AsNoTracking()
                            .Where(x => x.Id == contracts.FirstOrDefault().FirstPartyCitizenId).ToList();
                        foreach (var delegator in firstCitizen.FirstOrDefault().FromDelegators)
                        {
                            if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                            {
                                litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                                litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                    litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                else
                                    litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;

                                goOn2 = false;
                            }
                        }

                    }
                    if (goOn2)
                    {
                        if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                        {
                            var secondCitizen = (await _repository.GetQueryable<Citizen, Guid>())
                            .Include(x => x.FromDelegators)
                            .AsNoTracking()
                            .Where(x => x.Id == contracts.FirstOrDefault().SecondPartyCitizenId).ToList();

                            foreach (var delegator in secondCitizen.FirstOrDefault().FromDelegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                                {
                                    litigationRequestContract.FirstPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                    if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;

                                    goOn2 = false;
                                }
                            }
                        }
                    }
                    if (goOn2)
                    {
                        if (contracts.FirstOrDefault().FirstPartyFacilityId.HasValue)
                        {
                            var firstFacility = (await _repository.GetQueryable<Facility, Guid>())
                                .Include(x => x.Delegators)
                                .AsNoTracking()
                                .Where(x => x.Id == contracts.FirstOrDefault().FirstPartyFacilityId).ToList();

                            foreach (var delegator in firstFacility.FirstOrDefault().Delegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                                {
                                    litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                                    if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;

                                    goOn2 = false;
                                }
                            }

                        }
                    }
                    if (goOn2)
                    {
                        if (contracts.FirstOrDefault().SecondPartyFacilityId.HasValue)
                        {
                            var secondFacility = (await _repository.GetQueryable<Facility, Guid>())
                                .Include(x => x.Delegators)
                                .AsNoTracking()
                                .Where(x => x.Id == contracts.FirstOrDefault().SecondPartyFacilityId).ToList();

                            foreach (var delegator in secondFacility.FirstOrDefault().Delegators)
                            {
                                if (delegator.DelegatorCitizenId == userId && delegator.DelegationRelationshipTypeId == DelegationRelationshipType.Lawyer)
                                {
                                    litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;

                                    if (contracts.FirstOrDefault().FirstPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;

                                    goOn1 = false;
                                }
                            }
                        }
                    }
                    if (goOn2)
                        return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };
                    break;
                case LitigationRequestOwnerType.CompanyRepresentative:
                    bool goOn3 = true;
                    if (contracts.FirstOrDefault().FirstPartyFacilityId.HasValue)
                    {
                        var firstFacility = (await _repository.GetQueryable<Facility, Guid>())
                            .AsNoTracking()
                            .Where(x => x.Id == contracts.FirstOrDefault().FirstPartyFacilityId).ToList();

                        if (firstFacility.FirstOrDefault().CitizenId == userId)
                        {
                            litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;
                            litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;
                            litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;

                            if (contracts.FirstOrDefault().SecondPartyCitizenId.HasValue)
                                litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.SecondPartyCitizenId;
                            else
                                litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;

                            goOn3 = false;
                        }

                    }
                    if (goOn3)
                    {
                        if (contracts.FirstOrDefault().SecondPartyFacilityId.HasValue)
                        {
                            var secondFacility = (await _repository.GetQueryable<Facility, Guid>())
                                .Include(x => x.Delegators)
                                .AsNoTracking()
                                .Where(x => x.Id == contracts.FirstOrDefault().SecondPartyFacilityId).ToList();

                            foreach (var delegator in secondFacility.FirstOrDefault().Delegators)
                            {
                                if (secondFacility.FirstOrDefault()?.CitizenId == userId)
                                {
                                    litigationRequestContract.FirstPartyFacilityId = contracts.FirstOrDefault()?.SecondPartyFacilityId;
                                    litigationRequestContract.FirstPartyCapacity = contracts.FirstOrDefault()?.SecondPartyCapacity;
                                    litigationRequestContract.SecondPartyCapacity = contracts.FirstOrDefault()?.FirstPartyCapacity;

                                    if (contracts.FirstOrDefault().FirstPartyCitizenId.HasValue)
                                        litigationRequestContract.SecondPartyCitizenId = contracts.FirstOrDefault()?.FirstPartyCitizenId;
                                    else
                                        litigationRequestContract.SecondPartyFacilityId = contracts.FirstOrDefault()?.FirstPartyFacilityId;

                                    goOn3 = false;
                                }
                            }
                        }
                    }
                    if (goOn3)
                        return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };
                    break;
                default:
                    return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };
            }
            return new BaseResponse() { IsDone = true };

        }

        public async Task<BaseResponse> ValidatePropertiesContract(LitigationRequestDto litigationRequestDto, Guid userId)
        {
            var contracts = ContractIntegration.GetContracts().Where(t => t.ContractKey == litigationRequestDto.key &&
                    t.LitigationRequestContractTypeId == LitigationRequestContractType.Properties).ToList();
            if (!contracts.Any())
                return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };

            LitigationRequestContract litigationRequestContract = new LitigationRequestContract()
            {
                Id = Guid.NewGuid(),
                LitigationRequestContractTypeId = LitigationRequestContractType.Properties,
                ContractKey = litigationRequestDto.key,
                VersionNumber = litigationRequestDto.SubKey
            };
            await ValidateContractParties(litigationRequestDto, userId, contracts, litigationRequestContract);

            LitigationRequest litigationRequest = new LitigationRequest()
            {
                Id = Guid.NewGuid(),
                ContractId = litigationRequestContract.Id,
                ApplicantCitizenId = userId,
                LitigationRequestOwnerTypeId = litigationRequestDto.LitigationRequestOwnerType,
                InternalStatusId = RequestStatus.SaveAsDraft,
                ExternalStatusId = RequestStatus.SaveAsDraft,
                Fees = 0,
                IsPaid = false,
                CreatedOn = DateTime.Now,
                IsSent = false,
                IsAllowEdit = true,
                IsActive = true,
                IsFinished = false,
                ContractAddress = "شارع القصر - الرياض"
            };
            await _repository.AddAsync<LitigationRequestContract, Guid>(litigationRequestContract);

            await _repository.AddAsync<LitigationRequest, Guid>(litigationRequest);

            var saveResponse = await _repository.SaveChangesAsync();
            if (saveResponse.IsDone)
            {
                litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.ApplicantCitizen.LawLicenseFile)
                          .Include(x => x.ApplicantCitizen.IdentityFile)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.FirstPartyCitizen.Nationality)
                          .Include(x => x.Contract.FirstPartyCitizen.FromDelegators)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Include(x => x.Contract.SecondPartyCitizen.Nationality)

                          .Include(x => x.Contract.SecondPartyCitizen.FromDelegators)

                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyFacility.Delegators)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyFacility.Delegators)

                          .AsNoTracking()
                          .Where(x => x.Id == litigationRequest.Id).ToList().FirstOrDefault();

                return ReturnLitigationRequestDto(litigationRequest);
            }
            else
                return saveResponse;
        }

        public async Task<BaseResponse> ValidateWafiContract(LitigationRequestDto litigationRequestDto, Guid userId)
        {
            var contracts = ContractIntegration.GetContracts().Where(t => t.ContractKey == litigationRequestDto.key && t.LitigationRequestContractTypeId == LitigationRequestContractType.Wafi).ToList();
            if (!contracts.Any())
                return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };

            LitigationRequestContract litigationRequestContract = new LitigationRequestContract()
            {
                Id = Guid.NewGuid(),
                LitigationRequestContractTypeId = LitigationRequestContractType.Wafi,
                ContractKey = litigationRequestDto.key,
                VersionNumber = litigationRequestDto.SubKey
            };
            var validationResponse =
                await ValidateContractParties(litigationRequestDto, userId, contracts, litigationRequestContract);

            if (!validationResponse.IsDone)
                return validationResponse;

            LitigationRequest litigationRequest = new LitigationRequest()
            {
                Id = Guid.NewGuid(),
                ContractId = litigationRequestContract.Id,
                ApplicantCitizenId = userId,
                LitigationRequestOwnerTypeId = litigationRequestDto.LitigationRequestOwnerType,
                InternalStatusId = RequestStatus.SaveAsDraft,
                ExternalStatusId = RequestStatus.SaveAsDraft,
                Fees = 0,
                IsPaid = false,
                CreatedOn = DateTime.Now,
                IsSent = false,
                IsAllowEdit = true,
                IsActive = true,
                IsFinished = false,
                ContractAddress = "شارع القصر - الرياض"
            };
            await _repository.AddAsync<LitigationRequestContract, Guid>(litigationRequestContract);

            await _repository.AddAsync<LitigationRequest, Guid>(litigationRequest);

            var saveResponse = await _repository.SaveChangesAsync();
            if (saveResponse.IsDone)
            {
                litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.ApplicantCitizen.LawLicenseFile)
                          .Include(x => x.ApplicantCitizen.IdentityFile)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.FirstPartyCitizen.Nationality)
                          .Include(x => x.Contract.FirstPartyCitizen.FromDelegators)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Include(x => x.Contract.SecondPartyCitizen.Nationality)

                          .Include(x => x.Contract.SecondPartyCitizen.FromDelegators)

                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyFacility.Delegators)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyFacility.Delegators)

                          .AsNoTracking()
                          .Where(x => x.Id == litigationRequest.Id).ToList().FirstOrDefault();

                return ReturnLitigationRequestDto(litigationRequest);
            }
            else
                return saveResponse;
        }

        public async Task<BaseResponse> ValidateRealEstateBrokerageContract(LitigationRequestDto litigationRequestDto, Guid userId)
        {
            var contracts = ContractIntegration.GetContracts().Where(t => t.ContractKey == litigationRequestDto.key &&
                                t.LitigationRequestContractTypeId == LitigationRequestContractType.RealEstateBrokerage).ToList();
            if (!contracts.Any())
                return new BaseResponse() { IsDone = false, Message = "بيانات العقد غير صحيحة" };

            LitigationRequestContract litigationRequestContract = new LitigationRequestContract()
            {
                Id = Guid.NewGuid(),
                LitigationRequestContractTypeId = LitigationRequestContractType.RealEstateBrokerage,
                ContractKey = litigationRequestDto.key,
                VersionNumber = litigationRequestDto.SubKey
            };
            await ValidateContractParties(litigationRequestDto, userId, contracts, litigationRequestContract);

            LitigationRequest litigationRequest = new LitigationRequest()
            {
                Id = Guid.NewGuid(),
                ContractId = litigationRequestContract.Id,
                ApplicantCitizenId = userId,
                LitigationRequestOwnerTypeId = litigationRequestDto.LitigationRequestOwnerType,
                InternalStatusId = RequestStatus.SaveAsDraft,
                ExternalStatusId = RequestStatus.SaveAsDraft,
                Fees = 0,
                IsPaid = false,
                CreatedOn = DateTime.Now,
                IsSent = false,
                IsAllowEdit = true,
                IsActive = true,
                IsFinished = false,
                ContractAddress = "شارع القصر - الرياض"
            };
            await _repository.AddAsync<LitigationRequestContract, Guid>(litigationRequestContract);

            await _repository.AddAsync<LitigationRequest, Guid>(litigationRequest);

            var saveResponse = await _repository.SaveChangesAsync();
            if (saveResponse.IsDone)
            {
                litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.ApplicantCitizen.LawLicenseFile)
                          .Include(x => x.ApplicantCitizen.IdentityFile)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.FirstPartyCitizen.Nationality)
                          .Include(x => x.Contract.FirstPartyCitizen.FromDelegators)
                          .Include(x => x.Contract.SecondPartyCitizen)
                          .Include(x => x.Contract.SecondPartyCitizen.Nationality)

                          .Include(x => x.Contract.SecondPartyCitizen.FromDelegators)

                          .Include(x => x.Contract.FirstPartyFacility)
                          .Include(x => x.Contract.FirstPartyFacility.Delegators)
                          .Include(x => x.Contract.SecondPartyFacility)
                          .Include(x => x.Contract.SecondPartyFacility.Delegators)

                          .AsNoTracking()
                          .Where(x => x.Id == litigationRequest.Id).ToList().FirstOrDefault();

                return ReturnLitigationRequestDto(litigationRequest);
            }
            else
                return saveResponse;
        }

        public async Task<BaseResponse> ValidateOtherServicesContract(LitigationRequestDto litigationRequestDto, Guid userId)
        {
            throw new NotImplementedException();
        }



    }
}