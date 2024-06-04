using Microsoft.AspNetCore.Http;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class WafiCreateUpdateDto
    {
        public Guid WafiContractId { get; set; }
        public IFormFile WafiContractAttachment { get; set; }
        public LitigationRequestOwnerType LitigationRequestOwnerTypeId { get; set; }  // مقدم الدعوي
        public string? DelegationCitizenId { get; set; } // رقم صاحب التفويض
        public string FirstPartyCapacity { get; set; } = null!; // صفة المدعي
        public string? FirstPartyName { get; set; } 
    }
}
