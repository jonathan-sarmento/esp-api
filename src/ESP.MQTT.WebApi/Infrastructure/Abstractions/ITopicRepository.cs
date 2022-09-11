using System.Linq.Expressions;
using ESP.MQTT.WebApi.Domain;

namespace ESP.MQTT.WebApi.Infrastructure.Abstractions;

public interface ITopicRepository
{
    Task<IList<Topic>> GetAllAsync(Expression<Func<Topic, bool>> expression = null);
    Task<Topic> GetFirstAsync(Expression<Func<Topic, bool>> expression);
    Task<Topic> GetByIdAsync(long id);
}