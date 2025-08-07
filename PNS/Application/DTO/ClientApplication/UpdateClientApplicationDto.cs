using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ClientApplication
{
    public class UpdateClientApplicationDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}