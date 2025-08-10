// File Path: Application/DTO/EmailTemplate/CreateEmailTemplateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailTemplate
{
    public class CreateEmailTemplateDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}