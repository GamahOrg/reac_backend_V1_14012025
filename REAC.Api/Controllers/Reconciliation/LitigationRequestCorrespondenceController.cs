using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Security.Attributes;
using REAC.Application.Contracts.IAppServices.Reconciliation;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Api.Controllers.Reconciliation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Reconciliation)]
[PermissionRequired(Modules.LitigationRequest)]

public class LitigationRequestCorrespondenceController : BaseController<LitigationRequestCorrespondence,
       LitigationRequestCorrespondenceCreateUpdateDto, LitigationRequestCorrespondenceCreateUpdateDto
       , Guid>
{

    public LitigationRequestCorrespondenceController(ILitigationRequestCorrespondenceService litigationRequestCorrespondenceService) :
        base(litigationRequestCorrespondenceService)
    {

    }
}