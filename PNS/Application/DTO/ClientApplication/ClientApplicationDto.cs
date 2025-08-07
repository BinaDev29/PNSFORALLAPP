using System;

namespace Application.DTO.ClientApplication
{
    public class ClientApplicationDto
    {
        public Guid Id { get; set; }
        public required string AppId { get; set; } // 'required' ታክሏል
        public required string Name { get; set; } // 'required' ታክሏል
        public bool IsActive { get; set; }
    }
}