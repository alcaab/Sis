using System.Globalization;
using System.Text.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Desyco.Dms.Application;
using Desyco.Dms.Infrastructure;
using Desyco.Dms.Web;
using Serilog;
using Serilog.Exceptions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Desyco.Dms.Web.Infrastructure.Exceptions;

const string CorsPolicy = "_allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterModule(new InfrastructureModule());
});

builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithProcessId()
    .Enrich.WithProcessName()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateLogger();

builder.Services.AddSerilog(Log.Logger);
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Desyco.Iam.Web.Extensions.IamModuleExtensions).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        
        // options.JsonSerializerOptions.DictionaryKeyPolicy =
        //     JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(settings =>
{
    settings.ReportApiVersions = true;
    settings.AssumeDefaultVersionWhenUnspecified = true;
    settings.DefaultApiVersion = new ApiVersion(1, 0);
    settings.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(settings =>
{
    settings.GroupNameFormat = "'v'VVV";
    settings.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:5002")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(CorsPolicy);
}
else
{
    app.UseHsts();
     /*
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy"
      }
     */
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<Desyco.Dms.Web.Infrastructure.Middleware.ProblemDetailsMiddleware>(); // Register custom middleware

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.UseExceptionHandler(); // Use the registered GlobalExceptionHandler

app.MapScalarApiReference(options =>
{
    options.Title = "Documentación de mi API";
    options.Theme = ScalarTheme.BluePlanet;
    options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
});

app.MapGet("/", () => Results.Redirect("/scalar"));

app.ExecuteMigrations();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<Desyco.Dms.Infrastructure.Seeders.ApplicationDbSeeder>();
    await seeder.SeedAsync();

    var featureSeeder = scope.ServiceProvider.GetRequiredService<Desyco.Iam.Infrastructure.Seeders.FeatureSeeder>();
    await featureSeeder.SeedAsync();

    var securitySeeder = scope.ServiceProvider.GetRequiredService<Desyco.Iam.Infrastructure.Seeders.SecuritySeeder>();
    await securitySeeder.SeedAsync();

    var permissionSeeder = scope.ServiceProvider.GetRequiredService<Desyco.Iam.Infrastructure.Seeders.PermissionSeeder>();
    await permissionSeeder.SeedAsync();
}

app.MapControllers();
app.MapFallbackToFile("index.html");

try
{
    Log.Information("Starting web host...");
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Verbose("Web host exited. Closing and flushing the logger...");
    Log.CloseAndFlush();
}
