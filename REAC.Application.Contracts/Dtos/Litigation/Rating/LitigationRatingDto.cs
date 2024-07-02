using Core.GenericRepository.Shared.BaseEntities;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.Rating
{
    public class LitigationRatingDto : BaseEntity<Guid>
    {
        public Guid LitigationId { get; set; }
        public Guid CitizenId { get; set; }
        public Guid RatingId { get; set; }
        public int RatingValue { get; set; }
        public DateTime RatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int PretenderRating { get; set; }
        public int DefendantRating { get; set; }

        public virtual LitigationRequest Litigation { get; set; }

    }
}
