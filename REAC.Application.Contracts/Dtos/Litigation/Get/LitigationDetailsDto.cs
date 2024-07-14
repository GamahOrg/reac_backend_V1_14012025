using Core.GenericRepository.Shared.BaseEntities;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Get
{
    public class LitigationDetailsDto : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? RequestCode { get; set; }
        public Guid? CityId { get; set; }
        public Guid? AreaId { get; set; }
        public string? ContractAddress { get; set; }
        public bool? IsFinancialLitigation { get; set; }
        public Guid? LitigationRequestAmountId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Fees { get; set; }
        public Guid ContractId { get; set; }
        public LitigationRequestContractType LitigationRequestContractTypeId { get; set; }
        public LitigationRequestCategory LitigationRequestCategoryId { get; set; }
        public LitigationRequestOwnerType LitigationRequestOwnerTypeId { get; set; }
        public List<LitigationRequestGroundCreateUpdateDto> References { get; set; } 
        public List<LitigationRequestRequestCreateUpdateDto> Requests { get; set; }
        public List<ExemptionReductionCategoryDto> ExemptionReductionCategory { get; set; }

    }
}
