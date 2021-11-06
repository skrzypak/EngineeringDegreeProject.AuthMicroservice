using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Models.Dto.Enterprise
{
    public class EnterpriseDto<TP> : EnterpriseCoreDto
    {
        public int Id { get; set; }
        public virtual ICollection<TP> Persons { get; set; }
    }
}
