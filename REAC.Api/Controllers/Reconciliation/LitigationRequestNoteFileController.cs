using Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Security.Attributes;
using REAC.Application.Contracts.IAppServices.Reconciliation;
using REAC.Domain.Shared.Constants;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.Domain.Shared;


namespace REAC.Api.Controllers.Reconciliation;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = APISections.Reconciliation)]
[PermissionRequired(Modules.LitigationRequestNoteFile)]

public class LitigationRequestNoteFileController : BaseController<LitigationRequestNoteFile,
       LitigationRequestNoteFileCreateUpdateDto, LitigationRequestNoteFileCreateUpdateDto
       , Guid>
{

    public LitigationRequestNoteFileController(ILitigationRequestNoteFileService litigationRequestNoteFileService) :
        base(litigationRequestNoteFileService)
    {

    }
}