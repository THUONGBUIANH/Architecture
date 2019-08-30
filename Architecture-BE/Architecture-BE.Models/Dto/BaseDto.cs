using Architecture_BE.Models.Enums;
using System;

namespace Architecture_BE.Models.Dto
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public StatusEnum IsDeleted { get; set; }
    }
}
