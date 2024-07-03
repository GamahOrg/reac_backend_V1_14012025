using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.IAppServices.Wpf.Business
{
    public interface IWorkFlowLitigationActionsService
    {
         Task ExecuteLitigationWorkFlow(Guid litigationId,bool auotAsign) ; 
    
    }
}
