using bvnote_web_api.Data;
using bvnote_web_api.RouteGroup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BvnV1Context>(options => options.UseMySQL(connectionString));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebClients",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500");
        });
});

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
app.UseAuthorization();
app.UseCors("WebClients");
app.MapControllers();

// API Endpoints
app.MapGroup("/v1")
    .MapBible()
    .RequireCors("WebClients");

app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });
app.Run();

