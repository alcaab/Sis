using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Desyco.Dms.Application;
using Desyco.Dms.Infrastructure;
using Desyco.Dms.Web;
using Serilog;
using Serilog.Exceptions;
using Scalar.AspNetCore; // Added for Scalar UI

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
builder.Services.AddControllers(/*opt => opt.ModelBinderProviders.Insert(0, new QueryOptionsBinderProvider())*/);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:5002")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// builder.Services
//     .AddApiVersioning(settings =>
//     {
//         settings.ReportApiVersions = true;
//         settings.AssumeDefaultVersionWhenUnspecified = true;
//         settings.DefaultApiVersion = new ApiVersion(1, 0);
//         settings.ApiVersionReader = new UrlSegmentApiVersionReader();
//     }).AddApiExplorer(settings =>
//     {
//         settings.DefaultApiVersion = new ApiVersion(1, 0);
//         settings.GroupNameFormat = "'v'VVV";
//         settings.SubstituteApiVersionInUrl = true;
//     });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(CorsPolicy);
}
else
{
    app.UseHsts();
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.MapScalarApiReference(options =>
{
    options.Title = "Documentación de mi API";
    options.Theme = ScalarTheme.BluePlanet;
    options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
});

app.MapGet("/", () => Results.Redirect("/scalar"));

// app.UseHangfireDashboard("/jobs", new DashboardOptions
// {
//     Authorization = [new OidcDashboardAuthorizationFilter()],
//     IgnoreAntiforgeryToken = true
// });

app.ExecuteMigrations();
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
