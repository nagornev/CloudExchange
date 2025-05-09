using CloudExchange.Application.Abstractions.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Providers
{
    public class TestPathProvider : IPathProvider
    {
        public string GetBase()
        {
            return @"E:\Projects\02_WEB\CloudExchange\CloudExchange.Data\";
        }

        public string GetDirectory()
        {
            return @"Files\";
        }
    }
}
