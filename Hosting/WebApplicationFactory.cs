using System.Diagnostics;
using System.Reflection;
using Hosting.Endpoints;
using InfraServices;
using Microsoft.OpenApi.Models;
using Services;

namespace Hosting;

public static class WebApplicationFactory
{
    public static WebApplication CreateWebApplication(string[] args)
    {
        // We set up a Web Application builder using our own in-house WebApplicationBuilderFactory
        var webApplicationBuilder = WebApplicationBuilderFactory.CreateWebApplicationBuilder(args);

        // We configure service as needed
        ConfigureService(webApplicationBuilder.Services, webApplicationBuilder.Configuration);

        // We build the WebApplication
        var webApplication = webApplicationBuilder.Build();

        // We configure the middleware on the web application
        ConfigureMiddleWare(webApplication, webApplication.Environment, webApplication.Configuration);

        // We configure the endpoints on the application
        ConfigureEndpoints(webApplication);

        return webApplication;
    }

    private static void ConfigureMiddleWare(IApplicationBuilder webApplication, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        // For testing purposes only, if you add this, you get a welcome page for url "/"
        // webApplication.UseWelcomePage();

        // Middleware that servers static files from the wwwroot folder
        //webApplication.UseStaticFiles();

        if (webHostEnvironment.IsDevelopment())
        {
            webApplication.UseDeveloperExceptionPage();
        }
        else webApplication.UseExceptionHandler("/Error");

        if (configuration.IsSwaggerEnabled())
        {
            // Add the Swagger middleware: generates the Swagger documentation
            webApplication.UseSwagger();
        
            // Add the Swagger UI middleware: serves the Swagger documentation
            webApplication.UseSwaggerUI(conf => conf.SwaggerEndpoint("/swagger/expenditure/swagger.json", "Expenditure API"));    
        }

        // Add the Routing middleware: looks at the request path and matches it to an endpoint
        // Auto added by ASP.NET Core
        webApplication.UseRouting();
    }

    private static void ConfigureEndpoints(IEndpointRouteBuilder webApplication)
    {
        // Set up the various endpoint mappings. This calls the UseEndpoints middleware internally 
        webApplication.MapGet("/", () => "Hello World!");
        
        webApplication.MapGet("GenerateError", () =>
        {
            // ReSharper disable once ConvertToLambdaExpression
            throw new Exception();
        });

        webApplication.MapGet("Error", () => "This is a nice, friendly error page");
        
        webApplication.MapExpensesEndpoints();
    }

    private static void ConfigureService(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsSwaggerEnabled())
        {
            // Adds the endpoint-discovery features of ASP.NET Core that Swashbuckle requires
            services.AddEndpointsApiExplorer();
           
            // Adds the Swashbuckle services required for creating OpenApi Documents
            services.AddSwaggerGen(cnf =>
            {
                var assembly = Assembly.Load(new AssemblyName("Hosting"));
                var apiVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
                cnf.SwaggerDoc("expenditure", new OpenApiInfo
                {
                    Title = "Expenditure API",
                    Version = $"v{apiVersion.FileVersion}",
                    Description = "API for managing expenses"
                });
            });    
        }
        
        // Placeholder to further configure service that may be needed and that have not yet been configured in the
        // WebApplicationBuilderFactory. 
        services.AddScoped<IExpensesService, ExpensesService>();
    }
}