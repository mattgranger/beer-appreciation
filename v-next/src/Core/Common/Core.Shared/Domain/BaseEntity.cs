namespace Core.Shared.Domain
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
