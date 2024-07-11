using Core.GenericRepository.Shared.BaseEntities;
using REAC.Application.Contracts.Dtos.Integration.Party;
using REAC.Application.Contracts.Dtos.Integration.Rent;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Get
{
    public partial class LitigationPartiesDetailsDto : BaseEntity<Guid>
    {
        public PartyDTO? FirstPartyDto { get; set; }
       public PartyDTO? SecondPartyDto { get; set; }
        public string? ContractKey { get; set; }
        [MaxLength(4)]
        public string VersionNumber { get; set; } = null!;
        public string? AssociationName { get; set; }
        public LitigationRequestOwnerType FirstPartyCapacity { get; set; }
        public string? ContractAddress { get; set; }
        public Guid LitigationId { get; set; }

        public  SysFileManagerCreateUpdateDto Contract { get; set; } = null!;

    }
}
