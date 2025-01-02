using ECommerce.Shared.Authentication;
using ECommerce.Shared.Infrastructure.Outbox;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using OpenTelemetry.Metrics;
using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "Order";

builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddOutbox(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(serviceName, builder.Configuration, (traceBuilder) => 
        traceBuilder.WithSqlInstrumentation())
    .AddOpenTelemetryMetrics(serviceName, builder.Services, (metricBuilder) => 
        metricBuilder.AddView("products-per-order", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = [1, 2, 5, 10]
        }));

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
    app.ApplyOutboxMigrations();
}

app.RegisterEndpoints();

app.UsePrometheusExporter();

app.UseJwtAuthentication();

app.Run();

public partial class Program { }
