using System;

namespace Application.DTO.EmailTemplate
{
    public class EmailTemplateDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}