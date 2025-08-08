using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailTemplate
{
    public class UpdateEmailTemplateDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string BodyHtml { get; set; } // ተስተካክሏል
        public string? BodyText { get; set; } // እንደ አማራጭ ተጨምሯል
    }
}