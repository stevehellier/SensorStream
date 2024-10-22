namespace SensorDataFunction;

using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using MQTTnet.Adapter;
using MQTTnet.Exceptions;

public class SensorDataFunction(
    IMessageSender messageSender,
    IOptions<AppConfig.Mqtt> options)
{
    [Function("SensorDataFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sensors/{sensorName}")]
        HttpRequest req,
        string sensorName)
    {
        var content = new SensorData
        {
            DeviceName = sensorName
        };

        _ = float.TryParse(req.Query["temperature"], out content.Temperature);
        _ = float.TryParse(req.Query["humidity"], out content.Humidity);
        _ = float.TryParse(req.Query["heatIndex"], out content.HeatIndex);

        try
        {
            messageSender.Username = options.Value.Username;
            messageSender.Password = options.Value.Password;
            messageSender.Server = options.Value.BrokerUrl;
            messageSender.Port = options.Value.Port;
            messageSender.Topic = $"{sensorName}";

            await messageSender.SendPayloadToBroker(content);
            return new OkResult();
        }
        catch (MqttConnectingFailedException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
        catch (MqttCommunicationException e)
        {
            return new UnprocessableEntityObjectResult(e);
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(e);
        }
    }
}