using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Litigation
{
    public class LitigationReferences
    {
        [MaxLength(100)]
        public string ReferenceTitle { get; set; } //عنوان الاسناد
        public List<Guid> ReferenceAttachments { get; set; }
    }
}
