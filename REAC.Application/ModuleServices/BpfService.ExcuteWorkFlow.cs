
using Core.Shared.Responses;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> ExcuteWorkFlow(Guid requestId, Guid WorkflowId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await ExecuteWorkFlowSteps(WorkflowId, requestId, null, 1, null, cancellationToken);

                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;

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
