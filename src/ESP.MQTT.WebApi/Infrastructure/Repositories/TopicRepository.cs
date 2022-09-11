using System.Linq.Expressions;
using ESP.MQTT.WebApi.Domain;
using ESP.MQTT.WebApi.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ESP.MQTT.WebApi.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly PostgreDbContext _dbContext;
    
    public TopicRepository(PostgreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Topic>> GetAllAsync(Expression<Func<Topic,bool>> expression = null) =>
        await _dbContext.Set<Topic>().AsNoTracking()
            .Where(expression ?? (_ => true))
            .ToListAsync();

    public async Task<Topic> GetFirstAsync(Expression<Func<Topic,bool>> expression) =>
        await _dbContext.Set<Topic>()
            .FirstAsync(expression);
    
    public async Task<Topic> GetByIdAsync(long id) =>
        await _dbContext.Set<Topic>()
            .FirstAsync(x => x.Id.Equals(id));
}