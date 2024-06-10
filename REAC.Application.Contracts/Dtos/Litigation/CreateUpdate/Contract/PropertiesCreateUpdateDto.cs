using Microsoft.AspNetCore.Http;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class PropertiesCreateUpdateDto
    {
        public string? AssociationName { get; set; } // اسم الجمعية
        public string? AssociationCode { get; set; } // كود الجمعية
        public IFormFile AssociationRegistrationCertificate { get; set; }
        public IFormFile IncorporationAssociationLetter { get; set; }
        public IFormFile AssociationInternalRegulations { get; set; }
        public string FirstPartyCapacity { get; set; } = null!; // صفة المدعي
        //public LitigationRequestOwnerType LitigationRequestOwnerTypeId { get; set; }  // مقدم الدعوي
        public string? DelegationCitizenId { get; set; } // رقم صاحب التفويض
    }
}
