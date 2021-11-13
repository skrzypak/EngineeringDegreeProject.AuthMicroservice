using System;

namespace AuthMicroservice.Core.Fluent.Entities.Confirmation
{
    public abstract class Confirmation
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
