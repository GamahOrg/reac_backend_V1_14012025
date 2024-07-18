using Core.Shared.Responses;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.List;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace REAC.Application.Contracts.IAppServices.Litigation
{
    public interface ILitigationRequestService : IBasicService<LitigationRequest, Guid>
    {
        Task<BaseResponse> Add(LitigationCreateUpdateDto model);
        Task<BaseResponse> List(int pageIndex, int pageSize, string? searchKey, LitigationListDto model);
        Task<BaseResponse> AuditngList(int pageIndex, int pageSize, string? searchKey, AuditingListDto model);
        Task<BaseResponse> Get(Guid litigationId);
        Task<BaseResponse> GetLitigationPartiesDetails(Guid litigationId);
        Task<BaseResponse> GetLitigationDetails(Guid litigationId);
        Task<BaseResponse> AddLitigation(LitigationCreateUpdateDto model);
        Task<BaseResponse> CreateLitigationRequest(LitigationRequestDto litigationRequestDto);


        #region pagination
        public class PaginatedList<T>
        {
            public List<T> Items { get; private set; }
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Items = items;
            }

            public bool HasPreviousPage => PageIndex > 1;
            public bool HasNextPage => PageIndex < TotalPages;

            public static async Task<PaginatedList<T>> CreateAsync(IList<T> source, int pageIndex, int pageSize)
            {
                var count = source.Count();
                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }
        #endregion
    }
}
