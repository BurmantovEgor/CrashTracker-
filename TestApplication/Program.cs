


using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Repositories;
using TestApplication.Services;
using TestApplication.Interfaces.Crash;
using TestApplication.Interfaces.Operations;
using TestApplication.Interfaces.Status;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICrashService, CrashService>();
builder.Services.AddScoped<ICrashRepository, CrashRepository>();

builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IOperationRepository, OperationRepository>();

builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();


builder.Services.AddDbContext<CrashTrackerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CrashTrackerDbContext>();
    dbContext.SeedStatuses();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
