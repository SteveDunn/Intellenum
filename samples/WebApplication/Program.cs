using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Intellenum;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Vogen;

#if USE_SWASHBUCKLE
using Swashbuckle.AspNetCore.SwaggerGen;
#endif
#if USE_MICROSOFT_OPENAPI_AND_SCALAR
using Scalar.AspNetCore;
#endif

// [assembly: IntellenumDefaults(openApiSchemaCustomizations: OpenApiSchemaCustomizations.GenerateSwashbuckleMappingExtensionMethod)]

var builder = WebApplication.CreateBuilder(args);

#if USE_MICROSOFT_OPENAPI_AND_SCALAR
    builder.Services.AddOpenApi((OpenApiOptions o) =>
    {
    });
#endif

#if USE_SWASHBUCKLE
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.MapType<DeliveryScheme>(() => new OpenApiSchema
    {
        Type = "string",
        // does normal enums treated as string send the payload back as numbers? We want to use the same mechanism
        //as standard enums
        Enum = new List<IOpenApiAny>(DeliveryScheme.List().Select(s => new OpenApiString(s.Name)).ToList())
    });
    // the following extension method is available if you specify `GenerateSwashbuckleMappingExtensionMethod` - as shown above
//    opt.MapVogenTypes();
    
    // the following schema filter is generated if you specify GenerateSwashbuckleSchemaFilter as shown above
    // opt.SchemaFilter<MyVogenSchemaFilter>();
});
#endif

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

#if USE_SWASHBUCKLE
    app.UseSwagger();
    app.UseSwaggerUI();
#endif


app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            {
                decimal temperatureC = Random.Shared.Next(-20, 55);
                return new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    temperatureC,
                    TemperatureSummary.FromValue(Random.Shared.Next(TemperatureSummary.List().Count())),
                    City.RandomMember()
                );
            })
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/weatherforecast/{city}", (City city) =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            {
                decimal temperatureC = Random.Shared.Next(-20, 55);
                return new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    temperatureC,
                    TemperatureSummary.FromValue(Random.Shared.Next(TemperatureSummary.List().Count())),
                    city
                );
            })
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecastByCity")
    .WithOpenApi();

#if USE_MICROSOFT_OPENAPI_AND_SCALAR
app.MapOpenApi();
app.MapScalarApiReference();
#endif

app.Run();