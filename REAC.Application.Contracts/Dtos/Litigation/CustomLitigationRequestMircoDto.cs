
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Application.Contracts.Dtos.Litigation
{
    public class CustomLitigationRequestMircoDto : LitigationRequestMircoDto
    {
        public Guid RequestId { get; set; }

        public DateTime? ResponseOn { get; set; }
        public LitigationRequestReformerAssigningRequestStatus? Response { get; set; }
    }
}
