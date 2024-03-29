using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using TODO.API.Infrastructure.Exstensions;
using TODO.API.Infrastructure.Mapster;
using TODO.API.Infrastructure.Service;
using TODO.Persistence;
using TODO.Persistence.Context;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.RegisterMaps();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.AddDbContext<ApplicationDBcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));

builder.Services.AddServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddNewtonsoftJson();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1.0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader());
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.ConfigureHealthChecks(builder.Configuration);



builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = " TODO API",
        Description = "An ASP.NET Core Web API for TODO APP'S backend",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {

        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "my API V1");
    });

}




app.UseRouting();
app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHandlerMiddleware();
app.UseRequestResponseMiddleware();
app.UseHttpsRedirection();
app.UseCultureMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();
app.MapControllers();

//await UserSeed.Initialize(app.Services);
app.Run();
