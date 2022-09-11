using System.Text;
using ESP.MQTT.WebApi.Services.Abstractions;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Serilog;

namespace ESP.MQTT.WebApi.Services;

public class MqttHandler : IMqttHandler
{
    public Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var sender = args.ApplicationMessage.Topic.Split("/")[1];
        var payload = args.ApplicationMessage.Payload == null ? null : Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
        Log.Logger.Information("TimeStamp: {TimeStamp} -- Message: ClientId = {ClientId}, Sender = {Sender},Topic = {Topic}, Payload = {Payload}, QoS = {Qos}",
            DateTime.Now,
            args.ClientId,
            sender,
            args.ApplicationMessage.Topic,
            payload,
            args.ApplicationMessage.QualityOfServiceLevel);

        return Task.CompletedTask;
    }
    
    public Task OnConnectedAsync(MqttClientConnectedEventArgs args)
    {
        Log.Logger.Information("Successfully connected");

        return Task.CompletedTask;
    }
    
    public Task OnConnectingFailedAsync(ConnectingFailedEventArgs args)
    {
        Log.Logger.Warning("Couldn't connect to broker");
            
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs args)
    {
        Log.Logger.Information("Successfully disconnected");

        return Task.CompletedTask;
    }
}