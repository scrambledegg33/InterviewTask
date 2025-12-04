using Fines.Data;
using Fines.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<FinesDbContext>(options =>
    options.UseInMemoryDatabase("FinesDb"));

// Register services
builder.Services.AddScoped<IFinesRepository, FinesRepository>();
builder.Services.AddScoped<IFinesService, FinesService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinesDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
