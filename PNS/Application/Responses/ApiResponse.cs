using System.Collections.Generic;

namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse(T? data, string message, bool success, List<string>? errors = null)
        {
            Data = data;
            Message = message;
            Success = success;
            Errors = errors;
        }

        // Success ምላሽ ለመፍጠር
        public ApiResponse(T data, string message = "Request successful.")
            : this(data, message, true) { }

        // Error ምላሽ ለመፍጠር (List of errors)
        public ApiResponse(List<string> errors, string message = "An error occurred.")
            : this(default, message, false, errors) { }

        // ባዶ ምላሽ ለመፍጠር (message ብቻ)
        public ApiResponse(string message = "Request successful.", bool success = true)
            : this(default, message, success) { }
    }
}