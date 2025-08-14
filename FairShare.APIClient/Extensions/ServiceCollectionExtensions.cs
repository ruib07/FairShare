using FairShare.APIClient.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FairShare.APIClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFairShareServices(this IServiceCollection services, IConfiguration configuration) 
    { 
        var apiUrl = configuration["Api:Url"] ?? throw new InvalidOperationException("API URL not configured."); 
        
        if (!apiUrl.EndsWith('/')) apiUrl += "/"; 
        
        services.AddHttpClient("FairShareApi", client => 
        { 
            client.BaseAddress = new Uri(apiUrl, UriKind.Absolute); 
        });

        services.AddScoped<IAuthenticationsApiService, AuthenticationsApiService>();
        services.AddScoped<IUsersApiService, UsersApiService>();
        services.AddScoped<IExpenseParticipantsApiService, ExpenseParticipantsApiService>(); 
        services.AddScoped<IExpensesApiService, ExpensesApiService>(); 
        services.AddScoped<IGroupMembersApiService, GroupMembersApiService>(); 
        services.AddScoped<IGroupsApiService, GroupsApiService>(); 
        services.AddScoped<ISettlementsApiService, SettlementsApiService>(); 
        
        return services; 
    }
}
