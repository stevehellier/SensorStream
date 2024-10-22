namespace SensorDataFunction;

public class AppConfig
{
    public class Mqtt
    {
        public required string BrokerUrl { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}