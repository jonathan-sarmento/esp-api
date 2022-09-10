using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Serilog;

namespace ESP.MQTT.WebApi;

public static class ServiceCollectionExtensions
{
    public static async Task ConfigureMqttClient(this IServiceCollection services)
    {
        var mqttFactory = new MqttFactory();

        var mqttClient = mqttFactory.CreateManagedMqttClient();
        
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId("testeeee")
            .WithTcpServer("broker.emqx.io", 1883)
            .Build();
        
        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(20))
            .WithClientOptions(mqttClientOptions)
            .Build();

        // Setup message handling before connecting so that queued messages
        // are also handled properly. When there is no event handler attached all
        // received messages get lost.
        mqttClient.ApplicationMessageReceivedAsync += args =>
        {
            var sender = args.ApplicationMessage.Topic.Split("/")[1];
            var payload = args.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(args.ApplicationMessage?.Payload);
            Log.Logger.Information("TimeStamp: {TimeStamp} -- Message: ClientId = {clientId}, Sender = {sender},Topic = {topic}, Payload = {payload}, QoS = {qos}",
                DateTime.Now,
                args.ClientId,
                sender,
                args.ApplicationMessage?.Topic,
                payload,
                args.ApplicationMessage?.QualityOfServiceLevel);

            return Task.CompletedTask;
        };

        mqttClient.ConnectedAsync += args =>
        {
            Log.Logger.Information("Successfully connected");

            return Task.CompletedTask;
        };

        mqttClient.ConnectingFailedAsync += args =>
        {
            Log.Logger.Warning("Couldn't connect to broker");
            
            return Task.CompletedTask;
        };

        mqttClient.DisconnectedAsync += args =>
        {
            Log.Logger.Information("Successfully disconnected");

            return Task.CompletedTask;
        };
        
        try
        {
            //using var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            //await mqttClient.ConnectAsync(mqttClientOptions, timeoutToken.Token);
            await mqttClient.StartAsync(managedMqttClientOptions);
        }
        catch (OperationCanceledException)
        {
            Log.Logger.Warning("Timeout while connecting");
        }

        var topics = new string[] {"3AEDF0045F/maui/led", "3AEDF0045F/esp/touch"};

        await mqttClient.SubscribeAsync(new List<MqttTopicFilter>()
        {
            new MqttTopicFilterBuilder().WithTopic(topics[0]).Build(),
            new MqttTopicFilterBuilder().WithTopic(topics[1]).Build()
        });
        Log.Logger.Information("MQTT client subscribed to topics: {Topic}", topics);
        
    }
}