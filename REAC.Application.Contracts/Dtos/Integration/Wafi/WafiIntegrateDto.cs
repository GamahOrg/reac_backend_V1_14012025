using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.Wafi
{
    public class WafiIntegrateDto
    {
        public WafiFirstPartyDto FirstPartyDto { get; set; }
        public WafiSecondPartyDto SecondPartyDto { get; set; }
        public WafiThirdPartyDto ThirdPartyDto { get; set; }
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; }
        public string ContractAddress { get; set; }
    }
}
