using Core.GenericRepository.Shared.BaseEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Litigation
{
    public class LitigationCreateUpdateDto : BaseEntity<Guid>
    {
        public string? RequestCode { get; set; } // from system
        [MaxLength(100)]
        public string? Title { get; set; } // عنوان الدعوي
        [MaxLength(1000)]
        public string? Description { get; set; } // وصف الدعوي
        public bool? IsFinancialLitigation { get; set; } // نزاع مالي ام لا 
        public Guid LitigationRequestAmountId { get; set; }// مبلغ المنازعه
        //public Guid RegionId { get; set; }
        //public Guid CityId { get; set; }
        public List<LitigationRequests> Requests { get; set; }
        public List<LitigationReferences> References { get; set; }
        public Guid ContractId { get; set; }// contract primary key
        public decimal? Fees { get; set; } // from system
        public LitigationRequestCategory LitigationRequestCategoryId { get; set; }
        public Guid ExemptionReductionCategoryId { get; set; }
        public LitigationRequestContractType LitigationRequestContractType { get; set; } // front send type here
        public LitigationRequestOwnerType? LitigationRequestOwnerTypeId { get; set; }// front send type here  from integration and check identity from system 

        // Updates 
        public Guid? EstablishmentContractId { get; set; } // عقد التاسيس
        public string? AgencyNumber { get; set; } // رقم الوكالة
        public Guid? AgancyAttachmentId { get; set; } // ارفاق الوكالة
        public string? LawLicenseNumber { get; set; }  //رقم رخصة المحاماة
        public DateTime? LawLicenseExpirationDate { get; set; }  //تاريخ انتهاء رخصة المحاماه
        public Guid LawLicenseAttachmentId { get; set; }

    }

}
