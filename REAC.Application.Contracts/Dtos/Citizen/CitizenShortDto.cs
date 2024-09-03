using Core.GenericRepository.Shared.BaseEntities;

namespace REAC.Application.Contracts.Dtos.Citizen
{
    public class CitizenShortDto : BaseEntity<Guid>
    {
        public string IdentityNumber { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Mobile { get; set; } = null!;

        public string? HomeTelephone { get; set; }

        public string? WorkTelephone { get; set; }

        public string? Fax { get; set; }

        public string Email { get; set; } = null!;

    }

}
