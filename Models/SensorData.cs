namespace Models;

using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class SensorData
{
    [JsonProperty("deviceName", NullValueHandling = NullValueHandling.Ignore)] [JsonPropertyName("deviceName")]
    public string DeviceName = string.Empty;

    [JsonProperty("heatIndex", NullValueHandling = NullValueHandling.Ignore)] [JsonPropertyName("heatIndex")]
    public float HeatIndex;

    [JsonProperty("humidity", NullValueHandling = NullValueHandling.Ignore)] [JsonPropertyName("humidity")]
    public float Humidity;

    [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)] [JsonPropertyName("temperature")]
    public float Temperature;
}