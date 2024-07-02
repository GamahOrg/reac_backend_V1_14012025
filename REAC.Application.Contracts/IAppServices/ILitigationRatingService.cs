using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Litigation.Rating;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.IAppServices
{
    public interface ILitigationRatingService : IBasicService<LitigationRequest, Guid>
    {
        Task<BaseResponse> GetRatings(Guid litigationId);
        Task<BaseResponse> AddRating(Guid litigationId, Guid CitizenId, int RatingValue);
     //   Task<BaseResponse> UpdateRating(Guid RatingId, int RatingValue);
        Task<BaseResponse> DeleteRating(Guid RatingId);
    }
}
