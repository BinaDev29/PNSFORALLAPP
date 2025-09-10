// File Path: Domain/Models/ClientApplication.cs
using Domain.Common;

namespace Domain.Models
{
    public class ClientApplication : BaseDomainEntity
    {
        public required string AppId { get; set; }
        public required string Key { get; set; }
        public required string Name { get; set; }
        public required string Slogan { get; set; }
        public required string Logo { get; set; }

        public required string SenderEmail { get; set; }
        public required string AppPassword { get; set; }
    }
}