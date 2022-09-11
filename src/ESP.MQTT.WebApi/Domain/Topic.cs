using ESP.MQTT.WebApi.Domain.Abstractions;

namespace ESP.MQTT.WebApi.Domain;

public class Topic : SimpleId<long>
{
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ChangeDate { get; set; }
}