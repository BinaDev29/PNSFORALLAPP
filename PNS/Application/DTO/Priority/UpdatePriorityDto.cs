// File Path: Application/DTO/Priority/UpdatePriorityDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Priority
{
    public class UpdatePriorityDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required int Level { get; set; }
    }
}