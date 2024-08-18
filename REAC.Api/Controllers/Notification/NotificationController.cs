using Microsoft.AspNetCore.Mvc;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Notification;
using REAC.Domain.Shared.Constants;
using REAC.Domain.Shared.Enum;

namespace REAC.Api.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = APISections.Public)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IJwtReader _jwtReader;

        public NotificationController(INotificationService notificationService, IJwtReader jwtReader)
        {
            _notificationService = notificationService;
            _jwtReader = jwtReader;
        }

        [HttpGet]
        [Route(nameof(GetNotificationListForPortal))]
        public async Task<IActionResult> GetNotificationListForPortal()
        {
            Guid citizenId = Guid.NewGuid();
            if (_jwtReader.GetCitizenId(ref citizenId))
            {
                return Ok(await _notificationService.GetNotificationListForPortal(citizenId, NotificationActorType.Employee));
            }
            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(GetNotificationListForBackOffice))]
        public async Task<IActionResult> GetNotificationListForBackOffice()
        {
            Guid citizenId = Guid.NewGuid();
            if (_jwtReader.GetCitizenId(ref citizenId))
            {
                return Ok(await _notificationService.GetNotificationListForBackOffice(citizenId, NotificationActorType.Employee));
            }
            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(MarkNotificationViewed) + "/{notificationId}")]
        public async Task<IActionResult> MarkNotificationViewed(Guid notificationId)
        {
            return Ok(await _notificationService.MarkNotificationViewed(notificationId));
        }

        [HttpGet]
        [Route(nameof(GetNotificationListForPortal))]
        public async Task<IActionResult> GetNotificationListForPortal(int pageIndex, int pageSize)
        {
            Guid referenceId = Guid.NewGuid();
            if (_jwtReader.GetReferenceId(ref referenceId))
            {
                return Ok(await _notificationService.GetNotificationListForPortal(pageIndex, pageSize, referenceId, NotificationActorType.Employee));
            }
            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(GetNotificationListForBackOffice))]
        public async Task<IActionResult> GetNotificationListForBackOffice(int pageIndex, int pageSize)
        {
            Guid referenceId = Guid.NewGuid();
            if (_jwtReader.GetReferenceId(ref referenceId))
            {
                return Ok(await _notificationService.GetNotificationListForBackOffice(pageIndex, pageSize, referenceId, NotificationActorType.Employee));
            }
            return BadRequest();
        }
    }
}
