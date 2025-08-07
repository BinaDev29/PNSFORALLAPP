using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DTO.EmailTemplate
{
    public class CreateEmailTemplateDto
    {
        public required string Name { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}