using CloudExchange.UseCases.Providers;
using System;
using System.IO;
using ZstdSharp.Unsafe;

namespace CloudExchange.Infrastructure.Providers
{
    public class PathProvider : IPathProvider
    {
        private const string _base = "E:\\Projects\\02_WEB\\CloudExchange\\";

        private const string _directory = "CloudExchange.Data\\Files\\";

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
