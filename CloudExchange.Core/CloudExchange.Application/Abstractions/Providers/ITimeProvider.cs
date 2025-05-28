﻿namespace CloudExchange.Application.Abstractions.Providers
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Return unix timestamp in seconds.
        /// </summary>
        /// <returns></returns>
        long NowTimestamp();

        /// <summary>
        /// Return current time in DateTime format.
        /// </summary>
        /// <returns></returns>
        DateTime NowDateTime();
    }
}
