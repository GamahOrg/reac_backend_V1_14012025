using Core.GenericRepository.Shared.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Create.Contract
{
    public class RealEstateBrokerageCreateUpdateDto : BaseEntity<Guid>
    {
        public string ContractKey { get; set; }//رقم عقد الوساطة العقارية
        public Guid ContractAttachmentId { get; set; }  // عقد الوساطة العقارية
        public string? DelegationCitizenId { get; set; } // رقم صاحب التفويض
        public string FirstPartyCapacity { get; set; } = null!; // صفة المدعي
        public Guid? LeaseContractFileId { get; set; } 

    }
}
