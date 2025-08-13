using FairShare.API.Configurations;
using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Infrastructure.Data.Repositories;
using FairShare.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomSecurity(builder.Configuration);
builder.Services.AddCustomDatabaseConfiguration(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseParticipantRepository, ExpenseParticipantRepository>();
builder.Services.AddScoped<ISettlementRepository, SettlementRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IGroupMembersService, GroupMembersService>();
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IExpenseParticipantsService, ExpenseParticipantsService>();
builder.Services.AddScoped<ISettlementsService, SettlementsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AppSettings.AllowLocalhost,
        builder =>
        {
            builder.WithOrigins(AppSettings.ClientOrigin)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        });
});

var app = builder.Build();

app.UseCors(AppSettings.AllowLocalhost);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
