using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Models.Dto
{
    public class Password
    {
        [MaxLength(64), MinLength(8)]
        public string New { get; set; }
        [MaxLength(64), MinLength(8)]
        public string Confirm { get; set; }
    }
}
