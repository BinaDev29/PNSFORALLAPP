using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Priority
{
    public class UpdatePriorityDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Description { get; set; } // 'Name' ወደ 'Description' ተቀይሯል
        public int Level { get; set; } // 'Level' ተጨምሯል
    }
}