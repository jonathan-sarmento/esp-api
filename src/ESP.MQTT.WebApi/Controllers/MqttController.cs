using ESP.MQTT.WebApi.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ESP.MQTT.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        public MqttController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        [HttpGet]
        [Route("topics")]
        public async Task<IActionResult> GetAllTopics() 
            => Ok(await _topicRepository.GetAllAsync());
    }
}
