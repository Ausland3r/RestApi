using Serilog;
using Microsoft.Extensions.Logging;

namespace RestApiClient.Utils
{
    public static class LoggerProvider
    {
        private static ILoggerFactory? _loggerFactory;

        public static ILogger<T> CreateLogger<T>()
        {
            if (_loggerFactory == null)
            {
                InitializeLogger();
            }
            return _loggerFactory!.CreateLogger<T>();
        }

        private static void InitializeLogger()
        {
            string logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(logDirectory, "log-.log"),
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            });
        }
    }
}
