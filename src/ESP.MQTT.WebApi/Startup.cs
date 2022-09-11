using System.Globalization;
using ESP.MQTT.WebApi.Controllers;
using ESP.MQTT.WebApi.Infrastructure;
using ESP.MQTT.WebApi.Infrastructure.Abstractions;
using ESP.MQTT.WebApi.Infrastructure.Repositories;
using ESP.MQTT.WebApi.Services;
using ESP.MQTT.WebApi.Services.Abstractions;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ESP.MQTT.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<PostgreDbContext>(options => 
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? Configuration.GetValue<string>("CONNECTION_STRING")));
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        
        services.AddSingleton<IMqttHandler, MqttHandler>();

        services.AddScoped<ITopicRepository, TopicRepository>();
        
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "ESP.MQTT.Api", Version = "v1"});
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITopicRepository topicRepository)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESP.MQTT.Api v1"));
        }
        
        app.ConfigureMqttClient(app.ApplicationServices, topicRepository).Wait();
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}