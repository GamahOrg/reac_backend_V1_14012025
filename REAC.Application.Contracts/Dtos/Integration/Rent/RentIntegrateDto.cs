using REAC.Application.Contracts.Dtos.Integration.Party;
using REAC.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace REAC.Application.Contracts.Dtos.Integration.Rent
{
    public class RentIntegrateDto
    {
        public PartyDTO FirstPartyDto { get; set; }
        public PartyDTO SecondPartyDto { get; set; }
        public string Key { get; set; }//رقم عقد الايجار
        [MaxLength(4)]
        public string SubKey { get; set; } = null!; // رقم النسخة
        public LitigationRequestOwnerType LitigationRequestOwnerType { get; set; }
        public string ContractAddress { get; set; }
        public Guid Id { get; set; }
    }
}
