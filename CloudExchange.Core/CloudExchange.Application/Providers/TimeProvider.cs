using CloudExchange.Application.Abstractions.Providers;

namespace CloudExchange.Application.Providers
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
