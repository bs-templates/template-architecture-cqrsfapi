namespace BAYSOFT.Core.Domain.Entities
{
    public class DomainEntity<TIdType> : DomainEntity
    {
        public TIdType Id { get; set; }
    }

    public class DomainEntity
    {

    }
}
