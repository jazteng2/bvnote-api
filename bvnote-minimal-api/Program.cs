// instantiate web application
var builder = WebApplication.CreateBuilder(args);

// Add & Configure Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
} 
else
{
    app.UseExceptionHandler("/error");
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
/*app.MapGet("/api/v1/books", () =>
{
    return "hello";
});

app.MapGet("/api/v1/books/{id}", (string id) =>
{

});*/
app.MapGet("/error", () => { Results.Problem(); });
app.Run();
