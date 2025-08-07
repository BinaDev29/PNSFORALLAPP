using System;
using System.Collections.Generic;

namespace Application.Responses
{
    // ከ BaseCommandResponse በመውረስ የኮድ ድግግሞሽን እንቀንሳለን
    public class AuthenticationCommandResponse : BaseCommandResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public double ExpireIn { get; set; }
    }
}