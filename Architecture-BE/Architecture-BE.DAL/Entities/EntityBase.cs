using Architecture_BE.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Architecture_BE.DAL.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public StatusEnum IsDeleted { get; set; }
    }
}
