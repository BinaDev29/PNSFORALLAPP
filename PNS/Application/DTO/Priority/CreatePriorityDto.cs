// File Path: Application/DTO/Priority/CreatePriorityDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Priority
{
    public class CreatePriorityDto
    {
        [Required]
        public required string Description { get; set; }
        [Required]
        public required int Level { get; set; }
    }
}