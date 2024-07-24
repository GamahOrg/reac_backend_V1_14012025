using Core.Security.Attributes;
using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;

namespace REAC.Api.Controllers.Litigation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Litigation)]
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