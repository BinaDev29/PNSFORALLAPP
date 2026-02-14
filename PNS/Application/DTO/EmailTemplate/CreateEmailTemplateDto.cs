// File Path: Application/DTO/EmailTemplate/CreateEmailTemplateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailTemplate
{
    public class CreateEmailTemplateDto
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required string Subject { get; set; } = string.Empty;
        [Required]
        public required string BodyHtml { get; set; } = string.Empty;
        public string? BodyText { get; set; }
    }
}