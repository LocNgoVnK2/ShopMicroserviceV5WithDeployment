using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);


builder.Services.AddTransient<LoggingDelegatingHandler>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//config Grpc is calling to Discount
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])); ;
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
//config AddMassTransit rappit MQ
builder.Services.AddMassTransit(config =>
{

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(/*"amqp://guest:guest@localhost:5672"*/builder.Configuration["EventBusSettings:HostAddress"]);
        
    });
});
//builder.Services.AddMassTransitHostedService();

builder.Services.AddStackExchangeRedisCache(options =>
{
 
    options.Configuration = builder.Configuration.GetSection("CacheSettings:ConnectionString").Value;
    
});
builder.Services.AddOpenTelemetry().WithTracing(b => {
    b.SetResourceBuilder(
        ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
     .AddAspNetCoreInstrumentation()
     .AddOtlpExporter(opts => { opts.Endpoint = new Uri(builder.Configuration["Jeager:IdsUrl"]); });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});
app.UseAuthorization();

app.MapControllers();

app.Run();
