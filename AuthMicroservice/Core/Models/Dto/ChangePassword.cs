using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Models.Dto
{
    public class ChangePassword : Password
    {
        [MaxLength(64), MinLength(8)]
        public string Current { get; set; }
    }
}
