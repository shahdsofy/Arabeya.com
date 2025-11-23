namespace Arabeya.Core.Domain.Entities.Common
{
    public abstract class BaseEntity<T> where T : IEquatable<T>
    {
        public required T Id { get; set; }
    }
}
