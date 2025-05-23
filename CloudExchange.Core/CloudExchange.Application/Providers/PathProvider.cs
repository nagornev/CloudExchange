﻿using CloudExchange.Application.Abstractions.Providers;

namespace CloudExchange.Application.Providers
{
    public class PathProvider : IPathProvider
    {
        private const string _base = "/app/";

        private const string _directory = "files/";

        public string GetBase()
        {
            return _base;
        }

        public string GetDirectory()
        {
            return _directory;
        }
    }
}
