using AutoMapper;
using Core.Dtos;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.BaseEntities;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Schema;
using REAC.Application.AppServices.Integration;
using REAC.Application.Contracts.Dtos.Integration.Properties;
using REAC.Application.Contracts.Dtos.Integration.Rent;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Get;
using REAC.Application.Contracts.Dtos.Litigation.List;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Application.Contracts.IAppServices.Wpf.System;
using REAC.Application.Contracts.IModuleServices;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.Domain.Shared.Helpers;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static REAC.Application.AppServices.Litigation.LitigationRequestService;
using static REAC.Application.Contracts.IAppServices.Litigation.ILitigationRequestService;
using static System.Runtime.InteropServices.JavaScript.JSType;



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
        
        public LitigationRequestService(REACDBContext context, ILitigationRequestGroundFileService groundFileService, IServiceProvider service,
            ILitigationRequestGroundService groundService, ILitigationRequestRequestFileService requestFileService, ILitigationRequestRequestService requestsService,
            ISysFileManagerService fileService, ISysLitigationRequestAmountService amountService, IUserProviderService contractService, IEntityRepository repository, 
            IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader) : base(repository, mapper, loggerManager, jwtReader)

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
        }

        public async Task<BaseResponse> AddLitigation(LitigationCreateUpdateDto model)
        { 
            return await Add(model);
        }
        public async Task<BaseResponse> Add(LitigationCreateUpdateDto model)
        {
            var litigations = await GetQueryableAsync();
            BaseResponse? addedLitigation = null;
            Guid litigationId = Guid.Empty;
            if (model.ExemptionReductionCategory.IsExemptionReductionCategory == true)
            {
                #region  Exempt Categories 
                string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ExemptionReductionCategory.AttachmentId.ToString());
                await FileHelper.Move(model.ExemptionReductionCategory.AttachmentId, rootFolder);
                #endregion
            }
            else
            {
                model.ExemptionReductionCategory.Id = new Guid("d4b21df4-bd49-4e3b-b634-2a714e4ebcc3");
            }
            if (model.IsSent && model.Id ==Guid.Empty)
            {         
                model.ExternalStatusId = LitigationRequestStatus.New;
                model.InternalStatusId = LitigationRequestStatus.New;
             
                //    var countt = GetLitigationsCountByType(model.LitigationRequestContractType);
                // var count = _contractService.GetContractCountByContractType(model.LitigationRequestContractType);
                var count = litigations.Where(x => (int)x.Contract.LitigationRequestContractTypeId == (int)model.LitigationRequestContractType).Count();
                var requestCode = RequestCode.GenerateRequestCode(model.LitigationRequestContractType, count);
                model.RequestCode = requestCode;

            }
            else if(model.Id == Guid.Empty)
            {
                model.ExternalStatusId = LitigationRequestStatus.SaveAsDraft;
                model.InternalStatusId = LitigationRequestStatus.SaveAsDraft;
               // model.SentOn = null;
            }
        
            if (model.LitigationRequestContractType == LitigationRequestContractType.OtherServices)
            {
                #region Litigation Entity

                // Save Litigation entity first
                var litigationRequestAmount = await _amountService.GetLitigationRequestAmount(model.LitigationRequestAmountId);
                model.Fees = AdministrativeFees.GetFees(int.Parse(litigationRequestAmount.Split("-")[0]), int.Parse(litigationRequestAmount.Split("-")[1]));
                //      model.LitigationRequestOwnerTypeId = _contractService.IntegrationVerification("1234abcd").Result.OwnerType;
                if (model.Id == Guid.Empty)
                {
                    addedLitigation = await base.AddAsync(model, default);
                    litigationId = await GetAddedLitigation();
                }
                else
                {
                    addedLitigation = await base.UpdateAsync(model);
                    litigationId = model.Id;
                }


                if (!addedLitigation.IsDone)
                {
                    return addedLitigation;
                }
                #endregion

                #region Requests
                foreach (var item in model.Requests)
                {
                    await _requestsService.AddRequests(new LitigationRequestRequestCreateUpdateDto { LitigationRequestId = litigationId, RequestContent = item.LitigationPurpose });
                    // there is no files in requests await _requestFileService.AddRequestFiles(new LitigationRequestRequestFileCreateUpdateDto { LitigationRequestRequestId = await _requestsService.GetAddedRequestId(), FileManagerId = item.LitigationPurposeFileId });
                }
                #endregion

                #region References Attachments

                string referencerootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractId.ToString(), "References");
                List<Guid> referencenames = new List<Guid>();
                foreach (var item in model.References)
                {
                    foreach (var Attachment in item.ReferenceAttachments)
                    {
                        referencenames.Add(Attachment);
                        await _groundService.AddReference(new LitigationRequestGroundCreateUpdateDto { LitigationRequestId = litigationId, Title = item.ReferenceTitle });
                        await _groundFileService.AddReferenceFiles(new LitigationRequestGroundFileCreateUpdateDto { LitigationRequestGroundId = await _groundService.GetAddedReferenceId(), FileManagerId = Attachment });
                    }

                }
                await FileHelper.Move(referencenames, referencerootFolder);

                #endregion

                if (model.ExemptionReductionCategory.IsExemptionReductionCategory == true)
                {
                    #region  Exempt Categories 
                    string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ExemptionReductionCategory.AttachmentId.ToString());
                    await FileHelper.Move(model.ExemptionReductionCategory.AttachmentId, rootFolder);
                    #endregion
                }
                if (model.IsFinancialLitigation == true && model.LitigationRequestAmountId == null)
                {
                    //Must Insert amount
                    throw new Exception();
                }


                if (model.IsSent)
                {
                    var _bpfService = _serviceProvider.GetRequiredService<IBpfService>();
                    var _stepService = _serviceProvider.GetRequiredService<IWorkFlowStepService>();
                    await _bpfService.ExcuteWorkFlow(litigationId, Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"), default);
                    Guid employee = await _stepService.GetLowestEmployee();
                    await _bpfService.Assign(new Contracts.Dtos.Wpf.Business.AssignRequestPayload
                    {
                        DestinationEmployeeId = employee,
                        SourceEmployeeId = Guid.Parse("B9BBE3E5-0CA9-44AF-95FA-D2C3A0EC2280"),
                        RequestId = litigationId

                    }, default);
                }

                return addedLitigation;
            }
            else
            {
                var errors = ValidateAsync(model);
                //if (errors.Count != 0)
                //    throw new ExceptionResponse(errors);

                #region Litigation Entity

                // Save Litigation entity first
             
                var litigationRequestAmount = await _amountService.GetLitigationRequestAmount(model.LitigationRequestAmountId);
                model.Fees = AdministrativeFees.GetFees(int.Parse(litigationRequestAmount.Split("-")[0]), int.Parse(litigationRequestAmount.Split("-")[1]));
                //  model.LitigationRequestOwnerTypeId = _contractService.RentIntegrate().Result.OwnerType; front will send it and then map successfully
                if (model.Id == Guid.Empty)
                {
                    addedLitigation = await base.AddAsync(model, default);

                    litigationId = await GetAddedLitigation();
                    #region Requests
                    foreach (var item in model.Requests)
                    {
                        await _requestsService.AddRequests(new LitigationRequestRequestCreateUpdateDto { LitigationRequestId = litigationId, RequestContent = item.LitigationPurpose });
                        // there is no files in requests await _requestFileService.AddRequestFiles(new LitigationRequestRequestFileCreateUpdateDto { LitigationRequestRequestId = await _requestsService.GetAddedRequestId(), FileManagerId = item.LitigationPurposeFileId });
                    }
                    #endregion

                    #region References Attachments

                    string referencerootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractId.ToString(), "References");
                    if (!Directory.Exists(referencerootFolder)) { Directory.CreateDirectory(referencerootFolder); }
                    List<Guid> referencenames = new List<Guid>();
                    foreach (var item in model.References)
                    {
                        foreach (var Attachment in item.ReferenceAttachments)
                        {
                            referencenames.Add(Attachment);
                            await _groundService.AddReference(new LitigationRequestGroundCreateUpdateDto { LitigationRequestId = litigationId, Title = item.ReferenceTitle });
                            await _groundFileService.AddReferenceFiles(new LitigationRequestGroundFileCreateUpdateDto { LitigationRequestGroundId = await _groundService.GetAddedReferenceId(), FileManagerId = Attachment });
                        }

                    }
                    await FileHelper.Move(referencenames, referencerootFolder);

                    #endregion

                    #region  Exempt Categories 
                    string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Attachments", "Rent", model.ContractId.ToString(), model.ExemptionReductionCategory.Id.ToString());
                    if (!Directory.Exists(rootFolder)) { Directory.CreateDirectory(rootFolder); }
                    await FileHelper.Move(model.ExemptionReductionCategory.AttachmentId, rootFolder);
                    #endregion

                    if (model.IsSent)
                    {
                        try
                        {
                            var _bpfService = _serviceProvider.GetRequiredService<IBpfService>();                          
                            var _stepService = _serviceProvider.GetRequiredService<IWorkFlowStepService>();
                            await _bpfService.ExcuteWorkFlow(model.Id, Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"), default);
                            Guid employee = await _stepService.GetLowestEmployee();
                            await _bpfService.Assign(new Contracts.Dtos.Wpf.Business.AssignRequestPayload
                            {
                                DestinationEmployeeId = employee,
                                SourceEmployeeId = Guid.Parse("B9BBE3E5-0CA9-44AF-95FA-D2C3A0EC2280"),
                                RequestId = model.Id

                            }, default);
                        }
                        catch (Exception ex)
                        {
                            _loggerManager.Log(Core.Log.Domain.Shared.Enums.LoggerType.File,
                            new Core.Log.Application.Contracts.Dto.LogDto
                            { Message = ex.Message, Id = new Guid() });                       
                         }

                    }
                }
                else
                {
                    addedLitigation = await base.UpdateAsync(model);
                    litigationId = model.Id;
                }

               

                if (!addedLitigation.IsDone)
                {
                    return addedLitigation;
                }
                #endregion

                return addedLitigation;
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
            var list = _mapper.Map<List<AuditingListDto>>(litigations).Where(x => x.InternalStatusId != LitigationRequestStatus.SaveAsDraft).ToList();
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
            var result = quarble.Include(x => x.Contract).Include(x => x.LitigationRequestCategory).Include(x => x.Area).Include(x => x.City).Include(x => x.ApplicantCitizen).Include(x => x.Contract.SecondPartyCitizen).Include(x => x.Contract.FirstPartyCitizen).Include(x=>x.LitigationRequestGrounds).Include(x=>x.LitigationRequestRequests).FirstOrDefault(x => x.Id == litigationId);
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
                .Include(x => x.Contract.FirstPartyCitizen)
                .FirstOrDefault(x => x.Id == litigationId);
            var mapped = _mapper.Map<LitigationPartiesDetailsDto>(result);
            return new DataResponse<LitigationPartiesDetailsDto> { Data = mapped, IsDone = true };
        }

        public async Task<BaseResponse> GetLitigationDetails(Guid litigationId)
        {
            var quarble = await GetQueryableAsync();
            var result = quarble.Include(x => x.Contract)
                .Include(x => x.LitigationRequestCategory)
                .Include(x => x.Area)
                .Include(x => x.City)
                .Include(x => x.ApplicantCitizen)
                .Include(x => x.Contract.SecondPartyCitizen)
                .Include(x => x.Contract.FirstPartyCitizen)
                .FirstOrDefault(x => x.Id == litigationId);
            var mapped = _mapper.Map<LitigationDetailsDto>(result);
            return new DataResponse<LitigationDetailsDto> { Data = mapped, IsDone = true };
        }

    }
}