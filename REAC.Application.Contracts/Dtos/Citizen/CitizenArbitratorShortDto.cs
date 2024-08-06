using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.Citizen
{
    public class CitizenArbitratorShortDto : BaseEntity<Guid>
    {
        public string IdentityNumber { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Mobile { get; set; } = null!;

        public string? Email { get; set; } = null!;
        public int? ExperienceYears { get; set; }
        public string? ArbitratorClass { get; set; }
        public int RequestsCount { get; set; }

        public string? ArbitratorStatus { get; set; }

        public DateTime? ArbitratorStatusDate { get; set; }


    }



}
