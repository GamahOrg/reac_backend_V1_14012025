using Core.GenericRepository.Shared.BaseEntities;
using Core.Shared.Responses;
using Microsoft.AspNetCore.Http;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class PropertiesCreateUpdateDto : BaseEntity<Guid>
    {
        public string? AssociationName { get; set; } // اسم الجمعية
        public string ContractKey { get; set; } // كود الجمعية
        public Guid AssociationRegistrationCertificateId { get; set; }
        public Guid IncorporationAssociationLetterId { get; set; }
        public Guid AssociationInternalRegulationsId { get; set; }
        public Guid? LeaseContractFileId { get; set; }
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; } // صفة المدعي
        
    }
}
