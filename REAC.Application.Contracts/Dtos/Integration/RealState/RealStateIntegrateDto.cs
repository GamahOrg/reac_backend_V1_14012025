using REAC.Application.Contracts.Dtos.Integration.ReRealStatent;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.RealState
{
    public class RealStateIntegrateDto
    {
        public RealStateFirstPartyDto FirstPartyDto { get; set; }
        public RealStateSecondPartyDto SecondPartyDto { get; set; }
        public string ContractKey { get; set; }//رقم عقد الايجار
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; }
        public string ContractAddress { get; set; }
        public Guid Id { get; set; }
    }
}
