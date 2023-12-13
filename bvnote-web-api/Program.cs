using bvnote_web_api.Data;
using bvnote_web_api.RouteGroup;
using bvnote_web_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var serverVersion = new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "BvnClients",
        policy =>
        {
            policy.WithOrigins("http://192.168.1.124:3000", "http://localhost:3000", "http://127.0.0.1:5500")
                .AllowAnyOrigin()
                .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<DbBvnContext>(
    options => options
    .UseMySql(connectionString, serverVersion)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAbbrevService, AbbrevService>();

// Configure the HTTP request pipeline.
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
app.UseRouting();
app.UseCors("BvnClients");
app.UseAuthorization();
// API Endpoints
app.MapGroup("/v1")
    .MapBible()
    .RequireCors("BvnClients");

app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });
app.Run();

