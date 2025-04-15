using CompanyEmployees;
using CompanyEmployees.Core.Services.Abstractions;
using CompanyEmployees.Core.Services.DataShaping;
using CompanyEmployees.Core.Services.Hateoas;
using CompanyEmployees.Infrastructure.Presentation.ActionFilters;
using CompanyEmployees.ServiceExtensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.DataTransferObjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();

builder.Services.ConfigureVersioning();
//builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
        //config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
    })
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    .AddApplicationPart(typeof(CompanyEmployees.Infrastructure.Presentation.AssemblyReference).Assembly);

builder.Services.AddCustomMediaTypes();

builder.Host.UseSerilog((hostContext, configuration) =>
{
    configuration.ReadFrom.Configuration(hostContext.Configuration);
});

var app = builder.Build();

app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.ConfigureHealthChecksEndpoints();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRateLimiter();
app.UseCors("CorsPolicy");
//app.UseResponseCaching();
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"Logic before executing the next delegate in the Use method");
//     await next.Invoke();
//     Console.WriteLine($"Logic after executing the next delegate in the Use method");
// });
// app.Map("/usingmapbranch", builder =>
// {
//     builder.Use(async (context, next) =>
//     {
//         Console.WriteLine("Map branch logic in the Use method before the next delegate");
//         await next.Invoke();
//         Console.WriteLine("Map branch logic in the Use method after the next delegate");
//     });
//     builder.Run(async context =>
//     {
//         Console.WriteLine($"Map branch response to the client in the Run method");
//         await context.Response.WriteAsync("Hello from the map branch.");
//     });
// });
// app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
// {
//     builder.Run(async context =>
//     {
//         await context.Response.WriteAsync("Hello from the MapWhen branch.");
//     });
// });
// app.Run(async context =>
// {
//     Console.WriteLine($"Writing the response to the client in the Run method");
//     await context.Response.WriteAsync("Hello from the middleware component.");
// });

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Maze API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Code Maze API v2");
});

app.MapControllers();

app.Run();
return;

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();

public partial class Program { }
