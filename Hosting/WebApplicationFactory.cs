using InfraServices;
using Services;

namespace Hosting;

public static class WebApplicationFactory
{
    public static WebApplication CreateWebApplication(string[] args)
    {
        // We set up a Web Application builder using our own in house WebApplicationBuilderFactory
        var webApplicationBuilder = WebApplicationBuilderFactory.CreateWebApplicationBuilder(args);

        // We configure service as needed
        ConfigureService(webApplicationBuilder.Services);

        // We build the WebApplication
        var webApplication = webApplicationBuilder.Build();

        // We configure the middle ware on the web application
        ConfigureMiddleWare(webApplication, webApplication.Environment);

        // We configure the endpoints on the application
        ConfigureEndpoints(webApplication);

        return webApplication;
    }

    private static void ConfigureMiddleWare(IApplicationBuilder webApplication, IWebHostEnvironment webHostEnvironment)
    {
        // For testing purposes only, if you add this you get a welcome page for url "/"
        // webApplication.UseWelcomePage();

        // Middleware that servers static files from the wwwroot folder
        //webApplication.UseStaticFiles();

        if (webHostEnvironment.IsDevelopment())
        {
            webApplication.UseDeveloperExceptionPage();
        }
        else webApplication.UseExceptionHandler("/Error");

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
        
        webApplication.MapGet("Expenses", (CostRepartitionService costRepartitionService) => costRepartitionService.GetAllExpenses());
    }

    private static void ConfigureService(IServiceCollection services)
    {
        // Placeholder to further configure service that may be needed and that have not yet been configured in the
        // WebApplicationBuilderFactory. 
        services.AddSingleton<CostRepartitionService>();
    }
}