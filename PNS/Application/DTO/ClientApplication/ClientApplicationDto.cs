// File Path: Application/DTO/ClientApplication/ClientApplicationDto.cs
using System;

namespace Application.DTO.ClientApplication
{
    public class ClientApplicationDto
    {
        public Guid Id { get; set; }
        public required string AppId { get; set; }
        public required string Key { get; set; }
        public required string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Logo { get; set; }
        public string? SmsSenderName { get; set; }
        public string? SmsSenderNumber { get; set; }
    }
}