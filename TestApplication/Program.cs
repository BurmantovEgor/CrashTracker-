using Microsoft.EntityFrameworkCore;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Repositories;
using TestApplication.Core.Interfaces.Crash;
using TestApplication.Core.Interfaces.Operations;
using TestApplication.Core.Interfaces.Status;
using TestApplication.Core.Services;
using TestApplication.Application.Middleware;
using TestApplication.Core.Mapper_s;
using TestApplication.Core.Abstractions.User;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CrashTracker.Core.Abstractions;
using CrashTracker.Core.Services;
using CrashTracker.Application.Log_s;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' followed by a space and then your token",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "CrashTracker_";
});
builder.Services.AddLogging();
builder.Services.AddSingleton<RedisLogService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICrashService, CrashService>();
builder.Services.AddScoped<ICrashRepository, CrashRepository>();

builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IOperationRepository, OperationRepository>();

builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();




builder.Services.AddDbContext<CrashTrackerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CrashTrackerDbContext>();
    dbContext.Database.Migrate();
}


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CrashTrackerDbContext>();
    dbContext.SeedStatuses();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<HttpLogService>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

app.Run();
