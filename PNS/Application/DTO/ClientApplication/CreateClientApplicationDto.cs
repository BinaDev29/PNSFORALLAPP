// File Path: Application/DTO/ClientApplication/CreateClientApplicationDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ClientApplication
{
    public class CreateClientApplicationDto
    {
        [Required]
        public required string AppId { get; set; }

        [Required]
        public required string Key { get; set; } // ⭐ ይህንን ጨምር ⭐

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Slogan { get; set; }

        [Required]
        public required string Logo { get; set; }

        [Required]
        [EmailAddress]
        public required string SenderEmail { get; set; }

        [Required]
        public required string AppPassword { get; set; }
    }
}