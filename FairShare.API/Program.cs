using FairShare.API.Configurations;
using FairShare.Application.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomSecurity(builder.Configuration);
builder.Services.AddCustomDatabaseConfiguration(builder.Configuration);

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
