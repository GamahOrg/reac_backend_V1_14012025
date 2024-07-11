using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.GEE
{
    public class MOJRequestDto
    {
        public string Token { get; set; }
        public string AttorneyCode { get; set; }
        public string IdentityNumber { get; set; }
    }

}
