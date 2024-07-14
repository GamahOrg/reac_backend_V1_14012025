using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.EntityFrameworkCore.Entities.Integration
{
    public class NafathTransaction : BaseEntity<Guid>
    {
        public string NationalId { get; set; }
        public string TransactionId { get; set; }
        public string RandomNumber { get; set; }
        public DateTime RequestExpired { get; set; }
    }
}
