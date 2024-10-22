namespace MessageService;

using Interfaces;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

public class MessageSender(ILogger<MessageSender> logger) : IMessageSender
{
    /// <summary>
    /// Username for the MQTT server
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for the MQTT server
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// The MQTT Server/Broker URL
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// MQTT Server port
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// MQTT Topic
    /// </summary>
    public string? Topic { get; set; }

    /// <summary>
    /// Send the Sensor Data payload to the MQTT server
    /// </summary>
    /// <param name="payload"></param>
    public async Task SendPayloadToBroker(object? payload)
    {
        try
        {
            var factory = new MqttFactory();
            using var client = factory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(Server, Port)
                .WithCredentials(Username, Password)
                .WithClientId(Guid.NewGuid().ToString())
                .WithCleanSession()
                .Build();
            logger.LogInformation("connecting to MQTT broker.");
            await client.ConnectAsync(mqttClientOptions);

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"sensors/{Topic?.ToLower()}")
                .WithPayload(jsonPayload)
                .Build();

            logger.LogInformation("sending payload.");
            await client.PublishAsync(message);
            await client.DisconnectAsync();
        }
        catch (Exception e)
        {
            logger.LogCritical(0, e, "Error");
            Console.WriteLine(e);
            throw;
        }
    }
}