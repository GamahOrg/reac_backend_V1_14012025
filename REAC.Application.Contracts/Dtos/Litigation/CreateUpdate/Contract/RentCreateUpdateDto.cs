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
        //public Guid FirstPartyCitizenId { get; set; }
        //public Guid SecondPartyCitizenId { get; set; }
        public Guid? LeaseContractFileId { get; set; }
        [MaxLength(12)]
        public string ContractKey { get; set; }//رقم عقد الايجار
        [MaxLength(4)]
        public string VersionNumber { get; set; } = null!; // رقم النسخة
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; } // صفة المدعي
        public Guid ContractAttachmentId { get; set; }  // عقد ايجار محل الدعوة
        public IFormFile? IdentityAttachment { get; set; } // صورة الهوية PENDING

        // Updates 
        public Guid? EstablishmentContractId { get; set; } // عقد التاسيس
        [MaxLength(15)]
        public string? AgencyNumber { get; set; } // رقم الوكالة
        public Guid? AgancyAttachmentId { get; set; } // ارفاق الوكالة
        [MaxLength(10)]
        public string? LawLicenseNumber { get; set; }  //رقم رخصة المحاماة
        public DateTime? LawLicenseExpirationDate { get; set; }  //تاريخ انتهاء رخصة المحاماه
        public Guid? LawLicenseAttachmentId { get; set; }

    }
}
