using CloudExchange.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Persistence
{
    public static class DescriptorContextSeeder
    {
        public static async Task SeedAsync(DescriptorContext context, CancellationToken cancellation = default)
        {
            await context.Database.MigrateAsync(cancellation);
        }
    }
}
