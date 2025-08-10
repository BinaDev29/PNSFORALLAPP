// File Path: Application/DTO/ClientApplication/CreateClientApplicationDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ClientApplication
{
    public class CreateClientApplicationDto
    {
        [Required]
        public required string AppId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Slogan { get; set; }

        [Required]
        public required string Logo { get; set; }

        // ይህንን አዲስ property ጨምር
        [Required]
        [EmailAddress]
        public required string SenderEmail { get; set; }
    }
}