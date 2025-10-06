using CloudExchange.Domain.Aggregates;
using DDD.Specifications;
using System.Linq.Expressions;

namespace CloudExchange.Domain.Specifications
{
    public class DescriptorByIdSpecification : Specification<DescriptorAggregate>
    {
        private readonly Guid _id;

        public DescriptorByIdSpecification(Guid id)
        {
            _id = id;
        }

        public override Expression<Func<DescriptorAggregate, bool>> ToExpression()
        {
            return x => x.Id == _id;
        }
    }
}
