using CloudExchange.Domain.Aggregates;
using DDD.Specifications;
using System.Linq.Expressions;

namespace CloudExchange.Domain.Specifications
{
    public class DescriptorByUploadedBeforeSpecification : Specification<DescriptorAggregate>
    {
        private readonly long _timestamp;

        public DescriptorByUploadedBeforeSpecification(long timestamp)
        {
            _timestamp = timestamp;
        }

        public override Expression<Func<DescriptorAggregate, bool>> ToExpression()
        {
            return x => x.CreatedAt < _timestamp;
        }
    }
}
