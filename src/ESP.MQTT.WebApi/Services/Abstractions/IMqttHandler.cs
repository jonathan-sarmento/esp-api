using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace ESP.MQTT.WebApi.Services.Abstractions;

public interface IMqttHandler
{
    Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args);
    Task OnConnectedAsync(MqttClientConnectedEventArgs args);
    Task OnConnectingFailedAsync(ConnectingFailedEventArgs args);
    Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs args);
}