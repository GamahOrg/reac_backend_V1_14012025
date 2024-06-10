using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation
{
    public class LitigationRequests
    {
        [MaxLength(200)]
        public string LitigationPurpose { get; set; }
        public Guid LitigationPurposeFileId { get; set; }
    }
}
