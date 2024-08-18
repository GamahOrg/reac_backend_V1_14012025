using AutoMapper;
using Core.GenericRepository.BaseRepository;
using Core.GenericRepository.Shared.Pagging;
using Core.Log.Application.Contracts.IAppServices;
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.CitizenRequests;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Actor;

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

        public async Task<BaseResponse> GetCitizenRequests(int pageIndex, int pageSize, bool isSend = false, CitizenRequestType? citizenRequestTypeId = null)
        {
            try
            {
                Guid referenceId = Guid.Empty;
                _jwtReader.GetReferenceId(ref referenceId);
                var citizenRequestsQuery = (await _repository.GetQueryable<CitizenRequest, Guid>())
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
                        .AsNoTracking();

                if (isSend)
                {
                    citizenRequestsQuery = citizenRequestsQuery.Where(x => x.StatusId != CitizenRequestStatus.Draft && x.CitizenRequestTypeId == citizenRequestTypeId);
                }

                var citizenRequests = citizenRequestsQuery.GetPaged(pageIndex, pageSize);

                return new DataResponse<PagedResult<CitizenRequestShortDataDto>>()
                {
                    IsDone = true,
                    Data = new PagedResult<CitizenRequestShortDataDto>
                    {
                        CurrentPage = citizenRequests.CurrentPage,
                        PageSize = citizenRequests.PageSize,
                        PageCount = citizenRequests.PageCount,
                        TotalCount = citizenRequests.TotalCount,
                        Results = _mapper.Map<List<CitizenRequestShortDataDto>>(citizenRequests.Results)
                    }
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<BaseResponse> GetCitizenRequestById(Guid citizenRequestId)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                .Where(x => x.Id == citizenRequestId)
                .Include(x => x.SysCitizenRequestType)
                .Include(x => x.Status)
                .Include(x => x.SysArbitratorClass)
                .Include(x => x.SysArea)
                .Include(x => x.Citizen)
                .Include(x => x.CitizenRequestNotes)
                .Include(x => x.CitizenRequestAcademicDegreeFiles)
                .ThenInclude(x => x.AcademicDegreeFile)
                .Include(x => x.CitizenRequestNotes)
                .Include(x => x.CitizenRequestFreelanceWorkDocumentFiles)
                .ThenInclude(x => x.FreelanceWorkDocumentFile)
                .Include(x => x.CitizenRequestExperienceCertificates)
                .ThenInclude(x => x.CitizenRequestExperienceCertificateFiles)
                .ThenInclude(x => x.ExperienceCertificateFile)
                .AsSplitQuery().AsNoTracking().ToList();

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

                request.RequestOn = DateTime.Now;
                await _repository.AddAsync<CitizenRequest, Guid>(request);
                var response = await _repository.SaveChangesAsync();
                if (!response.IsDone) return response;
                return response;
            }
            else
            {
                var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                    .Where(x => x.Id == citizenRequestCreateUpdateDto.Id)
                    .Include(x => x.SysCitizenRequestType)
                    .Include(x => x.Status)
                    .Include(x => x.SysArbitratorClass)
                    .Include(x => x.SysArea)
                    .Include(x => x.CitizenRequestAcademicDegreeFiles).ThenInclude(x => x.AcademicDegreeFile)
                    .Include(x => x.CitizenRequestNotes)
                    .Include(x => x.CitizenRequestFreelanceWorkDocumentFiles).ThenInclude(x => x.FreelanceWorkDocumentFile)
                    .Include(x => x.CitizenRequestExperienceCertificates).ThenInclude(x => x.CitizenRequestExperienceCertificateFiles)
                    .ThenInclude(x => x.ExperienceCertificateFile)
                    .AsSplitQuery().FirstOrDefault();

                if (citizenRequests is null)
                    return new BaseResponse() { IsDone = false, Message = "Not Found" };

                #region citizenRequest

                citizenRequests.AreaId = citizenRequestCreateUpdateDto.AreaId;
                citizenRequests.AcademicDegreeName = citizenRequestCreateUpdateDto.AcademicDegreeName;
                citizenRequests.AcademicDegreeName = citizenRequestCreateUpdateDto.AcademicDegreeName;
                citizenRequests.ExperienceYears = citizenRequestCreateUpdateDto.ExperienceYears;
                citizenRequests.ExperienceYears = citizenRequestCreateUpdateDto.ExperienceYears;
                citizenRequests.ArbitratorClassId = citizenRequestCreateUpdateDto.ArbitratorClassId;
                citizenRequests.IsVolunteerApproval = citizenRequestCreateUpdateDto.IsVolunteerApproval;
                citizenRequests.Brief = citizenRequestCreateUpdateDto.Brief;

                #endregion

                var response = new BaseResponse { IsDone = true };

                #region CitizenRequestFreelanceWorkDocumentFiles

                var deleteCitizenRequestFreelanceWorkDocumentFiles = citizenRequests.CitizenRequestFreelanceWorkDocumentFiles
                    .Where(x => !citizenRequestCreateUpdateDto.CitizenRequestFreelanceWorkDocumentFiles.Any(y => y.Id == x.Id));

                _repository.RemoveRange<CitizenRequestFreelanceWorkDocumentFile, Guid>(deleteCitizenRequestFreelanceWorkDocumentFiles);
                response = await _repository.SaveChangesAsync();
                if (!response.IsDone) return response;

                foreach (var citizenRequestFreelanceWorkDocument in citizenRequestCreateUpdateDto.CitizenRequestFreelanceWorkDocumentFiles)
                {
                    if (citizenRequestFreelanceWorkDocument.Id == Guid.Empty)
                    {
                        await _repository.AddAsync<CitizenRequestFreelanceWorkDocumentFile, Guid>(new CitizenRequestFreelanceWorkDocumentFile
                        {
                            Id = Guid.NewGuid(),
                            CitizenRequestId = citizenRequests.Id,
                            FreelanceWorkDocumentFileId = citizenRequestFreelanceWorkDocument.FreelanceWorkDocumentFileId
                        });
                        response = await _repository.SaveChangesAsync();
                        if (!response.IsDone) return response;
                    }
                }


                #endregion

                #region CitizenRequestAcademicDegreeFiles

                var deleteCitizenRequestAcademicDegreeFiles = citizenRequests.CitizenRequestAcademicDegreeFiles
                    .Where(x => !citizenRequestCreateUpdateDto.CitizenRequestAcademicDegreeFiles.Any(y => y.Id == x.Id));

                _repository.RemoveRange<CitizenRequestAcademicDegreeFile, Guid>(deleteCitizenRequestAcademicDegreeFiles);
                response = await _repository.SaveChangesAsync();
                if (!response.IsDone) return response;


                foreach (var citizenRequestAcademicDegree in citizenRequestCreateUpdateDto.CitizenRequestAcademicDegreeFiles)
                {
                    if (citizenRequestAcademicDegree.Id == Guid.Empty)
                    {
                        await _repository.AddAsync<CitizenRequestAcademicDegreeFile, Guid>(new CitizenRequestAcademicDegreeFile
                        {
                            CitizenRequestId = citizenRequests.Id,
                            Id = Guid.NewGuid(),
                            AcademicDegreeFileId = citizenRequestAcademicDegree.AcademicDegreeFileId
                        });
                        response = await _repository.SaveChangesAsync();
                        if (!response.IsDone) return response;
                    }
                }

                #endregion

                #region CitizenRequestExperienceCertificates

                var deleteCitizenRequestExperienceCertificates = citizenRequests.CitizenRequestExperienceCertificates
                    .Where(x => !citizenRequestCreateUpdateDto.CitizenRequestExperienceCertificates.Any(y => y.Id == x.Id));

                _repository.RemoveRange<CitizenRequestExperienceCertificate, Guid>(deleteCitizenRequestExperienceCertificates);
                response = await _repository.SaveChangesAsync();
                if (!response.IsDone) return response;


                foreach (var citizenRequestExperienceCertificates in citizenRequestCreateUpdateDto.CitizenRequestExperienceCertificates)
                {
                    if (citizenRequestExperienceCertificates.AcquisitionDate > DateTime.Now)
                        return new BaseResponse() { IsDone = false, Message = "تأكد من سنة الحصول على شهادة الخبرة" };

                    if (citizenRequestExperienceCertificates.Id == Guid.Empty)
                    {
                        CitizenRequestExperienceCertificate citizenRequestExperienceCertificate = new CitizenRequestExperienceCertificate();
                        citizenRequestExperienceCertificate.Id = Guid.NewGuid();
                        citizenRequestExperienceCertificate.CitizenRequestId = citizenRequests.Id;
                        citizenRequestExperienceCertificate.AcquisitionDate = citizenRequestExperienceCertificates.AcquisitionDate;
                        citizenRequestExperienceCertificate.CertificateName = citizenRequestExperienceCertificates.CertificateName;
                        citizenRequestExperienceCertificate.IssuingEntityName = citizenRequestExperienceCertificates.IssuingEntityName;
                        await _repository.AddAsync<CitizenRequestExperienceCertificate, Guid>(citizenRequestExperienceCertificate);
                        response = await _repository.SaveChangesAsync();
                        if (!response.IsDone) return response;

                        foreach (var item in citizenRequestExperienceCertificates.CitizenRequestExperienceCertificateFiles)
                        {
                            await _repository.AddAsync<CitizenRequestExperienceCertificateFile, Guid>(new CitizenRequestExperienceCertificateFile()
                            {
                                CitizenRequestExperienceCertificateId = citizenRequestExperienceCertificate.Id,
                                Id = Guid.NewGuid(),
                                ExperienceCertificateFileId = item.ExperienceCertificateFileId
                            });
                            response = await _repository.SaveChangesAsync();
                            if (!response.IsDone) return response;
                        }
                    }
                }

                #endregion

                if (citizenRequestCreateUpdateDto.IsSend & citizenRequests.StatusId == CitizenRequestStatus.Draft)
                    await ExecuteAction(CitizenRequestAction.Send, citizenRequests.Id, null);
                if (citizenRequestCreateUpdateDto.IsSend & citizenRequests.StatusId == CitizenRequestStatus.SentForEdit)
                    await ExecuteAction(CitizenRequestAction.SendFromEdit, citizenRequests.Id, null);

                return response;

            }
        }

        public async Task<BaseResponse> ExecuteAction(CitizenRequestAction citizenRequestAction, Guid requestId, string? notes)
        {
            Guid referenceId = Guid.Empty;
            _jwtReader.GetReferenceId(ref referenceId);
            var citizenRequests = (await _repository.GetQueryable<CitizenRequest, Guid>())
                  .FirstOrDefault(x => x.Id == requestId);

            if (citizenRequests is null)
                return new BaseResponse() { IsDone = false, Message = "Not Found" };

            BaseResponse saveResponse = new BaseResponse() { IsDone = false, Message = "Not Found" };
            switch (citizenRequestAction)
            {
                case CitizenRequestAction.Send:
                    citizenRequests.StatusId = CitizenRequestStatus.WaitingForCertify;
                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم اضافة طلب جديد بالانضمام كمحكم
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم اضافة طلب جديد بالانضمام كمصلح
                            break;
                    }
                    break;
                case CitizenRequestAction.SendFromEdit:
                    citizenRequests.StatusId = CitizenRequestStatus.SentFromEdit;
                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        CitizenRequestNote citizenRequestNote = new CitizenRequestNote();
                        citizenRequestNote.Id = Guid.NewGuid();
                        citizenRequestNote.CitizenRequestId = requestId;
                        citizenRequestNote.Notes = notes;
                        citizenRequestNote.NoteDateTime = DateTime.Now;
                        await _repository.AddAsync<CitizenRequestNote, Guid>(citizenRequestNote);
                    }
                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم تعديل طلب بالانضمام كمحكم
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم تعديل طلب بالانضمام كمصلح
                            break;
                    }
                    break;
                case CitizenRequestAction.Reject:
                    citizenRequests.StatusId = CitizenRequestStatus.Rejected;
                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم رفض طلب بالانضمام كمحكم
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم رفض طلب بالانضمام كمصلح
                            break;
                    }
                    break;
                case CitizenRequestAction.CertifiedWithFees:
                    citizenRequests.StatusId = CitizenRequestStatus.WaitingForMembershipFeesToBePaid;
                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم إعتماد طلب بالانضمام كمحكم مع وجود رسوم
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم إعتماد طلب بالانضمام كمصلح مع وجود رسوم
                            break;
                    }
                    break;
                case CitizenRequestAction.CertifiedWithOutFees:
                    citizenRequests.StatusId = CitizenRequestStatus.Certified;
                    var citizen = (await _repository.GetQueryable<Citizen, Guid>())
                         .FirstOrDefault(x => x.Id == citizenRequests.CitizenId);
                    if (citizen != null)
                    {
                        if (citizen is not null)
                        {
                            switch (citizenRequests.CitizenRequestTypeId)
                            {
                                case CitizenRequestType.Reformer:
                                    citizen.IsReformer = true;
                                    citizen.ReformerStatusId = ReformerStatus.Certified;
                                    break;
                                case CitizenRequestType.Arbitrator:
                                    citizen.IsArbitrator = true;
                                    break;
                            }
                        }
                    }
                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم إعتماد طلب بالانضمام كمحكم مع عدم وجود رسوم
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم إعتماد طلب بالانضمام كمصلح مع عدم وجود رسوم
                            break;
                    }
                    break;
                case CitizenRequestAction.SendForEdit:
                    citizenRequests.StatusId = CitizenRequestStatus.SentForEdit;
                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        CitizenRequestNote newCitizenRequestNote = new CitizenRequestNote();
                        newCitizenRequestNote.Id = Guid.NewGuid();
                        newCitizenRequestNote.CitizenRequestId = requestId;
                        newCitizenRequestNote.Notes = notes;
                        newCitizenRequestNote.NoteDateTime = DateTime.Now;
                        await _repository.AddAsync<CitizenRequestNote, Guid>(newCitizenRequestNote);
                    }

                    saveResponse = await _repository.SaveChangesAsync();
                    if (!saveResponse.IsDone) return saveResponse;
                    switch (citizenRequests.CitizenRequestTypeId)
                    {
                        case CitizenRequestType.Arbitrator:
                            // Send Notification
                            // تم مراجعة طلب بالانضمام كمحكم مع وجود واعادته للتعديل لوجود ملاحظات
                            break;
                        case CitizenRequestType.Reformer:
                            // Send Notification
                            // تم مراجعة طلب بالانضمام كمصلح مع وجود واعادته للتعديل لوجود ملاحظات
                            break;
                    }
                    break;
            }
            return saveResponse;
        }
    }
}
