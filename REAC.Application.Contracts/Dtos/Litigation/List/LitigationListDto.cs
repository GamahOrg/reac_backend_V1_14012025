using Core.GenericRepository.Shared.BaseEntities;
using REAC.Domain.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace REAC.Application.Contracts.Dtos.Litigation.List
{
    public class LitigationListDto : BaseEntity<Guid>
    {
        public string? RequestCode { get; set; }
        [MaxLength(100)]
        public DateTime? CreatedOn { get; set; }
        public string? SecondPartyName { get; set; }
        public RequestStatus? InternalStatusId { get; set; }
        public RequestStatus? ExternalStatusId { get; set; }
        public LitigationRequestContractType? LitigationRequestContractTypeId { get; set; }
        public string? LitigationRequestContractType { get; set; }

    }
}
