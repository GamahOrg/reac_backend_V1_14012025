using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class RealEstateBrokerageCreateUpdateDto : BaseEntity<Guid>
    {
        public string ContractKey { get; set; }//رقم عقد الوساطة العقارية
        public Guid ContractAttachmentId { get; set; }  // عقد الوساطة العقارية
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; } // صفة المدعي
        public Guid? LeaseContractFileId { get; set; }

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
