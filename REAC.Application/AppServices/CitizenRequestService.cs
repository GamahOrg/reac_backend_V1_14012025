using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.CitizenRequests;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;

namespace REAC.Application.AppServices
{
    public class CitizenRequestService : ICitizenRequestService
    {
        protected IEntityRepository _repository;
        protected IMapper _mapper;
        protected ILoggerManager _loggerManager;
        private readonly IJwtReader _jwtReader;
        public CitizenRequestService(IEntityRepository repository,
          IMapper mapper, ILoggerManager loggerManager, IJwtReader jwtReader)
        {
            _repository = repository;
            _mapper = mapper;
            _jwtReader = jwtReader;
            _loggerManager = loggerManager;
        }

        public async Task<BaseResponse> GetCitizenRequest()
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                    .Include(x => x.SysCitizenRequestType)
                    .Include(x => x.Status)
                    .Include(x => x.SysArbitratorClass)
                    .Include(x => x.SysArea)
                    .Include(x => x.CitizenRequestAcademicDegreeFiles)
                    .ThenInclude(x => x.AcademicDegreeFile)
                    .Include(x => x.CitizenRequestNotes)
                    .Include(x => x.CitizenRequestFreelanceWorkDocumentFiles)
                    .ThenInclude(x => x.FreelanceWorkDocumentFile)
                    .Include(x => x.CitizenRequestExperienceCertificates)
                    .ThenInclude(x => x.CitizenRequestExperienceCertificateFiles)
                    .ThenInclude(x => x.ExperienceCertificateFile)
                    .AsNoTracking()
                              .ToList();

            var fullDelegators = _mapper.Map<List<CitizenRequestShortDataDto>>(citizenRequests);

            return new DataResponse<List<CitizenRequestShortDataDto>>()
            {
                IsDone = true,
                Data = fullDelegators
            };
        }

        public async Task<BaseResponse> GetCitizenRequestById(Guid citizenRequestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                    .Include(x => x.SysCitizenRequestType)
                    .Include(x => x.Status)
                    .Include(x => x.SysArbitratorClass)
                    .Include(x => x.SysArea)
                    .Include(x => x.CitizenRequestAcademicDegreeFiles)
                    .ThenInclude(x => x.AcademicDegreeFile)
                    .Include(x => x.CitizenRequestNotes)
                    .Include(x => x.CitizenRequestFreelanceWorkDocumentFiles)
                    .ThenInclude(x => x.FreelanceWorkDocumentFile)
                    .Include(x => x.CitizenRequestExperienceCertificates)
                    .ThenInclude(x => x.CitizenRequestExperienceCertificateFiles)
                    .ThenInclude(x => x.ExperienceCertificateFile)
                    .AsNoTracking().Where(x => x.Id == citizenRequestId)
                              .ToList();

            if (citizenRequests.Count > 0)
            {
                var fullDelegators = _mapper.Map<CitizenRequestFullDataDto>(citizenRequests.FirstOrDefault());

                return new DataResponse<CitizenRequestFullDataDto>()
                {
                    IsDone = true,
                    Data = fullDelegators
                };
            }
            return new BaseResponse()
            {
                IsDone = false,
                Message = "Not Found"
            };
        }

