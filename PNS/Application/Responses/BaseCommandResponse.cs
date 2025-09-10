// File Path: Application/Responses/BaseCommandResponse.cs

using System;
using System.Collections.Generic;

namespace Application.Responses
{
    public class BaseCommandResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        // FIX: Add the 'Errors' property to the class.
        public List<string> Errors { get; set; } = new List<string>();
    }
}