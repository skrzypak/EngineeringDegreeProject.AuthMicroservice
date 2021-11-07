using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Models.Dto
{
    public class ChangePassword
    {
        [MaxLength(64), MinLength(8)]
        public string CurrentPassword { get; set; }
        [MaxLength(64), MinLength(8)]
        public string NewPassword { get; set; }
        [MaxLength(64), MinLength(8)]
        public string ConfirmNewPassword { get; set; }
    }
}
