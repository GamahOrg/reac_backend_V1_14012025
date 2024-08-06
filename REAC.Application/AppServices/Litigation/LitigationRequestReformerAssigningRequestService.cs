using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices.Litigation
{

    public class LitigationRequestReformerAssigningRequestService : BasicService<LitigationRequestReformerAssigningRequest, Guid>, ILitigationRequestReformerAssigningRequest
    {
        private readonly REACDBContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtReader _jwtReader;
        private readonly ICitizenService _citizen;

        public LitigationRequestReformerAssigningRequestService(REACDBContext context, IEntityRepository repository, IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader, ICitizenService citizen) : base(repository, mapper, loggerManager, jwtReader)
        {
            _context = context;
            _mapper = mapper;
            _jwtReader = jwtReader;
            _citizen = citizen;

        }
        public async Task<BaseResponse> AddLitigationRequestReformerAssigningRequestsResponse(Guid litigationRequestReformerAssigningRequestId, LitigationRequestReformerAssigningRequestStatus response, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequestReformerAssigningRequest = (await _repository.GetQueryable<LitigationRequestReformerAssigningRequest, Guid>())
                         .Where(x => x.Id == litigationRequestReformerAssigningRequestId).ToList().FirstOrDefault();

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())?
                        .Include(x => x.LitigationRequestExtendPeriodRequests)
                        .Where(x => x.Id == litigationRequestReformerAssigningRequest.LitigationRequestId)
                        .SingleOrDefault();


            if (litigationRequestReformerAssigningRequest != null)
            {
                litigationRequestReformerAssigningRequest.ResponseOn = DateTime.Now;
                litigationRequestReformerAssigningRequest.Response = response;
            }

            if (litigationRequest != null)
            {
                switch (response)
                {
                    case LitigationRequestReformerAssigningRequestStatus.Approve:
                        litigationRequest.ReformerCitizenId = referenceId;
                        litigationRequest.LitigationRequestStage = LitigationRequestStage.Reconciliation;
                        litigationRequest.LitigationRequestSubStage = LitigationRequestSubStage.AddSession;
                        litigationRequest.StartReconciliationDate = DateTime.Now;
                        // Send Notification
                        break;
                    case LitigationRequestReformerAssigningRequestStatus.Reject:
                        var department = (await _repository.GetQueryable<SysDepartment, Guid>())?
                                .Where(x => x.DepartmentType == DepartmentType.Reconciliation)
                                .SingleOrDefault();
                        if (department != null)
                        {
                            switch (department.DistributionMode)
                            {
                                case DepartmentDistributionMode.AutoDistribution:
                                    var reformers = (await _repository.GetQueryable<Citizen, Guid>())?
                                                .Where(x => x.IsReformer == true & x.ReformerStatusId == ReformerStatus.Certified)
                                                .OrderBy(x => x.ReformerLitigationRequests
                                                .Count(t => t.IsFinished == false))
                                                .ToList();
                                    if (reformers != null)
                                    {
                                        if (reformers.Any())
                                        {
                                            LitigationRequestReformerAssigningRequest newLitigationRequestReformerAssigningRequest = new LitigationRequestReformerAssigningRequest();
                                            newLitigationRequestReformerAssigningRequest.CitizenId = reformers.FirstOrDefault().Id;
                                            newLitigationRequestReformerAssigningRequest.LitigationRequestId = litigationRequest.Id;
                                            newLitigationRequestReformerAssigningRequest.Response = LitigationRequestReformerAssigningRequestStatus.None;
                                            await _repository.AddAsync<LitigationRequestReformerAssigningRequest, Guid>(newLitigationRequestReformerAssigningRequest);
                                        }
                                        var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                                        if (!repositorySaveResponse.IsDone)
                                            return repositorySaveResponse;
                                    }
                                    break;
                            }
                        }
                        // Send Notification
                        break;
                }
            }
            var saveResponse = await _repository.SaveChangesAsync(cancellationToken);
            if (!saveResponse.IsDone)
                return saveResponse;
            return await GetLitigationRequestReformerAssigningRequests(litigationRequestReformerAssigningRequest.LitigationRequestId, litigationRequestReformerAssigningRequestId, cancellationToken);
        }
        public async Task<BaseResponse> GetLitigationRequestReformerAssigningRequests(CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequests = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestReformerAssigningRequests)
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
                         .Where(x => x.LitigationRequestReformerAssigningRequests
                         .Where(t => t.CitizenId == referenceId).Count() > 0).ToList();

            List<CustomLitigationRequestMircoDto> litigationRequestMircoDtos = new List<CustomLitigationRequestMircoDto>();
            if (litigationRequests != null)
            {
                foreach (var litigationRequest in litigationRequests)
                {
                    CustomLitigationRequestMircoDto litigationRequestMircoDto = new CustomLitigationRequestMircoDto();
                    litigationRequestMircoDto.Id = litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault().Id;
                    litigationRequestMircoDto.RequestId = litigationRequest.Id;
                    litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                    litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                    litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                    litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                    litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                    litigationRequestMircoDto.Response = litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response;
                    litigationRequestMircoDto.ResponseOn = litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.ResponseOn;

                    bool allowAdd = true;

                    if (litigationRequest.LitigationRequestReformerAssigningRequests.Any())
                    {
                        if (litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Approve |
                            litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Cancelled |
                            litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Reject)
                        {
                            allowAdd = false;
                        }
                    }

                    if (allowAdd)
                        litigationRequestMircoDto.IsDone = "--";
                    else
                        litigationRequestMircoDto.IsDone = "تم";

                    litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;
                    litigationRequestMircoDtos.Add(litigationRequestMircoDto);
                }
            }

            return new DataResponse<List<CustomLitigationRequestMircoDto>>()
            {
                Data = litigationRequestMircoDtos,
                IsDone = true
            };

            throw new NotImplementedException();
        }
        public async Task<BaseResponse> GetLitigationRequestReformerAssigningRequests(Guid requestId, Guid LitigationRequestReformerAssigningRequestId, CancellationToken cancellationToken)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);

            var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                         .Include(x => x.Area)
                         .Include(x => x.City)
                         .Include(x => x.LitigationRequestReformerAssigningRequests)
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
                         .Where(x => x.LitigationRequestReformerAssigningRequests
                         .Where(t => t.Id == LitigationRequestReformerAssigningRequestId).Count() > 0).ToList().FirstOrDefault();

            if (litigationRequest != null)
            {
                CustomLitigationRequestMircoDto litigationRequestMircoDto = new CustomLitigationRequestMircoDto();

                litigationRequestMircoDto.Id = litigationRequest.Id;
                litigationRequestMircoDto.RequestId = requestId;
                litigationRequestMircoDto.RequestDate = litigationRequest.CreatedOn;
                litigationRequestMircoDto.RequestCode = litigationRequest.RequestCode;
                litigationRequestMircoDto.FirstPartyName = (litigationRequest.Contract.FirstPartyCitizen == null) ? litigationRequest.Contract.FirstPartyFacility.ArabicFacilityName : litigationRequest.Contract.FirstPartyCitizen.FullName;
                litigationRequestMircoDto.SecondPartyName = (litigationRequest.Contract.SecondPartyCitizen == null) ? litigationRequest.Contract.SecondPartyFacility.ArabicFacilityName : litigationRequest.Contract.SecondPartyCitizen.FullName;
                litigationRequestMircoDto.RequestTypeName = litigationRequest.Contract.LitigationRequestContractType.TypeName;
                litigationRequestMircoDto.Response = litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response;
                litigationRequestMircoDto.ResponseOn = litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.ResponseOn;

                bool allowAdd = true;

                if (litigationRequest.LitigationRequestReformerAssigningRequests.Any())
                {
                    if (litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Approve |
                        litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Cancelled |
                        litigationRequest.LitigationRequestReformerAssigningRequests.FirstOrDefault()?.Response == LitigationRequestReformerAssigningRequestStatus.Reject)
                    {
                        allowAdd = false;
                    }
                }

                if (allowAdd)
                    litigationRequestMircoDto.IsDone = "--";
                else
                    litigationRequestMircoDto.IsDone = "تم";

                litigationRequestMircoDto.Status = litigationRequest.InternalStatus.RequestStatus;

                return new DataResponse<CustomLitigationRequestMircoDto>()
                {
                    Data = litigationRequestMircoDto,
                    IsDone = true
                };

            }
            throw new NotImplementedException();
        }
    }
}