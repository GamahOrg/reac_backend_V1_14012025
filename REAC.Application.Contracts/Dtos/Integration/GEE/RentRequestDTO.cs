using REAC.Application.Contracts.Dtos.Integration.Party;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.GEE
{
    public class RentRequestDTO
    {
        public PartyDTO FirstPartyDto { get; set; }
        public PartyDTO SecondPartyDto { get; set; }
        public string ContractKey { get; set; }//رقم عقد الايجار
        [MaxLength(4)]
        public string VersionNumber { get; set; } = null!; // رقم النسخة
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; }
        public string ContractAddress { get; set; }
        public Guid Id { get; set; }
    }
}
