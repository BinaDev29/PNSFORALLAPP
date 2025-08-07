// CreateClientApplicationDto.cs
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
        public required string Description { get; set; }
        [Required]
        public required string Slogan { get; set; }
        [Required]
        public required string Logo { get; set; }
        [Required]
        public required string Email { get; set; }
    }
}