using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration
{
    public class RentIntegrationResultDto
    {
        public string FirstPartyIdentityNo { get; set; }
        public string SecondPartyIdentityNo { get; set; }
        public LitigationRequestOwnerType OwnerType { get; set; }
        public string FirstPartyMobile { get; set; }
        public string FirstPartyEmail { get; set; }
        public string FirstPartyBirthDate { get; set; }
        public string SecondPartyMobile { get; set; }
        public string SecondPartyEmail { get; set; }
        public string SecondPartyBirthDate { get; set; }
        public List<string> Delegators { get; set; }
    }

}
