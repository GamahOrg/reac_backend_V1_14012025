using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Wpf.System
{
    public class AuditNotesDto : BaseEntity<Guid>
    {
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }

        public SysFileManagerCreateUpdateDto AuditFile { get; set; }
    }
}
