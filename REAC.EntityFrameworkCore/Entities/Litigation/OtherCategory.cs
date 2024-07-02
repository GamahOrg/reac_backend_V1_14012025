using Core.GenericRepository.Shared.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Litigation
{
    public class OtherCategory : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }

    }
}