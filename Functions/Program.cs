using Interfaces;
using MessageService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorDataFunction;

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddTransient<IMessageSender, MessageSender>();
        services.Configure<AppConfig.Mqtt>(config.GetSection("Mqtt"));
    })
    .Build();

host.Run();