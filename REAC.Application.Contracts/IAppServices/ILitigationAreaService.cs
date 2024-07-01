using Core.Shared.Responses;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.IAppServices
{
    public interface ILitigationAreaService : IBasicService<LitigationArea, Guid>
    {
       
    }
}
