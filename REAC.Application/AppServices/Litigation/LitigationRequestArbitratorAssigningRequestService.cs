using AutoMapper;
using Core.Contracts;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Integration;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.Arbitration;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.Litigation
{

    public class LitigationRequestArbitratorAssigningRequestService : BasicService<LitigationRequestArbitratorAssigningRequest, Guid>, ILitigationRequestArbitratorAssigningRequestService
    {
        private readonly REACDBContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;
        private readonly ISysSettingService _sysSetting;
        private readonly IMailService _emailService;
        private readonly ISMSService _smsService;
        public LitigationRequestArbitratorAssigningRequestService(REACDBContext context, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen, ISysSettingService sysSetting, IMailService emailService, ISMSService smsService) : base(repository, mapper, loggerManager, jwtReader)
        {
            _context = context;
            _mapper = mapper;
            _jwtReader = jwtReader;
            _citizen = citizen;
            _sysSetting = sysSetting;
            _emailService = emailService;
            _smsService = smsService;
        }
        public async Task<BaseResponse> AddLitigationRequestArbitratorAssigningRequestsResponse(Guid litigationRequestArbitratorAssigningRequestId, LitigationRequestArbitratorAssigningRequestStatus response, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequestArbitratorAssigningRequest = (await _repository.GetQueryable<LitigationRequestArbitratorAssigningRequest, Guid>())

                         .Where(x => x.Id == litigationRequestArbitratorAssigningRequestId).ToList().FirstOrDefault();

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestArbitratorNominationLists)
                        .Include(x => x.LitigationRequestArbitratorLists)
                        .Where(x => x.Id == litigationRequestArbitratorAssigningRequest.LitigationRequestId)
                        .SingleOrDefault();


            if (litigationRequestArbitratorAssigningRequest != null)
            {
                litigationRequestArbitratorAssigningRequest.ResponseOn = DateTime.Now;
                litigationRequestArbitratorAssigningRequest.Response = response;
            }

            if (litigationRequest != null)
            {
                var citizenInfo = (await _repository.GetQueryable<Citizen, Guid>())
                    .Where(c => c.Id == referenceId)
                    .Select(c => new { Email = c.Email, Mobile = c.Mobile })
                    .FirstOrDefault();
                var msg = "";

                switch (response)
                {
                    case LitigationRequestArbitratorAssigningRequestStatus.Approve:
                        switch (litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId)
                        {
                            case ArbitratorAssigningRequestType.Arbitrator:
                                if (litigationRequest.LitigationRequestArbitratorLists != null)
                                {
                                    if (litigationRequest.LitigationRequestArbitratorLists.Count(x => x.ArbitratorCitizenId == referenceId) == 0)
                                    {
                                        litigationRequest.LitigationRequestArbitratorLists.Add(new LitigationRequestArbitratorList()
                                        {
                                            Id = Guid.NewGuid(),
                                            ArbitratorCitizenId = referenceId,
                                            LitigationRequestId = litigationRequest.Id
                                        });
                                    }
                                }
                                else
                                {
                                    litigationRequest.LitigationRequestArbitratorLists.Add(new LitigationRequestArbitratorList()
                                    {
                                        Id = Guid.NewGuid(),
                                        ArbitratorCitizenId = referenceId,
                                        LitigationRequestId = litigationRequest.Id
                                    });
                                }
                                if (litigationRequest.ArbitratorsCount == 1)
                                {
                                    litigationRequest.ArbitratorCitizenId = referenceId;
                                    litigationRequest.LitigationRequestStage = LitigationRequestStage.Arbitration;
                                    litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitrationSession;

                                }
                                break;
                            case ArbitratorAssigningRequestType.ArbitratorsChairman:
                                litigationRequest.ArbitratorCitizenId = referenceId;
                                litigationRequest.LitigationRequestStage = LitigationRequestStage.Arbitration;
                                litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitrationSession;
                                litigationRequest.StartReconciliationDate = DateTime.Now;
                                break;
                        }

                        // Send Notification
                        // تم قبول طلب المهمة من المحكم
                        msg = _sysSetting.GetBySettingKey(SysSettingKeyConstants.Arbitration_Arbitraor_AcceptTask).Result.Data?.Value;
                        var emailResult = await _emailService.SendMail(new List<string>() { citizenInfo.Email }, "Arbitraor Accept Task", msg);
                        var smsResult = _smsService.SendSMS(new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                        {
                            message_text = msg,
                            recipients = new List<string>() { citizenInfo.Mobile },
                            notification_type = "SMS",

                        });
                        break;
                    case LitigationRequestArbitratorAssigningRequestStatus.Reject:
                        var arbitratorsList = litigationRequest.LitigationRequestArbitratorNominationLists.Where(x => x.IsSent != true).OrderBy(x => x.TotalPriority).ThenBy(x => x.DefendantPriority).ToList();
                        if (arbitratorsList != null)
                        {
                            if (arbitratorsList.Count > 0)
                            {

                                LitigationRequestArbitratorAssigningRequest nextLitigationRequestArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                                nextLitigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                                nextLitigationRequestArbitratorAssigningRequest.RequestOn = DateTime.Now;
                                nextLitigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.Arbitrator;
                                nextLitigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                                nextLitigationRequestArbitratorAssigningRequest.LitigationRequestArbitratorNominationListId = arbitratorsList.FirstOrDefault().Id;
                                nextLitigationRequestArbitratorAssigningRequest.CitizenId = arbitratorsList.FirstOrDefault().ArbitratorCitizenId;
                                nextLitigationRequestArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                                await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(nextLitigationRequestArbitratorAssigningRequest);
                                arbitratorsList.FirstOrDefault().IsSent = true;
                            }
                        }
                        // Send Notification
                        // تم رفض طلب المهمة من المحكم
                        break;
                }
            }
            var saveResponse = await _repository.SaveChangesAsync(cancellationToken);
            if (!saveResponse.IsDone)
                return saveResponse;

            litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestArbitratorLists)
                        .Where(x => x.Id == litigationRequestArbitratorAssigningRequest.LitigationRequestId)
                        .SingleOrDefault();

            switch (litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId)
            {
                case ArbitratorAssigningRequestType.Arbitrator:
                    if (litigationRequest.LitigationRequestArbitratorLists.Count == litigationRequest.ArbitratorsCount - 1)
                        litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitratorChairmanListNomination;
                    break;
            }

            saveResponse = await _repository.SaveChangesAsync(cancellationToken);
            if (!saveResponse.IsDone)
                return saveResponse;


            return await GetLitigationRequestArbitratorAssigningRequests(litigationRequestArbitratorAssigningRequest.LitigationRequestId, litigationRequestArbitratorAssigningRequestId, cancellationToken);
        }
        public async Task<BaseResponse> GetLitigationRequestArbitratorAssigningRequests(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestArbitratorAssigningRequests)
                         .Include(x => x.LitigationRequestExtendPeriodRequests)
                         .ThenInclude(x => x.LitigationRequestExtendPeriodRequestFiles)
                         .ThenInclude(x => x.FileManager)
                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)
                         .Where(x => x.LitigationRequestArbitratorAssigningRequests
                         .Where(t => t.CitizenId == referenceId).Count() > 0).OrderByDescending(x => x.CreatedOn).ToList();

            List<CustomLitigationRequestMircoArbitratorDto> litigationRequestMircoDtos = new List<CustomLitigationRequestMircoArbitratorDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    CustomLitigationRequestMircoArbitratorDto litigationRequestMircoDto = new CustomLitigationRequestMircoArbitratorDto();
                    litigationRequestMircoDto.Id = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault().Id;
                    litigationRequestMircoDto.RequestId = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.ArbitratorAssigningRequestType = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault().ArbitratorAssigningRequestTypeId;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.Response = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault()?.Response;
                    litigationRequestMircoDto.ResponseOn = litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault()?.ResponseOn;

                    bool allowAdd = true;

                    if (litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).Any())
                    {
                        if (litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Approve |
                            litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Cancelled |
                            litigationRequest.LitigationRequestArbitratorAssigningRequests.Where(x => x.CitizenId == referenceId).FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Reject)
                        {
                            allowAdd = false;
                        }
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.ExternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<List<CustomLitigationRequestMircoArbitratorDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };

            throw new NotImplementedException();
        }
        public async Task<BaseResponse> GetLitigationRequestArbitratorAssigningRequestsAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            PagedResult<LitigationRequestArbitratorAssigningRequest> litigationRequests = null;

            var entitiesQuerable = (await _repository.GetQueryable<LitigationRequestArbitratorAssigningRequest, Guid>())
                         .Include(x => x.ArbitratorCitizen)
                         .Include(x => x.LitigationRequest)
                         .Include(x => x.LitigationRequest.InternalStatus)
                         .Include(x => x.LitigationRequest.ExternalStatus)
                         .Include(x => x.LitigationRequest.LitigationRequestCategory)
                         .Include(x => x.LitigationRequest.Contract)
                         .Include(x => x.LitigationRequest.Contract.ContractFile)
                         .Include(x => x.LitigationRequest.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.LitigationRequest.Contract.FoundingLetterFile)
                         .Include(x => x.LitigationRequest.Contract.InternalRegulationsFile)
                         .Include(x => x.LitigationRequest.Contract.FirstPartyFacility)
                         .Include(x => x.LitigationRequest.Contract.FirstPartyCitizen)
                         .Include(x => x.LitigationRequest.Contract.SecondPartyFacility)
                         .Include(x => x.LitigationRequest.Contract.SecondPartyCitizen)
                         .Include(x => x.LitigationRequest.Contract.LitigationRequestContractType);


            litigationRequests = entitiesQuerable
                                       .OrderByDescending(x => x.LitigationRequest.CreatedOn)
                                       .AsSplitQuery()
                                       .AsNoTracking()
                                       .GetPaged(pageIndex, pageSize);



            List<CustomLitigationRequestMircoArbitratorAllDto> litigationRequestMircoDtos = new List<CustomLitigationRequestMircoArbitratorAllDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests.Results)
                {
                    CustomLitigationRequestMircoArbitratorAllDto litigationRequestMircoDto = new CustomLitigationRequestMircoArbitratorAllDto();
                    litigationRequestMircoDto.Id = litigationRequest.Id;
                    litigationRequestMircoDto.RequestId = litigationRequest.LitigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.LitigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.LitigationRequest.RequestCode;
                    litigationRequestMircoDto.ArbitratorAssigningRequestType = litigationRequest.ArbitratorAssigningRequestTypeId;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.LitigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.LitigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.LitigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.LitigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.LitigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.LitigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.LitigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.RequestTypeId = litigationRequest.LitigationRequest.Contract.LitigationRequestContractType.Id.GetHashCode();
                    litigationRequestMircoDto.Response = litigationRequest.Response;
                    litigationRequestMircoDto.ResponseOn = litigationRequest.ResponseOn;
                    litigationRequestMircoDto.ArbitratorName =
                        (litigationRequest.ArbitratorAssigningRequestTypeId ==
                        ArbitratorAssigningRequestType.ArbitratorsChairman) ? litigationRequest.ArbitratorCitizen?.FullName :
                        litigationRequest.ArbitratorCitizen?.FullName;

                    bool allowAdd = true;


                    if (litigationRequest.Response == LitigationRequestArbitratorAssigningRequestStatus.Approve |
                        litigationRequest.Response == LitigationRequestArbitratorAssigningRequestStatus.Cancelled |
                        litigationRequest.Response == LitigationRequestArbitratorAssigningRequestStatus.Reject)
                    {
                        allowAdd = false;
                    }


                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.LitigationRequest.ExternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<PagedResult<CustomLitigationRequestMircoArbitratorAllDto>>
            {
                Data =
                              new PagedResult<CustomLitigationRequestMircoArbitratorAllDto>
                              {
                                  Results = litigationRequestMircoDtos,
                                  CurrentPage = litigationRequests.CurrentPage,
                                  PageCount = litigationRequests.PageCount,
                                  PageSize = litigationRequests.PageSize,
                                  TotalCount = litigationRequests.TotalCount
                              },
                IsDone = true
            };
        }
        public async Task<BaseResponse> GetLitigationRequestArbitratorAssigningRequests(Guid requestId, Guid LitigationRequestArbitratorAssigningRequestId, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestArbitratorAssigningRequests)
                         .Include(x => x.ApplicantCitizen)
                         .Include(x => x.InternalStatus)
                         .Include(x => x.ExternalStatus)
                         .Include(x => x.LitigationRequestAmount)
                         .Include(x => x.LitigationRequestCategory)
                         .Include(x => x.SysExemptionReductionCategory)
                         .Include(x => x.LitigationRequestOwnerType)
                         .Include(x => x.Contract)
                         .Include(x => x.Contract.ContractFile)
                         .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                         .Include(x => x.Contract.FoundingLetterFile)
                         .Include(x => x.Contract.InternalRegulationsFile)
                         .Include(x => x.Contract.FirstPartyFacility)
                         .Include(x => x.Contract.FirstPartyCitizen)
                         .Include(x => x.Contract.SecondPartyFacility)
                         .Include(x => x.Contract.SecondPartyCitizen)
                         .Include(x => x.Contract.LitigationRequestContractType)
                         .Where(x => x.LitigationRequestArbitratorAssigningRequests
                         .Where(t => t.Id == LitigationRequestArbitratorAssigningRequestId).Count() > 0).OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();

            if (litigationRequest != null)
            {
                CustomLitigationRequestMircoArbitratorDto litigationRequestMircoDto = new CustomLitigationRequestMircoArbitratorDto();

                litigationRequestMircoDto.Id = litigationRequest.Id;
                litigationRequestMircoDto.RequestId = requestId;
                litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                litigationRequestMircoDto.ArbitratorAssigningRequestType = litigationRequest?.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.ArbitratorAssigningRequestTypeId;
                litigationRequestMircoDto.RequestCode = litigationRequest?.RequestCode;
                litigationRequestMircoDto.FirstPartyName = (litigationRequest?.Contract.FirstPartyCitizen == null) ? litigationRequest?.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                litigationRequestMircoDto.SecondPartyName = (litigationRequest?.Contract.SecondPartyCitizen == null) ? litigationRequest?.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                litigationRequestMircoDto.RequestTypeName = litigationRequest?.Contract.LitigationRequestContractType.TypeName;
                litigationRequestMircoDto.Response = litigationRequest.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.Response;
                litigationRequestMircoDto.ResponseOn = litigationRequest.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.ResponseOn;

                bool allowAdd = true;

                if (litigationRequest.LitigationRequestArbitratorAssigningRequests.Any())
                {
                    if (litigationRequest.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Approve |
                        litigationRequest.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Cancelled |
                        litigationRequest.LitigationRequestArbitratorAssigningRequests.FirstOrDefault()?.Response == LitigationRequestArbitratorAssigningRequestStatus.Reject)
                    {
                        allowAdd = false;
                    }
                }

                if (allowAdd)
                    litigationRequestMircoDto.IsDone = "--";
                else
                    litigationRequestMircoDto.IsDone = "تم";

                litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;

                return new DataResponse<CustomLitigationRequestMircoArbitratorDto>()
                {
                    Data = litigationRequestMircoDto,
                    IsDone = true
                };

            }
            throw new NotImplementedException();
        }
    }
}