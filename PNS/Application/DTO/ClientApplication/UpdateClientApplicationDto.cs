// File Path: Application/DTO/ClientApplication/UpdateClientApplicationDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ClientApplication
{
    public class UpdateClientApplicationDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string AppId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Logo { get; set; }
        public string? SmsSenderName { get; set; }
        public string? SmsSenderNumber { get; set; }
    }
}