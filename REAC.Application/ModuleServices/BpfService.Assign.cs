using Core.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using REAC.Application.Contracts.Dtos.Wpf.Business;
using REAC.EntityFrameworkCore.Entities.System;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;

namespace Bpf.Application.ModuleServices
{
    public partial class BpfService
    {
        public async Task<BaseResponse> Assign(AssignRequestPayload assignRequestPayload, CancellationToken cancellationToken)
        {
            try
            {

                #region SourceEmployee
                var sourceEmployee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                    .Where(x => x.Id == assignRequestPayload.SourceEmployeeId).Single();

                if (sourceEmployee == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Source Employee Not Found"
                    };

                if (!sourceEmployee.AllowAssign)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Not allowed"
                    };
                #endregion

                #region DestinationEmployee
                var destinationEmployee = (await _repository.GetQueryable<SysEmployee, Guid>())?
                    .Where(x => x.Id == assignRequestPayload.DestinationEmployeeId).Single();

                if (destinationEmployee == null)
                {
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "Destination Employee Not Found"
                    };
                }
                #endregion

                #region Processinglogs
                var processinglogs = (await _repository.GetQueryable<WorkFlowProcessingLog, Guid>())?
                    .Where(x => x.RequestId == assignRequestPayload.RequestId
                                            & !x.IsFinished
                                            & x.WorkFlowStep.DepartmentId == destinationEmployee.DepartmentId)
                    .Include(x => x.WorkFlowStep)
                    .ThenInclude(x => x.WorkFlow)
                    .ToList();

                if (processinglogs == null)
                    return new BaseResponse()
                    {
                        IsDone = false,
                        Message = "request Not Found"
                    };

                bool passed = true;
                processinglogs.ForEach(x =>
                {
                    if (x.EmployeeReferenceId.HasValue)
                    {
                        if (sourceEmployee.AllowReAssign)
                            x.EmployeeReferenceId = destinationEmployee?.Id;
                        else
                            passed = false;
                    }
                    else
                        x.EmployeeReferenceId = destinationEmployee?.Id;
                });
                #endregion

                #region SaveChanges
                var repositorySaveResponse = await _repository.SaveChangesAsync(new CancellationToken());
                if (!repositorySaveResponse.IsDone)
                    return repositorySaveResponse;

                if (!passed)
                    return new BaseResponse()
                    {
                        IsDone = true,
                        Message = "employee not allow to reasign"
                    };
                else
                    return new BaseResponse()
                    {
                        IsDone = true
                    };
                #endregion

            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Exception = ex.Message + "\n" + ex.ToString(),
                };
            }
        }
    }
}
