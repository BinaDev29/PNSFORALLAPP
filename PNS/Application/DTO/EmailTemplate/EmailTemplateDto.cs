// File Path: Application/DTO/EmailTemplate/EmailTemplateDto.cs
using System;

namespace Application.DTO.EmailTemplate
{
    public class EmailTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}