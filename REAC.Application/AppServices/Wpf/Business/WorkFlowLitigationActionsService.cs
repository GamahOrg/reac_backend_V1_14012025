using AutoMapper;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Application.Contracts.IAppServices.Wpf.System;
using REAC.Application.Contracts.IModuleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.AppServices.Wpf.Business
{
    public class WorkFlowLitigationActionsService : IWorkFlowLitigationActionsService
    {

        

       
        public async Task ExecuteLitigationWorkFlow(Guid litigationId, bool auotAsign)
        {
           // await _bpfService.ExcuteWorkFlow(litigationId, Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"), default);
           //// Guid employee = await _stepService.GetLowestEmployee();
           // if (auotAsign)
           // {
           //     await _bpfService.Assign(new Contracts.Dtos.Wpf.Business.AssignRequestPayload
           //     {
           //         DestinationEmployeeId = employee,
           //         SourceEmployeeId = Guid.Parse("B9BBE3E5-0CA9-44AF-95FA-D2C3A0EC2280"),
           //         RequestId = litigationId

           //     }, default);
           //     //}
           // }
        }
    }
}
