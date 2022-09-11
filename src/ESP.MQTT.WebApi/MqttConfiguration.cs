using ESP.MQTT.WebApi.Infrastructure.Abstractions;
using ESP.MQTT.WebApi.Services.Abstractions;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Serilog;

namespace ESP.MQTT.WebApi;

public static class MqttConfiguration
{
    public static async Task ConfigureMqttClient(this IApplicationBuilder app, IServiceProvider provider, ITopicRepository topicRepository)
    {
        var mqttHandler = provider.GetRequiredService<IMqttHandler>();
        
        var mqttFactory = new MqttFactory();

        var mqttClient = mqttFactory.CreateManagedMqttClient();
        
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("broker.emqx.io", 1883)
            .Build();
        
        var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(20))
            .WithClientOptions(mqttClientOptions)
            .Build();

        mqttClient.ApplicationMessageReceivedAsync += mqttHandler.OnApplicationMessageReceivedAsync;

        mqttClient.ConnectedAsync += mqttHandler.OnConnectedAsync;

        mqttClient.ConnectingFailedAsync += mqttHandler.OnConnectingFailedAsync;

        mqttClient.DisconnectedAsync += mqttHandler.OnDisconnectedAsync;
        
        try
        {
            await mqttClient.StartAsync(managedMqttClientOptions);
        }
        catch (OperationCanceledException)
        {
            Log.Logger.Warning("Timeout while connecting");
        }

        var topics = await topicRepository.GetAllAsync();

        var mqttTopicFilterList = topics.Any()
            ? topics.Select(topic => new MqttTopicFilterBuilder().WithTopic(topic.Description).Build()).ToList()
            : new List<MqttTopicFilter>()
            {
                new MqttTopicFilterBuilder().WithTopic("3AEDF0045F/maui/led").Build(),
                new MqttTopicFilterBuilder().WithTopic("3AEDF0045F/esp/touch").Build()
            }.ToList();
        
        await mqttClient.SubscribeAsync(mqttTopicFilterList);
        Log.Logger.Information("MQTT client subscribed to topics: {Topic}", mqttTopicFilterList.Select(x => x.Topic));
        
    }
}