using Core.GenericRepository.Shared.BaseEntities;
using Microsoft.AspNetCore.Http;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class RentCreateUpdateDto : BaseEntity<Guid>
    {
        public Guid? LeaseContractFileId { get; set; }
        [MaxLength(12)]
        public string ContractKey { get; set; }//رقم عقد الايجار
        [MaxLength(4)]
        public string VersionNumber { get; set; } = null!; // رقم النسخة
        public string FirstPartyCapacity { get; set; } = null!; // صفة المدعي
        public Guid ContractAttachmentId { get; set; }  // عقد ايجار محل الدعوة
        public IFormFile? IdentityAttachment { get; set; } // صورة الهوية PENDING

    }
}
