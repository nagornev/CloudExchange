using CloudExchange.Domain.Aggregates;
using DDD.Specifications;
using System.Linq.Expressions;

namespace CloudExchange.Domain.Specifications
{
    public class DescriptorByConfirmedSpecification : Specification<DescriptorAggregate>
    {
        private readonly bool _confirmed;

        public DescriptorByConfirmedSpecification(bool confirmed)
        {
            _confirmed = confirmed;
        }

        public override Expression<Func<DescriptorAggregate, bool>> ToExpression()
        {
            return x => x.Confirmed == _confirmed;
        }
    }
}
