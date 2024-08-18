
using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Wpf.System;
using REAC.EntityFrameworkCore.Entities.Litigation;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> ExcuteWorkFlow(Guid requestId, CancellationToken cancellationToken)
        {
            try
            {
                var litigationRequest = (await _repository.GetQueryable<LitigationRequest, Guid>())
                          .Include(x => x.LitigationRequestCategory)
                          .AsNoTracking()
                          .Where(x => x.Id == requestId).ToList().FirstOrDefault();


                var response = await ExecuteWorkFlowSteps(litigationRequest.LitigationRequestCategory.BpfId, requestId, null, 1, null, cancellationToken);


                litigationRequest.InternalStatusId = (response as DataResponse<ResponseWorkFlowDto>).Data.InternalStatus.Value;
                litigationRequest.ExternalStatusId = (response as DataResponse<ResponseWorkFlowDto>).Data.ExternalStatus.Value;
                litigationRequest.IsSent = true;
                litigationRequest.SentOn = DateTime.Now;
                litigationRequest.IsActive = (response as DataResponse<ResponseWorkFlowDto>).Data.IsActive;
                litigationRequest.IsFinished = (response as DataResponse<ResponseWorkFlowDto>).Data.IsFinished;
                litigationRequest.IsAllowEdit = false;

                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;

                // Send Notification
                // تم إرسال الدعوى

                return response;
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    IsDone = false,
                    Exception = ex.Message + "\n" + ex.ToString(),
                };
            }
        }
    }
}
