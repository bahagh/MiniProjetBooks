var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Baha");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseDefaultFiles(); // Serve default files, like index.html, from wwwroot
app.UseStaticFiles();  // Serve static files from wwwroot

app.MapControllers();

app.Run();