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
        [Route(nameof(GetNotifications))]
        public async Task<IActionResult> GetNotifications()
        {
            Guid citizenId = Guid.NewGuid();
            if (_jwtReader.GetCitizenId(ref citizenId))
            {
                return Ok(await _notificationService.GetNotificationList(citizenId, NotificationActorType.Citizen));
            }

            Guid employeeId = Guid.NewGuid();
            if (_jwtReader.GetEmployeeId(ref employeeId))
            {
                return Ok(await _notificationService.GetNotificationList(employeeId, NotificationActorType.Employee));
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
        [Route(nameof(GetNotificationList))]
        public async Task<IActionResult> GetNotificationList(int pageIndex, int pageSize)
        {
            Guid id = Guid.NewGuid();
            if (_jwtReader.GetEmployeeId(ref id))
            {
                return Ok(await _notificationService.GetNotificationList(pageIndex, pageSize, id, NotificationActorType.Employee));
            }
            if (_jwtReader.GetCitizenId(ref id))
            {
                return Ok(await _notificationService.GetNotificationList(pageIndex, pageSize, id, NotificationActorType.Citizen));
            }
            return BadRequest();
        }
    }
}
