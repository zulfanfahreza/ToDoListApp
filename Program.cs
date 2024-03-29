using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;
using ToDoListApp.DatabaseContext;
using ToDoListApp.Services;
using ToDoListApp.Utilities;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

//Add Jwt configuration
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add DB configuration
var selectedDb = builder.Configuration.GetConnectionString("Selected");
if (string.IsNullOrEmpty(selectedDb))
{
    throw new Exception("Database not selected. Please check appsettings.json");
}

switch(selectedDb.ToUpper())
{
    case "MYSQL":
        builder.Services.AddDbContext<ToDoDbContext>(
            opt => {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));
                var connectionString = builder.Configuration.GetConnectionString("Options:MySql");
                opt.UseMySql(connectionString, serverVersion);
            });
        break;
    case "SQLSERVER":
        builder.Services.AddDbContext<ToDoDbContext>(
            opt => {
                var connectionString = builder.Configuration.GetConnectionString("Options:SqlServer");
                opt.UseSqlServer(connectionString);
            });
        break;
    default:
        throw new Exception("No Known Database match selected database");
}

builder.Services.AddScoped<IToDoDbContext, ToDoDbContext>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<ILogging, Logging>();

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
}).AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
