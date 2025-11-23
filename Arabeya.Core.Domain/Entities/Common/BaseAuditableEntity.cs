namespace Arabeya.Core.Domain.Entities.Common
{
    public class BaseAuditableEntity<TKey>:BaseEntity<TKey> 
        where TKey : IEquatable<TKey>

    {
        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; } 

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
