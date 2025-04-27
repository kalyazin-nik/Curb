using Curb.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Curb.API.ComponentRegistrar;

public static class SwaggerRegistrar
{
    public static SwaggerGenOptions SetIncludeXmlComments(this SwaggerGenOptions options)
    {
        var docTypeMarkers = new Type[] { typeof(WeatherForecastController), typeof(WeatherForecast) };
        foreach (var marker in docTypeMarkers)
        {
            var xmlFile = $"{marker.Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        }

        return options;
    }

    public static SwaggerGenOptions SetSecurityDefinition(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                            Enter 'Bearer' [space] and then your token in the text input below.
                            Example: 'Bearer secretKey'.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

        return options;
    }

    public static SwaggerGenOptions SetSecurityRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    Scheme = "oauth2",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
                },
                new List<string>()
            }
        });

        return options;
    }
}
