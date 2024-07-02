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
    public class WafiCreateUpdateDto : BaseEntity<Guid>
    {
        public Guid WafiContractAttachmentId { get; set; }
        [MaxLength(20)]
        public string ContractKey { get; set; }//رقم عقد وافي
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; } // صفة المدعي
        public Guid? LeaseContractFileId { get; set; }
    }
}
