using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Priority
{
    public class PriorityDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
    }
}
