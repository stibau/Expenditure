using Services;

namespace Hosting.Endpoints;

public static class ExpensesEndpoints
{
    public static void MapExpensesEndpoints(this IEndpointRouteBuilder webApplication)
    {
        //We use the WithTags method, so this tag is used to group endpoints in the Swagger documentation.
        var group = webApplication.MapGroup("/expenses").WithTags("Expenses");

        group.MapGet("/",GetAllExpenses);
    }

    private static IResult GetAllExpenses(IExpensesService costRepartitionService)
    {
        return Results.Ok(costRepartitionService.GetAllExpenses());
    }
}