using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.CitizenRequests
{
    public class CitizenRequestShortDataDto : BaseEntity<Guid>
    {
        //توع الطلب
        public string CitizenRequestType { get; set; }

        public string? AreaName { get; set; }

        public string AcademicDegreeName { get; set; } = null!;
        public bool? IsVolunteerApproval { get; set; }

        public int? ExperienceYears { get; set; }

        public string? ArbitratorClassName { get; set; }
        public string StatusName { get; set; } = null!;


    }
}