        public async Task<BaseResponse> SaveCitizenRequest(CitizenRequestCreateUpdateDto citizenRequestCreateUpdateDto)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            if (citizenRequestCreateUpdateDto.Id == Guid.Empty)
            {
                var request = _mapper.Map<CitizenRequest>(citizenRequestCreateUpdateDto);
                request.CitizenId = referenceId;
                request.Id = Guid.NewGuid();
                if (citizenRequestCreateUpdateDto.IsSend)
                    request.StatusId = CitizenRequestStatus.WaitingForCertify;
                else
                    request.StatusId = CitizenRequestStatus.Draft;

                await _repository.AddAsync<CitizenRequest, Guid>(request);
                var response = await _repository.SaveChangesAsync();
                return response;
            }
            else
            {
                var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                        .Include(x => x.SysCitizenRequestType)
                        .Include(x => x.Status)
                        .Include(x => x.SysArbitratorClass)
                        .Include(x => x.SysArea)
                        .Include(x => x.CitizenRequestAcademicDegreeFiles)
                        .ThenInclude(x => x.AcademicDegreeFile)
                        .Include(x => x.CitizenRequestNotes)
                        .Include(x => x.CitizenRequestFreelanceWorkDocumentFiles)
                        .ThenInclude(x => x.FreelanceWorkDocumentFile)
                        .Include(x => x.CitizenRequestExperienceCertificates)
                        .ThenInclude(x => x.CitizenRequestExperienceCertificateFiles)
                        .ThenInclude(x => x.ExperienceCertificateFile)
                        .Where(x => x.Id == citizenRequestCreateUpdateDto.Id)
                                  .ToList();

                if (citizenRequests.Count == 0)
                    return new BaseResponse() { IsDone = false, Message = "Not Found" };

                #region citizenRequest
                citizenRequests.FirstOrDefault().AreaId = citizenRequestCreateUpdateDto.AreaId;
                citizenRequests.FirstOrDefault().AcademicDegreeName = citizenRequestCreateUpdateDto.AcademicDegreeName;
                citizenRequests.FirstOrDefault().AcademicDegreeName = citizenRequestCreateUpdateDto.AcademicDegreeName;
                citizenRequests.FirstOrDefault().ExperienceYears = citizenRequestCreateUpdateDto.ExperienceYears;
                citizenRequests.FirstOrDefault().ExperienceYears = citizenRequestCreateUpdateDto.ExperienceYears;
                citizenRequests.FirstOrDefault().ArbitratorClassId = citizenRequestCreateUpdateDto.ArbitratorClassId;
                citizenRequests.FirstOrDefault().IsVolunteerApproval = citizenRequestCreateUpdateDto.IsVolunteerApproval;
                citizenRequests.FirstOrDefault().Brief = citizenRequestCreateUpdateDto.Brief;
                #endregion

                citizenRequests.FirstOrDefault().CitizenRequestFreelanceWorkDocumentFiles.ToList().ForEach(x =>
                {
                    _repository.Remove<CitizenRequestFreelanceWorkDocumentFile, Guid>(x);
                    var saveResponse = _repository.SaveChangesAsync().Result;

                });
                citizenRequests.FirstOrDefault().CitizenRequestAcademicDegreeFiles.ToList().ForEach(x =>
                {
                    _repository.Remove<CitizenRequestAcademicDegreeFile, Guid>(x);
                    var saveResponse = _repository.SaveChangesAsync().Result;
                });

                citizenRequests.FirstOrDefault().CitizenRequestExperienceCertificates.ToList().ForEach(x =>
                {
                    x.CitizenRequestExperienceCertificateFiles.ToList().ForEach(x =>
                    {
                        citizenRequests.FirstOrDefault().CitizenRequestAcademicDegreeFiles.ToList().ForEach(x =>
                        {
                            _repository.Remove<CitizenRequestAcademicDegreeFile, Guid>(x);
                            var saveResponse = _repository.SaveChangesAsync().Result;

                        });
                    });
                });
                citizenRequests.FirstOrDefault().CitizenRequestExperienceCertificates.ToList().ForEach(x =>
                {
                    _repository.Remove<CitizenRequestExperienceCertificate, Guid>(x);
                    var saveResponse = _repository.SaveChangesAsync().Result;
                });


                #region CitizenRequestFreelanceWorkDocumentFiles
                citizenRequestCreateUpdateDto.CitizenRequestFreelanceWorkDocumentFiles.ToList().ForEach(x =>
                {
                    _repository.AddAsync<CitizenRequestFreelanceWorkDocumentFile, Guid>(new CitizenRequestFreelanceWorkDocumentFile
                    {
                        Id = Guid.NewGuid(),
                        CitizenRequestId = citizenRequests.FirstOrDefault().Id,
                        FreelanceWorkDocumentFileId = x.FreelanceWorkDocumentFileId
                    });
                    var response = _repository.SaveChangesAsync();
                });
                #endregion

                #region CitizenRequestAcademicDegreeFiles
                citizenRequestCreateUpdateDto.CitizenRequestAcademicDegreeFiles.ToList().ForEach(x =>
                {
                    _repository.AddAsync<CitizenRequestAcademicDegreeFile, Guid>(new CitizenRequestAcademicDegreeFile
                    {
                        CitizenRequestId = citizenRequests.FirstOrDefault().Id,
                        Id = Guid.NewGuid(),
                        AcademicDegreeFileId = x.AcademicDegreeFileId
                    });
                    var response = _repository.SaveChangesAsync();
                });
                #endregion

                #region CitizenRequestExperienceCertificates

                citizenRequestCreateUpdateDto.CitizenRequestExperienceCertificates.ToList().ForEach(x =>
                {
                    CitizenRequestExperienceCertificate citizenRequestExperienceCertificate = new CitizenRequestExperienceCertificate();
                    citizenRequestExperienceCertificate.Id = Guid.NewGuid();
                    citizenRequestExperienceCertificate.CitizenRequestId = citizenRequests.FirstOrDefault().Id;
                    citizenRequestExperienceCertificate.AcquisitionDate = x.AcquisitionDate;
                    citizenRequestExperienceCertificate.CertificateName = x.CertificateName;
                    citizenRequestExperienceCertificate.IssuingEntityName = x.IssuingEntityName;
                    _repository.AddAsync<CitizenRequestExperienceCertificate, Guid>(citizenRequestExperienceCertificate);
                    var response = _repository.SaveChangesAsync();


                    x.CitizenRequestExperienceCertificateFiles.ToList().ForEach(y =>
                    {
                        _repository.AddAsync<CitizenRequestExperienceCertificateFile, Guid>(new CitizenRequestExperienceCertificateFile()
                        {
                            CitizenRequestExperienceCertificateId = citizenRequestExperienceCertificate.Id,
                            Id = Guid.NewGuid(),
                            ExperienceCertificateFileId = y.ExperienceCertificateFileId
                        });
                        var response = _repository.SaveChangesAsync();
                    });
                });
                #endregion

                if (citizenRequestCreateUpdateDto.IsSend & citizenRequests.FirstOrDefault().StatusId == CitizenRequestStatus.Draft)
                    ExecuteAction(CitizenRequestAction.Send, citizenRequests.FirstOrDefault().Id, null);
                if (citizenRequestCreateUpdateDto.IsSend & citizenRequests.FirstOrDefault().StatusId == CitizenRequestStatus.SentForEdit)
                    ExecuteAction(CitizenRequestAction.SendFromEdit, citizenRequests.FirstOrDefault().Id, null);

                return new BaseResponse();

            }
        }

        public async Task<BaseResponse> ExecuteAction(CitizenRequestAction citizenRequestAction, Guid requestId, string? notes)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                  .Where(x => x.Id == requestId)
                              .ToList();

            if (citizenRequests.Count == 0)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            BaseResponse saveResponse = new BaseResponse() { IsDone = false, Message = "Not Found" };
            switch (citizenRequestAction)
            {
                case CitizenRequestAction.Send:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.WaitingForCertify;
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
                case CitizenRequestAction.SendFromEdit:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.SentForEdit;
                    CitizenRequestNote citizenRequestNote = new CitizenRequestNote();
                    citizenRequestNote.Id = Guid.NewGuid();
                    citizenRequestNote.CitizenRequestId = requestId;
                    citizenRequestNote.Notes = notes;
                    citizenRequestNote.NoteDateTime = DateTime.Now;
                    await _repository.AddAsync<CitizenRequestNote, Guid>(citizenRequestNote);
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
                case CitizenRequestAction.Reject:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.Rejected;
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
                case CitizenRequestAction.CertifiedWithFees:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.WaitingForMembershipFeesToBePaid;
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
                case CitizenRequestAction.CertifiedWithOutFees:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.Certified;
                    var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                         .Where(x => x.Id == citizenRequests.FirstOrDefault().CitizenId)
                          .ToList();
                    if (citizen != null)
                    {
                        if (citizen.Count > 0)
                        {
                            switch (citizenRequests.FirstOrDefault().CitizenRequestTypeId)
                            {
                                case CitizenRequestType.Reformer:
                                    citizen.FirstOrDefault().IsReformer = true;
                                    break;
                                case CitizenRequestType.Arbitrator:
                                    citizen.FirstOrDefault().IsArbitrator = true;
                                    break;
                            }
                        }
                    }
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
                case CitizenRequestAction.SendForEdit:
                    citizenRequests.FirstOrDefault().StatusId = CitizenRequestStatus.SentForEdit;
                    saveResponse = await _repository.SaveChangesAsync();
                    break;
            }
            return saveResponse;
        }
    }
}
