using REAC.Application.Contracts.Dtos.Integration.GEE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.UserManage
{
    public class UserRegisterDto
    {
        public string Password { get; set; }
        public Guid AreaId { get; set; }
        public Guid CityId { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string UnitNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
