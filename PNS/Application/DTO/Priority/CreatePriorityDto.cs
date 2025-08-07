using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Priority
{
    public class CreatePriorityDto
    {
        public required string Description { get; set; }
        public required int Level { get; set; }
    }
}