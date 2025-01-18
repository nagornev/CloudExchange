using CloudExchange.UseCases.Providers;
using System;

namespace CloudExchange.Infrastructure.Providers
{
    public class TimeProvider : ITimeProvider
    {
        public long NowUnix()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public DateTime NowDateTime()
        {
            return DateTime.Now;
        }
    }
}
