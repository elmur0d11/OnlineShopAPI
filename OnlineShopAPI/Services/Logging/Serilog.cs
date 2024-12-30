using Serilog;
using Serilog.Formatting.Json;

namespace OnlineShopAPIFull.Services.Logging
{
    public static class Serilog
    {
        public static void SerilogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
