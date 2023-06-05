using System.Reflection;
using System.Linq;
using NLog.Extensions.Logging;

namespace AuthServer.Web
{
    public class Program
    {
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Выводим информацию о загруженных сборках
                ShowDependencies();
                // Меняем провайдер логирования на Nlog
                СonfigureLogging(builder);
                // Добавляем контроллеры
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
     

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Маппим контроллеры
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        private static void ShowDependencies()
        {
            var dependencies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var dependency in dependencies)
            {
                Console.WriteLine($"{dependency.GetName().Name} ({dependency.GetName().Version})");
            }
        }

        private static void СonfigureLogging(WebApplicationBuilder builder)
        {
            builder.Logging
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddNLog();
        }
    }
}