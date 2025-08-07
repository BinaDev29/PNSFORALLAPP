using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EmailTemplate
{
    public class UpdateEmailTemplateDto
    {
        public Guid Id { get; set; } // ይህ ንብረት መኖሩን አረጋግጥ
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string Message { get; set; }
    }
}