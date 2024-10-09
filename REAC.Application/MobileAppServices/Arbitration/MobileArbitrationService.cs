using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Shared.Responses;
using Microsoft.AspNetCore.SignalR;
using REAC.Application.Contracts.Dtos.Reconciliation.Process;
using REAC.Application.Contracts.IAppServices.Notification;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IMobileAppServices.Arbitration;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafi.Application.Hubs.Contracts;
using Wafi.Application.Hubs;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Arbitration;
using REAC.Application.Contracts.Dtos.Notification;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using REAC.Application.Contracts.Dtos.Citizen;
using REAC.EntityFrameworkCore.Entities.Actor;
using REAC.EntityFrameworkCore.Entities.Arbitration;
using System.Threading;

namespace REAC.Application.MobileAppServices.Arbitration
{
    public class MobileArbitrationService : IMobileArbitrationService
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepository _repository;
        private readonly IJwtReader _jwtReader;
        private readonly ISysSettingService _settingService;
        protected readonly INotificationService _notificationService;
        private readonly ISysEmployeeService _sysEmployeeService;
        protected readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;
        public MobileArbitrationService(IEntityRepository repository, ISysSettingService settingService,
                          IWorkFlowProcessingLogService workFlowProcessingLogService,
                          IMapper mapper,
                          IJwtReader jwtReader, ISysEmployeeService sysEmployeeService, INotificationService notificationService)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtReader = jwtReader;
            _settingService = settingService;
            _sysEmployeeService = sysEmployeeService;
            _notificationService = notificationService;

        }

        #region Count
        public async Task<BaseResponse> GetLitigationRequestForAddArbitratorsCount(Guid requestId)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).AsNoTracking()
                         .Include(x => x.Area)
                         .Include(x => x.City)
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

                         .Where(x => x.Id == requestId).ToList().FirstOrDefault();

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);


            bool isDone = false;
            bool allowAdd = true;

            if (litigationRequest is null)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                {
                    allowAdd = false;
                }
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorCount)
                        allowAdd = false;
                }
            }
            if (litigationRequest.ArbitratorsCount.HasValue)
            {
                allowAdd = false;
            }
            return new DataResponse<LitigationRequestForAllArbitratorCountDto>()
            {

                Data = new LitigationRequestForAllArbitratorCountDto()
                {
                    AllowAdd = allowAdd,
                    LitigationRequestShortDto = request,
                    ArbitratorsCount = litigationRequest?.ArbitratorsCount,
                    FirstPartyArbitratorsCount = litigationRequest?.FirstPartyArbitratorsCount,
                    SecondPartyArbitratorsCount = litigationRequest?.SecondPartyArbitratorsCount,
                },
                IsDone = true,
            };

        }

        public async Task<BaseResponse> AddArbitratorsCount(ArbitratorCountRequestPayload arbitratorCountRequestPayload)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                         .Include(x => x.ApplicantCitizen)
                        .Include(x => x.Contract)
                        .ThenInclude(x => x.FirstPartyCitizen)
                        .Include(x => x.Contract)
                        .ThenInclude(x => x.SecondPartyCitizen)
                        .Include(x => x.Contract).ThenInclude(x => x.FirstPartyFacility).ThenInclude(x => x.Citizen)
                        .Include(x => x.Contract).ThenInclude(x => x.SecondPartyFacility).ThenInclude(x => x.Citizen)
                        .FirstOrDefault(x => x.Id == arbitratorCountRequestPayload.RequestId);

                if (litigationRequest is null)
                    return new BaseResponse() { IsDone = false, Message = "لا يوجد دعوي" };
                bool isDone = false;
                if (litigationRequest != null)
                {
                    if (litigationRequest.ApplicantCitizen != null)
                    {
                        if (litigationRequest.ApplicantCitizenId == referenceId)
                        {
                            if (!litigationRequest.FirstPartyArbitratorsCount.HasValue)
                            {
                                litigationRequest.FirstPartyArbitratorsCount = arbitratorCountRequestPayload.ArbitratorCount;
                                isDone = true;
                                await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                {
                                    Id = Guid.NewGuid(),
                                    CreatedOn = DateTime.Now,
                                    RequestId = litigationRequest.Id,
                                    ShowForApplicant = false,
                                    LogMessage = "تم تحديد عدد هيئة التحكيم من قبل المدعى " + arbitratorCountRequestPayload.ArbitratorCount + " محكم"
                                });
                            }
                        }
                    }

                    if (!isDone)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizen != null)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            {
                                if (!litigationRequest.FirstPartyArbitratorsCount.HasValue)
                                {
                                    litigationRequest.FirstPartyArbitratorsCount = arbitratorCountRequestPayload.ArbitratorCount;
                                    isDone = true;
                                    await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                    {
                                        Id = Guid.NewGuid(),
                                        CreatedOn = DateTime.Now,
                                        RequestId = litigationRequest.Id,
                                        ShowForApplicant = false,
                                        LogMessage = "تم تحديد عدد هيئة التحكيم من قبل المدعى " + arbitratorCountRequestPayload.ArbitratorCount + " محكم"
                                    });
                                }
                            }
                        }
                    }

                    if (!isDone)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizen != null)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            {
                                if (!litigationRequest.SecondPartyArbitratorsCount.HasValue)
                                {
                                    litigationRequest.SecondPartyArbitratorsCount = arbitratorCountRequestPayload.ArbitratorCount;
                                    isDone = true;
                                    await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                    {
                                        Id = Guid.NewGuid(),
                                        CreatedOn = DateTime.Now,
                                        RequestId = litigationRequest.Id,
                                        ShowForApplicant = false,
                                        LogMessage = "تم تحديد عدد هيئة التحكيم من قبل المدعى عليه " + arbitratorCountRequestPayload.ArbitratorCount + " محكم"
                                    });
                                }
                            }
                        }
                    }

                    if (!isDone)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility != null)
                        {
                            if (litigationRequest.Contract.FirstPartyFacility.CitizenId == referenceId)
                            {
                                if (!litigationRequest.FirstPartyArbitratorsCount.HasValue)
                                {
                                    litigationRequest.FirstPartyArbitratorsCount = arbitratorCountRequestPayload.ArbitratorCount;
                                    isDone = true;
                                    await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                    {
                                        Id = Guid.NewGuid(),
                                        CreatedOn = DateTime.Now,
                                        RequestId = litigationRequest.Id,
                                        ShowForApplicant = false,
                                        LogMessage = "تم تحديد عدد هيئة التحكيم من قبل المدعى " + arbitratorCountRequestPayload.ArbitratorCount + " محكم"
                                    });
                                }
                            }
                        }
                    }

                    if (!isDone)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility != null)
                        {
                            if (litigationRequest.Contract.SecondPartyFacility.CitizenId == referenceId)
                            {
                                if (!litigationRequest.SecondPartyArbitratorsCount.HasValue)
                                {
                                    litigationRequest.SecondPartyArbitratorsCount = arbitratorCountRequestPayload.ArbitratorCount;
                                    isDone = true;
                                    await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                                    {
                                        Id = Guid.NewGuid(),
                                        CreatedOn = DateTime.Now,
                                        RequestId = litigationRequest.Id,
                                        ShowForApplicant = false,
                                        LogMessage = "تم تحديد عدد هيئة التحكيم من قبل المدعى عليه " + arbitratorCountRequestPayload.ArbitratorCount + " محكم"
                                    });
                                }
                            }
                        }
                    }
                }

                if (litigationRequest.FirstPartyArbitratorsCount.HasValue & litigationRequest.SecondPartyArbitratorsCount.HasValue)
                {

                    if (litigationRequest.FirstPartyArbitratorsCount.Value == litigationRequest.SecondPartyArbitratorsCount.Value)
                    {
                        litigationRequest.ArbitratorsCount = litigationRequest.FirstPartyArbitratorsCount;
                        litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.LitigationRequestAddArbitratorListNomination;
                        litigationRequest.ArbitratorsCountDateTime = DateTime.Now;

                        // Send Notification
                        // تم الإنتهاء من تحديد عدد المحكمين النهائي لاتفاق كلا المدعى والمدعى عليه على عدد المحكمين
                        try
                        {
                            var arbitrationAdmin = await _sysEmployeeService.GetAdminsByDepartmentType(DepartmentType.Arbitration);
                            var msg = _settingService.GetBySettingKey(SysSettingKeyConstants.Arbitrators_Count_FromBothSides).Result.Data?.Value;
                            msg = msg?.Replace("{LitigationCode}", litigationRequest.RequestCode);
                            //if (litigationRequest?.Contract?.FirstPartyCitizen != null)
                            //    msg = msg?.Replace("{FirstParty}", litigationRequest?.Contract?.FirstPartyCitizen.FullName);
                            //if (litigationRequest?.Contract?.SecondPartyCitizen != null)
                            //    msg = msg?.Replace("{SecondParty}", litigationRequest?.Contract?.SecondPartyCitizen.FullName);
                            //if (litigationRequest?.Contract?.FirstPartyFacility != null)
                            //    msg = msg?.Replace("{FirstParty}", litigationRequest?.Contract?.FirstPartyFacility.Citizen.FullName);
                            //if (litigationRequest?.Contract?.SecondPartyFacility != null)
                            //    msg = msg?.Replace("{SecondParty}", litigationRequest?.Contract?.SecondPartyFacility.Citizen.FullName);


                            if (arbitrationAdmin.IsDone)
                            {
                                List<SysNotificationDto> notifications = new List<SysNotificationDto>() {
                           new SysNotificationDto{
                        ActorId = (Guid)arbitrationAdmin.Data?.Id,
                        RequestId=litigationRequest.Id,
                        CreatedOn= DateTime.Now,
                        RequestTypeId=RequestType.LitigationRequest,
                        ActorTypeId=NotificationActorType.Employee,
                        Message=msg
                        }

                        };
                                List<string> emails = new List<string>() {
                                  arbitrationAdmin.Data?.Email
                                };
                                List<string> recipients = new List<string>() {
                                  arbitrationAdmin.Data?.Mobile
                                };

                                await _notificationService.SendNotifications(new Contracts.Dtos.Notification.SendEmailDto()
                                {
                                    To = emails,
                                    Subject = "الإنتهاء من تحديد عدد المحكمين",
                                    Body = msg
                                }, new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                                {
                                    message_text = msg,
                                    recipients = recipients,
                                    notification_type = "SMS",

                                }, notifications);

                            }
                        }
                        catch (Exception e)
                        {


                        }



                    }
                    else
                    {
                        // Send Notification
                        // تم الإنتهاء من تحديد عدد المحكمين من قبل المدعى والمدعى عليه
                        try
                        {
                            var arbitrationAdmin = await _sysEmployeeService.GetAdminsByDepartmentType(DepartmentType.Arbitration);
                            var msg = _settingService.GetBySettingKey(SysSettingKeyConstants.Arbitrators_Count_Added_AfterAgreement).Result.Data?.Value;
                            msg = msg?.Replace("{LitigationCode}", litigationRequest.RequestCode);

                            if (arbitrationAdmin.IsDone)
                            {
                                List<SysNotificationDto> notifications = new List<SysNotificationDto>() {
                           new SysNotificationDto{
                        ActorId = (Guid)arbitrationAdmin.Data?.Id,
                        RequestId=litigationRequest.Id,
                        CreatedOn= DateTime.Now,
                        RequestTypeId=RequestType.LitigationRequest,
                        ActorTypeId=NotificationActorType.Employee,
                        Message=msg
                        }

                        };
                                List<string> emails = new List<string>() {
                                  arbitrationAdmin.Data?.Email
                                };
                                List<string> recipients = new List<string>() {
                                  arbitrationAdmin.Data?.Mobile
                                };

                                await _notificationService.SendNotifications(new Contracts.Dtos.Notification.SendEmailDto()
                                {
                                    To = emails,
                                    Subject = "الإنتهاء من تحديد عدد المحكمين",
                                    Body = msg
                                }, new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                                {
                                    message_text = msg,
                                    recipients = recipients,
                                    notification_type = "SMS",

                                }, notifications);

                            }
                        }
                        catch (Exception e)
                        {


                        }

                    }
                }

                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;

            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestForAddArbitratorsCount(arbitratorCountRequestPayload.RequestId);
        }
        #endregion



        #region Nomination List
        public async Task<BaseResponse> GetCitizenLitigationRequestForArbitratorsNominationListReply(Guid requestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                            .Include(x => x.ArbitratorCitizen)
                            .Include(x => x.InternalStatus)
                            .Include(x => x.ExternalStatus)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorAssigningRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.ArbitratorLitigationRequests)
                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.CitizenRequests)
                            .ThenInclude(x => x.SysArbitratorClass)

                            .Include(x => x.LitigationRequestArbitratorNominationLists)
                            .ThenInclude(x => x.ArbitratorCitizen)
                            .ThenInclude(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.LitigationRequestArbitratorLists)
                            .Include(x => x.Contract)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Include(x => x.Contract.FirstPartyFacility)
                            .Include(x => x.Contract.FirstPartyCitizen)
                            .Include(x => x.Contract.SecondPartyFacility)
                            .Include(x => x.Contract.SecondPartyCitizen)
                            .Include(x => x.Contract.LitigationRequestContractType)
                            .Where(x => x.Id == requestId).AsNoTracking().ToList().FirstOrDefault();


            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            bool isDone = false;
            bool allowAdd = true;

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }
            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                {
                    allowAdd = false;
                }
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.LitigationRequestAddArbitratorListNominationDone)
                        allowAdd = false;
                    else
                    {
                        if (litigationRequest.ApplicantCitizenId == referenceId)
                        {
                            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                allowAdd = false;
                        }
                        if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            {
                                if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                        if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                            {
                                if (litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
                                    allowAdd = false;
                            }
                        }
                    }
                }

            }

            LitigationRequestForAllArbitratorNominationListWithPartiesValueDto litigationRequestForAllArbitratorNominationListDto = new LitigationRequestForAllArbitratorNominationListWithPartiesValueDto();
            litigationRequestForAllArbitratorNominationListDto.LitigationRequestShortDto = request;
            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos = new List<CitizenArbitratorShortStatusWithValueDto>();

            if (litigationRequest.LitigationRequestArbitratorNominationLists.Any())
            {
                foreach (var litigationRequestArbitratorNominationList in litigationRequest.LitigationRequestArbitratorNominationLists)
                {
                    if (allowAdd)
                    {
                        CitizenArbitratorShortStatusWithValueDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusWithValueDto();
                        citizenArbitratorShortStatusDto.Id = litigationRequestArbitratorNominationList.Id;
                        citizenArbitratorShortStatusDto.CitizenId = litigationRequestArbitratorNominationList.ArbitratorCitizenId;
                        citizenArbitratorShortStatusDto.FullName = litigationRequestArbitratorNominationList.ArbitratorCitizen.FullName;

                        citizenArbitratorShortStatusDto.Brief =
                            litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                            .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                            .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                        if (litigationRequest.ApplicantCitizenId == referenceId)
                            citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;


                        if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;
                        }
                        if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                        }

                        if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;

                        }
                        if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                        {
                            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                        }

                        litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
                    }
                    else
                    {
                        if (litigationRequest.LitigationRequestArbitratorLists.Count(x => x.ArbitratorCitizenId == litigationRequestArbitratorNominationList.ArbitratorCitizenId) > 0)
                        {
                            CitizenArbitratorShortStatusWithValueDto citizenArbitratorShortStatusDto = new CitizenArbitratorShortStatusWithValueDto();
                            citizenArbitratorShortStatusDto.Id = litigationRequestArbitratorNominationList.Id;
                            citizenArbitratorShortStatusDto.CitizenId = litigationRequestArbitratorNominationList.ArbitratorCitizenId;
                            citizenArbitratorShortStatusDto.FullName = litigationRequestArbitratorNominationList.ArbitratorCitizen.FullName;

                            citizenArbitratorShortStatusDto.Brief =
                                litigationRequestArbitratorNominationList.ArbitratorCitizen?.CitizenRequests?
                                .Where(x => x.CitizenRequestTypeId == CitizenRequestType.Arbitrator & x.StatusId == CitizenRequestStatus.Certified)?
                                .OrderBy(x => x.RequestOn).LastOrDefault()?.Brief;

                            if (litigationRequest.ApplicantCitizenId == referenceId)
                                citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;


                            if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                            {
                                if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;
                            }
                            if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                            {
                                if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                            }

                            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                            {
                                if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.PlaintiffPriority;

                            }
                            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                            {
                                if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                                    citizenArbitratorShortStatusDto.Value = litigationRequestArbitratorNominationList.DefendantPriority;
                            }

                            litigationRequestForAllArbitratorNominationListDto.CitizenArbitratorShortStatusDtos.Add(citizenArbitratorShortStatusDto);
                        }
                    }
                }
                litigationRequestForAllArbitratorNominationListDto.AllowAdd = allowAdd;
                return new DataResponse<LitigationRequestForAllArbitratorNominationListWithPartiesValueDto>()
                {
                    Data = litigationRequestForAllArbitratorNominationListDto,
                    IsDone = true,
                };
            }

            return new BaseResponse()
            {
                IsDone = false,
                Message = "خطأ ما يرجى الرجوع لمدير النظام"
            };

        }

        public async Task<BaseResponse> AddCitizenLitigationRequestForReportArbitratorsNominationListReply(ArbitratorNominationListRequestReplyPayload arbitratorNominationListRequestReplyPayload)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>()).Include(x => x.Area).Include(x => x.City)
                          .Include(x => x.LitigationRequestArbitratorNominationLists).ThenInclude(x => x.ArbitratorCitizen)
                          .Include(x => x.ArbitratorCitizen)
                          .Include(x => x.ApplicantCitizen)
                          .Include(x => x.Contract)
                          .Include(x => x.Contract.FirstPartyFacility).ThenInclude(x => x.Citizen)
                          .Include(x => x.Contract.FirstPartyCitizen)
                          .Include(x => x.Contract.SecondPartyFacility).ThenInclude(x => x.Citizen)
                          .Include(x => x.Contract.SecondPartyCitizen)
                           .Include(x => x.LitigationRequestArbitratorLists)
                          .FirstOrDefault(x => x.Id == arbitratorNominationListRequestReplyPayload.RequestId);

            bool isDone = false;
            LitigationRequestParty litigationRequestParty = LitigationRequestParty.None;
            if (litigationRequest != null)
            {
                if (litigationRequest.ApplicantCitizen != null)
                {
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                        litigationRequestParty = LitigationRequestParty.Plaintiff;
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyCitizen != null)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Defendant;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.FirstPartyFacility != null)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility.CitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                }

                if (!isDone)
                {
                    if (litigationRequest.Contract.SecondPartyFacility != null)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility.CitizenId == referenceId)
                            litigationRequestParty = LitigationRequestParty.Defendant;
                    }
                }
            }

            if (litigationRequest?.LitigationRequestArbitratorNominationLists != null)
            {
                switch (litigationRequestParty)
                {
                    case LitigationRequestParty.Plaintiff:
                        await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            RequestId = litigationRequest.Id,
                            ShowForApplicant = false,
                            LogMessage = "تم إضافة أولوية إختيار قائمة تعيين هيئة التحكيم من قبل المدعى"
                        });
                        litigationRequest.IsPlaintiffArbitratorNominationListDone = true;
                        break;
                    case LitigationRequestParty.Defendant:
                        await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            RequestId = litigationRequest.Id,
                            ShowForApplicant = false,
                            LogMessage = "تم إضافة أولوية إختيار قائمة تعيين هيئة التحكيم من قبل المدعى عليه"
                        });
                        litigationRequest.IsDefendantArbitratorNominationListDone = true;
                        break;
                }

                foreach (var item in litigationRequest?.LitigationRequestArbitratorNominationLists.ToList())
                {
                    switch (litigationRequestParty)
                    {
                        case LitigationRequestParty.Plaintiff:
                            item.PlaintiffPriority = arbitratorNominationListRequestReplyPayload.ArbitratorNominationListRequestItems.Where(x => x.ArbitratorListId == item.Id).FirstOrDefault()?.Priority;
                            if (item.DefendantPriority.HasValue)
                                item.TotalPriority = item.PlaintiffPriority + item.DefendantPriority;
                            break;
                        case LitigationRequestParty.Defendant:
                            item.DefendantPriority = arbitratorNominationListRequestReplyPayload.ArbitratorNominationListRequestItems.Where(x => x.ArbitratorListId == item.Id).FirstOrDefault()?.Priority;
                            if (item.DefendantPriority.HasValue)
                                item.TotalPriority = item.DefendantPriority + item.PlaintiffPriority;
                            break;
                    }
                }
            }
            var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
            if (!repositorySaveResponse.IsDone)
                return repositorySaveResponse;

            if (litigationRequest.IsPlaintiffArbitratorNominationListDone.HasValue & litigationRequest.IsDefendantArbitratorNominationListDone.HasValue)
            {
                litigationRequest.ArbitratorsNominationListSelectionDoneDateTime = DateTime.Now;
                var arbitratorsList = litigationRequest.LitigationRequestArbitratorNominationLists.OrderBy(x => x.TotalPriority).ThenBy(x => x.DefendantPriority).ToList();

                switch (litigationRequest.ArbitratorsCount)
                {
                    case null:
                        break;
                    case 0:
                        break;
                    case 1:
                        LitigationRequestArbitratorAssigningRequest litigationRequestOneArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                        litigationRequestOneArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                        litigationRequestOneArbitratorAssigningRequest.RequestOn = DateTime.Now;
                        litigationRequestOneArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.Arbitrator;
                        litigationRequestOneArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                        litigationRequestOneArbitratorAssigningRequest.LitigationRequestArbitratorNominationListId = arbitratorsList[0].Id;
                        litigationRequestOneArbitratorAssigningRequest.CitizenId = arbitratorsList[0].ArbitratorCitizenId;
                        litigationRequestOneArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                        await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(litigationRequestOneArbitratorAssigningRequest);
                        arbitratorsList[0].IsSent = true;
                        break;
                    default:
                        for (int record = 0; record < litigationRequest.ArbitratorsCount - 1; record++)
                        {
                            if (record == arbitratorsList.Count)
                                break;
                            LitigationRequestArbitratorAssigningRequest litigationRequestArbitratorAssigningRequest = new LitigationRequestArbitratorAssigningRequest();
                            litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                            litigationRequestArbitratorAssigningRequest.RequestOn = DateTime.Now;
                            litigationRequestArbitratorAssigningRequest.ArbitratorAssigningRequestTypeId = ArbitratorAssigningRequestType.Arbitrator;
                            litigationRequestArbitratorAssigningRequest.LitigationRequestId = litigationRequest.Id;
                            litigationRequestArbitratorAssigningRequest.LitigationRequestArbitratorNominationListId = arbitratorsList[record].Id;
                            litigationRequestArbitratorAssigningRequest.CitizenId = arbitratorsList[record].ArbitratorCitizenId;
                            litigationRequestArbitratorAssigningRequest.Response = LitigationRequestArbitratorAssigningRequestStatus.None;
                            await _repository.AddAsync<LitigationRequestArbitratorAssigningRequest, Guid>(litigationRequestArbitratorAssigningRequest);
                            arbitratorsList[record].IsSent = true;

                            // Send Notification
                            // يوجد طلب مهمه جديده كمحكم علي الدعوي رقم
                            try
                            {
                                var Fmsg = _settingService.GetBySettingKey(SysSettingKeyConstants.Arbitration_Arbitraor_Assigned).Result.Data?.Value;
                                Fmsg = Fmsg?.Replace("{LitigationCode}", litigationRequest.RequestCode);
                                var citizenInfo = (await _repository.GetQueryable<Citizen, Guid>())
                                                    .Where(c => c.Id == litigationRequestArbitratorAssigningRequest.CitizenId)
                                                    .FirstOrDefault();


                                List<SysNotificationDto> notificationList = new List<SysNotificationDto>() {

                           new SysNotificationDto{
                        ActorId = litigationRequestArbitratorAssigningRequest.CitizenId,
                        RequestId=litigationRequest.Id,
                        CreatedOn= DateTime.Now,
                        RequestTypeId=RequestType.ArbitratorTaskrequests,
                        ActorTypeId=NotificationActorType.Arbitrator,
                        Message=Fmsg
                        }

                        };
                                List<string> emailList = new List<string>() {
                                  citizenInfo.Email
                                };

                                List<string> recipientList = new List<string>()
                            {
                                  citizenInfo.Mobile
                            };

                                await _notificationService.SendNotifications(new Contracts.Dtos.Notification.SendEmailDto()
                                {
                                    To = emailList,
                                    Subject = "طلب مهمة جديدة كمحكم علي الدعوي",
                                    Body = Fmsg
                                }, new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                                {
                                    message_text = Fmsg,
                                    recipients = recipientList,
                                    notification_type = "SMS",

                                }, notificationList);

                            }
                            catch (Exception e)
                            {


                            }

                        }
                        break;
                }
                await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    RequestId = litigationRequest.Id,
                    ShowForApplicant = false,
                    LogMessage = "تم الإنتهاء من تعيين هيئة التحكيم وإرسال طلبات المهام"
                });

                repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;

                // Send Notification
                // تم تعيين هيئة التحكيم وإرسال طلبات المهام
                try
                {
                    var msg = _settingService.GetBySettingKey(SysSettingKeyConstants.ArbitratorsNominationList_AssignedAndSent).Result.Data?.Value;
                    msg = msg?.Replace("{LitigationCode}", litigationRequest.RequestCode);
                    var arbitrationAdmin = await _sysEmployeeService.GetAdminsByDepartmentType(DepartmentType.Arbitration);
                    if (arbitrationAdmin.IsDone)
                    {
                        List<SysNotificationDto> notificationList = new List<SysNotificationDto>() {

                           new SysNotificationDto{
                        ActorId = (Guid)arbitrationAdmin.Data?.Id,
                        RequestId=litigationRequest.Id,
                        CreatedOn= DateTime.Now,
                        RequestTypeId=RequestType.LitigationRequest,
                        ActorTypeId=NotificationActorType.Employee,
                        Message=msg
                        }

                        };
                        List<string> emailList = new List<string>() {
                                  arbitrationAdmin.Data?.Email
                                };

                        List<string> recipientList = new List<string>()
                            {
                                  arbitrationAdmin.Data?.Mobile
                            };

                        await _notificationService.SendNotifications(new Contracts.Dtos.Notification.SendEmailDto()
                        {
                            To = emailList,
                            Subject = "تم تعيين هيئة التحكيم وإرسال طلبات المهام",
                            Body = msg
                        }, new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                        {
                            message_text = msg,
                            recipients = recipientList,
                            notification_type = "SMS",

                        }, notificationList);
                    }
                }
                catch (Exception ex)
                {


                }
            }


            return await GetCitizenLitigationRequestForArbitratorsNominationListReply(arbitratorNominationListRequestReplyPayload.RequestId);

        }

        #endregion


        #region Arbitration Verdict
        public async Task<BaseResponse> GetLitigationRequestForReport(Guid requestId)
        {
            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestSessions)
                         .Include(x => x.LitigationRequestArbitrationReports)
                         .ThenInclude(x => x.MeetingLocation)
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
                         .Where(x => x.Id == requestId).AsNoTracking().ToList().FirstOrDefault();

            if (litigationRequest == null)
                return new BaseResponse() { IsDone = false, Message = "طلب غير موجود" };

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            var reports = _mapper.Map<List<LitigationRequestArbitrationReportCreateUpdateDto>>(litigationRequest?.LitigationRequestArbitrationReports);

            bool isDone = false;
            bool allowAdd = true;

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }

            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.JudgmentDecisionIssuing)
                        allowAdd = false;
                    else
                    {
                        if (litigationRequest?.LitigationRequestArbitrationReports != null)
                        {
                            if (litigationRequest.LitigationRequestArbitrationReports.Count > 0)
                            {
                                allowAdd = false;
                                reports.FirstOrDefault().MeetingLocationName = litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.MeetingLocation.MeetingLocationName;
                            }
                        }
                    }
                }
            }

            return new DataResponse<LitigationRequestForArbitrationReportDto>()
            {
                Data = new LitigationRequestForArbitrationReportDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    Report = reports.FirstOrDefault()
                },
                IsDone = true,
            };

        }
        #endregion


        #region Explanation Request
        public async Task<BaseResponse> AddArbitrationReportExplanationRequest(LitigationRequestArbitrationReportExplanationRequestCreateDto requestPayload)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);

                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())

                    .Include(x => x.ArbitratorCitizen)
                    .Include(x => x.ApplicantCitizen)
                    .Include(x => x.Area)
                    .Include(x => x.City)
                                      .Include(x => x.LitigationRequestArbitrationReports)
                                      .ThenInclude(x => x.LitigationRequestArbitrationReportExplanationRequests)
                                      .Include(x => x.Contract)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.Contract.ContractFile)
                                      .Include(x => x.Contract.AssociationRegistrationCertificateFile)
                                      .Include(x => x.Contract.FoundingLetterFile)
                                      .Include(x => x.Contract.InternalRegulationsFile)
                                      .Include(x => x.Contract.FirstPartyFacility).ThenInclude(x => x.Citizen)
                                      .Include(x => x.Contract.FirstPartyCitizen)
                                      .Include(x => x.Contract.SecondPartyFacility).ThenInclude(x => x.Citizen)
                                      .Include(x => x.Contract.SecondPartyCitizen)
                                      .Include(x => x.Contract.LitigationRequestContractType)
                                      .Include(x => x.ApplicantCitizen)
                                      .Where(x => x.Id == requestPayload.LitigationRequestId).SingleOrDefault();

                if (litigationRequest == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "طلب غير موجود"
                    };

                int LitigationRequestArbitrationReportExplanationRequestPeriod = 0;
                int.TryParse(_settingService.GetBySettingKey(SysSettingKeyConstants.Arbitration_LitigationRequestArbitrationReportExplanationRequestPeriod).Result.Data?.Value, out LitigationRequestArbitrationReportExplanationRequestPeriod);
                if (LitigationRequestArbitrationReportExplanationRequestPeriod > 0)
                {
                    var periodHours = (DateTime.Now - litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.JudgmentDate).Value.TotalHours;
                    if (periodHours >= LitigationRequestArbitrationReportExplanationRequestPeriod)
                    {
                        return new BaseResponse() { IsDone = false, Message = "لا يمكن إضافة طلب التفسير لتجاوز المهله المتاحة لذلك" };
                    }
                }

                bool allowAdd = true;

                LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;

                if (litigationRequest.LitigationRequestArbitrationReports.Count() == 0)
                    allowAdd = false;
                else
                {
                    if (litigationRequest.ApplicantCitizenId == referenceId)
                    {
                        litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                    }
                    if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                        {
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                        {
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                        {
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                        }
                    }
                    if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
                    {
                        if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                        {
                            litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                        }
                    }
                    if (litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.LitigationRequestArbitrationReportExplanationRequests != null)
                    {
                        if (litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault()?.LitigationRequestArbitrationReportExplanationRequests.Count(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty) > 0)
                            allowAdd = false;

                    }
                }

                if (allowAdd)
                {
                    if (litigationRequestArbitrationReportExplanationRequestParty == LitigationRequestParty.None)
                    {
                        return new BaseResponse()
                        {
                            IsDone = false,
                            Message = "غير متاح"
                        };
                    }
                    LitigationRequestArbitrationReportExplanationRequest litigationRequestArbitrationReportExplanationRequest = new LitigationRequestArbitrationReportExplanationRequest()
                    {
                        Id = Guid.NewGuid(),
                        ExplanationRequestDate = DateTime.Now,
                        ExplanationRequests = requestPayload.ExplanationRequests,
                        RequestArbitrationReportExplanationRequestParty = litigationRequestArbitrationReportExplanationRequestParty,
                        LitigationRequestArbitrationReportId = litigationRequest.LitigationRequestArbitrationReports.FirstOrDefault().Id,
                    };
                    switch (litigationRequestArbitrationReportExplanationRequestParty)
                    {
                        case LitigationRequestParty.Plaintiff:
                            await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                            {
                                Id = Guid.NewGuid(),
                                CreatedOn = DateTime.Now,
                                RequestId = litigationRequest.Id,
                                ShowForApplicant = false,
                                LogMessage = "تم إضافة طلب استفسار على حكم التحكيم من قبل المدعى"
                            });
                            break;
                        case LitigationRequestParty.Defendant:
                            await _repository.AddAsync<WorkFlowLog, Guid>(new WorkFlowLog()
                            {
                                Id = Guid.NewGuid(),
                                CreatedOn = DateTime.Now,
                                RequestId = litigationRequest.Id,
                                ShowForApplicant = false,
                                LogMessage = "تم إضافة طلب استفسار على حكم التحكيم من قبل المدعى عليه"
                            });
                            break;
                    }

                    await _repository.AddAsync<LitigationRequestArbitrationReportExplanationRequest, Guid>(litigationRequestArbitrationReportExplanationRequest);

                    var saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone)
                        return saveResponse;

                    requestPayload.LitigationRequestArbitrationReportExplanationRequestFiles.ForEach(t =>
                    {
                        _repository.AddAsync<LitigationRequestArbitrationReportExplanationRequestFile, Guid>(new LitigationRequestArbitrationReportExplanationRequestFile()
                        {
                            Id = Guid.NewGuid(),
                            ExplanationRequestFileId = t,
                            ArbitrationReportExplanationRequestId = litigationRequestArbitrationReportExplanationRequest.Id
                        });
                    });

                    var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;

                    litigationRequest.InternalStatusId = RequestStatus.WaitingForExplanation;
                    litigationRequest.ExternalStatusId = RequestStatus.WaitingForExplanation;
                    litigationRequest.IsMinutesOfMeetingApprovalRequired = true;


                    repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                    if (!repositorySaveResponse.IsDone)
                        return repositorySaveResponse;

                    // Send Notification  
                    // تم اضافة طلب تفسير على حكم التحكيم
                    try
                    {
                        var arbitrationAdmin = await _sysEmployeeService.GetAdminsByDepartmentType(DepartmentType.Arbitration);
                        var msg = "";
                        msg = _settingService.GetBySettingKey(SysSettingKeyConstants.Arbitration_ExplanationReport_Added).Result.Data?.Value;
                        msg = msg?.Replace("{LitigationCode}", litigationRequest?.RequestCode);

                        if (arbitrationAdmin.IsDone)
                        {


                            List<SysNotificationDto> notifications = new List<SysNotificationDto>() {
                       new SysNotificationDto{
                 ActorId = litigationRequest.ArbitratorCitizen.Id,
                 RequestId=litigationRequest.Id,
                 CreatedOn= DateTime.Now,
                 RequestTypeId=RequestType.LitigationRequest,
                 ActorTypeId=NotificationActorType.Arbitrator,
                 Message=msg
                 },

                    new SysNotificationDto{
                 ActorId = (Guid)arbitrationAdmin.Data?.Id,
                 RequestId=litigationRequest.Id,
                 CreatedOn= DateTime.Now,
                 RequestTypeId=RequestType.LitigationRequest,
                 ActorTypeId=NotificationActorType.Employee,
                 Message=msg
                 }

                 };
                            List<string> emails = new List<string>() {
                      litigationRequest.ArbitratorCitizen.Email,
                         litigationRequest.ApplicantCitizen.Email,
                           arbitrationAdmin.Data?.Email
                         };

                            List<string> recipients = new List<string>()
                     {
                     litigationRequest.ArbitratorCitizen.Mobile,
                         litigationRequest.ApplicantCitizen.Mobile,
                           arbitrationAdmin.Data?.Mobile
                     };
                            if (litigationRequestArbitrationReportExplanationRequest.RequestArbitrationReportExplanationRequestParty == LitigationRequestParty.Plaintiff)
                            {
                                notifications.Add(new SysNotificationDto
                                {
                                    ActorId = litigationRequest.ApplicantCitizen.Id,
                                    RequestId = litigationRequest.Id,
                                    CreatedOn = DateTime.Now,
                                    RequestTypeId = RequestType.LitigationRequest,
                                    ActorTypeId = NotificationActorType.Plaintiff,
                                    Message = msg
                                });
                                emails.Add(litigationRequest.ApplicantCitizen.Email);
                                recipients.Add(litigationRequest.ApplicantCitizen.Mobile);

                                if (litigationRequest?.Contract?.FirstPartyCitizenId != null && litigationRequest?.Contract?.FirstPartyCitizenId != litigationRequest.ApplicantCitizenId)
                                {
                                    notifications.Add(new SysNotificationDto
                                    {
                                        ActorId = (Guid)litigationRequest.Contract.FirstPartyCitizenId,
                                        RequestId = litigationRequest.Id,
                                        CreatedOn = DateTime.Now,
                                        RequestTypeId = RequestType.LitigationRequest,
                                        ActorTypeId = NotificationActorType.Plaintiff,
                                        Message = msg
                                    });
                                    emails.Add(litigationRequest.Contract.FirstPartyCitizen.Email);
                                    recipients.Add(litigationRequest.Contract.FirstPartyCitizen.Mobile);


                                };
                                if (litigationRequest?.Contract?.FirstPartyFacility != null && litigationRequest?.Contract?.FirstPartyFacility.CitizenId != litigationRequest.ApplicantCitizenId)
                                {
                                    notifications.Add(new SysNotificationDto
                                    {
                                        ActorId = (Guid)litigationRequest.Contract.FirstPartyFacility.CitizenId,
                                        RequestId = litigationRequest.Id,
                                        CreatedOn = DateTime.Now,
                                        RequestTypeId = RequestType.LitigationRequest,
                                        ActorTypeId = NotificationActorType.Plaintiff,
                                        Message = msg
                                    });
                                    emails.Add(litigationRequest.Contract.FirstPartyFacility.Citizen.Email);
                                    recipients.Add(litigationRequest.Contract.FirstPartyFacility.Citizen.Mobile);
                                };
                            }

                            if (litigationRequestArbitrationReportExplanationRequest.RequestArbitrationReportExplanationRequestParty == LitigationRequestParty.Defendant)
                            {
                                if (litigationRequest?.Contract?.SecondPartyCitizenId != null)
                                {
                                    notifications.Add(new SysNotificationDto
                                    {
                                        ActorId = (Guid)litigationRequest.Contract.SecondPartyCitizenId,
                                        RequestId = litigationRequest.Id,
                                        CreatedOn = DateTime.Now,
                                        RequestTypeId = RequestType.LitigationRequest,
                                        ActorTypeId = NotificationActorType.Defendant,
                                        Message = msg
                                    });
                                    emails.Add(litigationRequest.Contract.SecondPartyCitizen.Email);
                                    recipients.Add(litigationRequest.Contract.SecondPartyCitizen.Mobile);
                                };
                                if (litigationRequest?.Contract?.SecondPartyFacilityId != null)
                                {
                                    notifications.Add(new SysNotificationDto
                                    {
                                        ActorId = (Guid)litigationRequest.Contract.SecondPartyFacility.CitizenId,
                                        RequestId = litigationRequest.Id,
                                        CreatedOn = DateTime.Now,
                                        RequestTypeId = RequestType.LitigationRequest,
                                        ActorTypeId = NotificationActorType.Defendant,
                                        Message = msg
                                    });
                                    emails.Add(litigationRequest.Contract.SecondPartyFacility.Citizen.Email);
                                    recipients.Add(litigationRequest.Contract.SecondPartyFacility.Citizen.Mobile);
                                };
                            }

                            await _notificationService.SendNotifications(new Contracts.Dtos.Notification.SendEmailDto()
                            {
                                To = emails,
                                Subject = "اضافة طلب تفسير على حكم التحكيم",
                                Body = msg
                            }, new Contracts.Dtos.Integration.GEE.SMSRequestDto()
                            {
                                message_text = msg,
                                recipients = recipients,
                                notification_type = "SMS",

                            }, notifications);
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }
                else
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "لا يمكن إضافة محضر جديد"
                    };

                }
            }
            catch (Exception ex)
            {
                new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString()
                };
            }
            return await GetLitigationRequestForReportExplanationRequest(requestPayload.LitigationRequestId);
        }
        public async Task<BaseResponse> GetLitigationRequestForReportExplanationRequest(Guid requestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestArbitrationReports)
                         .ThenInclude(x => x.LitigationRequestArbitrationReportExplanationRequests)
                                                  .ThenInclude(x => x.LitigationRequestArbitrationReportExplanationRequestFiles)
                                                                           .ThenInclude(x => x.ExplanationRequestFile)


                         .Include(x => x.ApplicantCitizen)
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
                         .Where(x => x.Id == requestId).AsNoTracking().ToList().FirstOrDefault();

            if (litigationRequest == null)
                return new BaseResponse() { IsDone = false, Message = "طلب غير موجود" };

            bool isDone = false;
            bool allowAdd = true;

            var request = _mapper.Map<LitigationRequestShortDto>(litigationRequest);

            LitigationRequestParty litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.None;
            if (litigationRequest.ApplicantCitizenId == referenceId)
            {
                litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
            }
            if (litigationRequest.Contract.FirstPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyCitizenId == referenceId)
                {
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyCitizenId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyCitizenId == referenceId)
                {
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }
            if (litigationRequest.Contract.FirstPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.FirstPartyFacility?.CitizenId == referenceId)
                {
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Plaintiff;
                }
            }
            if (litigationRequest.Contract.SecondPartyFacilityId.HasValue)
            {
                if (litigationRequest.Contract.SecondPartyFacility?.CitizenId == referenceId)
                {
                    litigationRequestArbitrationReportExplanationRequestParty = LitigationRequestParty.Defendant;
                }
            }

            if (litigationRequest.IsFinished.HasValue)
            {
                if (litigationRequest.IsFinished.Value)
                {
                    allowAdd = false;
                    isDone = true;
                }
            }

            var requestArbitrationReportExplanationRequest = litigationRequest?.LitigationRequestArbitrationReports.FirstOrDefault()?
                .LitigationRequestArbitrationReportExplanationRequests
                .Where(x => x.RequestArbitrationReportExplanationRequestParty == litigationRequestArbitrationReportExplanationRequestParty).FirstOrDefault();

            if (!isDone)
            {
                if (litigationRequest?.LitigationRequestStage != LitigationRequestStage.Arbitration)
                    allowAdd = false;
                else
                {
                    if (litigationRequest?.LitigationRequestSubStage != LitigationRequestSubStage.JudgmentDecisionIssued)
                        allowAdd = false;
                    else
                    {
                        if (requestArbitrationReportExplanationRequest != null)
                            allowAdd = false;
                    }
                }
            }
            var reportExplanationRequest = _mapper.Map<LitigationRequestArbitrationReportExplanationRequestGetDto>(requestArbitrationReportExplanationRequest);

            return new DataResponse<LitigationRequestForArbitrationReportExplanationRequestDto>()
            {
                Data = new LitigationRequestForArbitrationReportExplanationRequestDto()
                {
                    LitigationRequestShortDto = request,
                    AllowAdd = allowAdd,
                    Report = reportExplanationRequest
                },
                IsDone = true,
            };

        }
        #endregion
    }
}
