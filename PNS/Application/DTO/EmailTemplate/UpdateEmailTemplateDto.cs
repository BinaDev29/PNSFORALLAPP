// File Path: Application/DTO/EmailTemplate/UpdateEmailTemplateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailTemplate
{
    public class UpdateEmailTemplateDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}