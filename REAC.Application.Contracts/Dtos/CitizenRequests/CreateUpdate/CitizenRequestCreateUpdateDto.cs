using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;

namespace REAC.Application.Contracts.Dtos.CitizenRequests
{
    public class CitizenRequestCreateUpdateDto : BaseEntity<Guid>
    {
        //توع الطلب
        public CitizenRequestType CitizenRequestTypeId { get; set; }
        public Guid? AreaId { get; set; }
        public bool IsVolunteerApproval { get; set; }
        public bool IsSend { get; set; }

        public string AcademicDegreeName { get; set; } = null!;
        public int? ExperienceYears { get; set; }
        public Guid? ArbitratorClassId { get; set; }

        public string? Brief { get; set; }
        public virtual ICollection<CitizenRequestFreelanceWorkDocumentFileCreateUpdateDto> CitizenRequestFreelanceWorkDocumentFiles { get; set; } = new List<CitizenRequestFreelanceWorkDocumentFileCreateUpdateDto>();
        public virtual ICollection<CitizenRequestAcademicDegreeFileCreateUpdateDto> CitizenRequestAcademicDegreeFiles { get; set; } = new List<CitizenRequestAcademicDegreeFileCreateUpdateDto>();
        public virtual ICollection<CitizenRequestExperienceCertificateCreateUpdateDto> CitizenRequestExperienceCertificates { get; set; } = new List<CitizenRequestExperienceCertificateCreateUpdateDto>();
    }
}
