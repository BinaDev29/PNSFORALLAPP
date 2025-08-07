using System;
using System.Collections.Generic;
using System.Linq; // ToDictionary ለመጠቀም

namespace Application.Responses
{
    public class BaseCommandResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public string Status { get; set; } = string.Empty;

        public BaseCommandResponse() { }

        public BaseCommandResponse(Guid id, string message = "Request successful.", string status = "Success")
        {
            Id = id;
            Success = true;
            Message = message;
            Status = status;
        }

        public BaseCommandResponse(List<string> errors, string message = "An error occurred.", string status = "Failed")
        {
            Success = false;
            Message = message;
            Errors = errors;
            Status = status;
        }
    }
}