namespace Interfaces;

public interface IMessageSender
{

    /// <summary>
    /// Username for the MQTT server
    /// </summary>
    string? Username { get; set; }

    /// <summary>
    /// Password for the MQTT server
    /// </summary>
    string? Password { get; set; }

    /// <summary>
    /// The MQTT Server/Broker URL
    /// </summary>
    string? Server { get; set; }

    /// <summary>
    /// MQTT Server port
    /// </summary>
    int? Port { get; set; }

    /// <summary>
    /// MQTT Topic
    /// </summary>
    string? Topic { get; set; }

    /// <summary>
    /// Send the Sensor Data payload to the MQTT server
    /// </summary>
    /// <param name="payload"></param>
    Task SendPayloadToBroker(object? payload);
}