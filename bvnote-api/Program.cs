using bvnote_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;
// Instantiating web application
var builder = WebApplication.CreateBuilder(args);

// Registering & configuring services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebClients",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500");
        });
});

var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
var connString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddScoped(_conn => new MySqlConnection(connString));
builder.Services.AddDbContext<MyContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Registering & configuring middleware
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("WebClients");
app.MapControllers();
app.Run();

