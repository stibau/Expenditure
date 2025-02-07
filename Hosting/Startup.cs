namespace Hosting;

public class Startup
{
    // Called first by the IHost.Run() method
    public void ConfigureServices(IServiceCollection services)
    {
        // We set up the DI of the services as much as possible in the IHostBuilder.ConfigureWebHost() method
        // in HostBuilderFactory
    }

    // Called next. It sets up the middleware pipeline
    public void Configure(IApplicationBuilder app)
    {
        // Matches incoming request urls to available endpoints.
        app.UseRouting();

        // Will actual execute the endpoints, that are in this case mapped from the controllers
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/", () => "Hello World!");
        });
    }
}