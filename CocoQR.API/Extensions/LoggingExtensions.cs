using Serilog;
using System.Text;
using static CocoQR.Domain.Constants.FileStorage;

namespace CocoQR.API.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;

            if (env.IsDevelopment())
            {
                builder.Logging.ClearProviders();
                builder.Logging.AddSimpleConsole(options =>
                {
                    options.TimestampFormat = "HH:mm:ss ";
                    options.SingleLine = true;
                });

                Console.OutputEncoding = Encoding.UTF8;
                return builder;
            }

            var basePath = AppContext.BaseDirectory;
            var logPath = Environment.GetEnvironmentVariable(EnvKeys.Logs) ?? "logs";

            var logFolder = Path.Combine(basePath, logPath);

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console();

            if (env.IsStaging())
            {
                loggerConfig
                    .MinimumLevel.Information()

                    .WriteTo.File(
                        Path.Combine(logFolder, "info-.txt"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7,
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                        encoding: Encoding.UTF8
                    )

                    .WriteTo.File(
                        Path.Combine(logFolder, "warning-.txt"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7,
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                        encoding: Encoding.UTF8
                    );
            }
            else
            {
                loggerConfig
                    .MinimumLevel.Warning()
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        Path.Combine(logFolder, "log-.txt"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7,
                        encoding: Encoding.UTF8,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();
            }

            builder.Host.UseSerilog();
            return builder;
        }
    }
}
