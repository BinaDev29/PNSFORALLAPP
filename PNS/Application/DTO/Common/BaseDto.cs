using System;

namespace ECX.HR.Application.DTOs.Common
{
    public abstract class BaseDto
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}