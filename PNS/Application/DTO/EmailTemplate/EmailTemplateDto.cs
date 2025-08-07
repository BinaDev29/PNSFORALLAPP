using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Application.DTO.EmailTemplate
{
    public class EmailTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
