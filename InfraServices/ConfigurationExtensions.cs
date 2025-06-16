namespace InfraServices;

public static class ConfigurationExtensions
{
    public static bool IsSwaggerEnabled(this IConfiguration configuration)
    {
        return bool.TryParse(configuration["Swagger:Enabled"], out var isSwaggerEnabled) && isSwaggerEnabled;
    }
}