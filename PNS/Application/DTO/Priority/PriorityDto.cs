// File Path: Application/DTO/Priority/PriorityDto.cs
using System;

namespace Application.DTO.Priority
{
    public class PriorityDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
    }
}