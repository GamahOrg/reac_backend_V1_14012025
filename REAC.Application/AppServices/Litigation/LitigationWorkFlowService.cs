using AutoMapper;
using Bpf.Application.ModuleServices;
using Core.Shared.Responses;
using REAC.Application.Contracts;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Litigation;
using REAC.Application.Contracts.Dtos.Wpf.System;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.Wpf.System;
using REAC.Application.Contracts.IModuleServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.AppServices.Litigation
{
    public class LitigationWorkFlowService : ILitigationWorkFlowService
    {
        private readonly ILitigationRequestService _litigationRequestService;
        private readonly IMapper _mapper;
        private readonly IBpfService _bpfService;
        private readonly IWorkFlowStepService _stepService;

        public LitigationWorkFlowService(IMapper mapper,ILitigationRequestService litigationRequestService)
        {
            _litigationRequestService = litigationRequestService;
            _mapper = mapper;
            //_bpfService = bpfService;
            //_stepService = workFlowStepService;
        }
        public async Task<BaseResponse> UpdateLitigationStatusAsync(ResponseWorkFlowDto model, Guid requestId)
        {
            var litigationRquest = await _litigationRequestService.Get(requestId) as DataResponse<LitigationRequest>;
            litigationRquest.Data.InternalStatusId = model.InternalStatus.Value;
            litigationRquest.Data.ExternalStatusId = model.ExternalStatus.Value;
            var litiationCreateUpdateDto = _mapper.Map<LitigationCreateUpdateDto>(litigationRquest.Data);
            if(litiationCreateUpdateDto != null)
            await _litigationRequestService.AddLitigation(litiationCreateUpdateDto);
            //((DataResponse<LitigationRequestCreateUpdateDto>)litigationRquest).Data.ExternalStatusId = (int)model.ExternalStatus;
            //await _litigationRequestService.Add<LitigationRequestCreateUpdateDto>(((DataResponse<LitigationRequestCreateUpdateDto>)litigationRquest).Data);

            return litigationRquest;
        }
        public async Task ExecuteLitigationWorkFlow(Guid litigationId) //Create new interface to implement this logic to avoid cercullar injection
        {
            //await _bpfService.ExcuteWorkFlow(litigationId, Guid.Parse("15d87d0d-3cf4-4969-9c7e-933aeea0e5f5"), default);
            //Guid employee = await _stepService.GetLowestEmployee();
            //await _bpfService.Assign(new Contracts.Dtos.Wpf.Business.AssignRequestPayload
            //{
            //    DestinationEmployeeId = employee,
            //    SourceEmployeeId = Guid.Parse("B9BBE3E5-0CA9-44AF-95FA-D2C3A0EC2280"),
            //    RequestId = litigationId

            //}, default);
        }
    }
}
