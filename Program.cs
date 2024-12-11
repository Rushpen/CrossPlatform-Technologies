using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Gadelshin_Lab1.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Gadelshin_Lab1Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Gadelshin_Lab1Context") ?? throw new InvalidOperationException("Connection string 'Gadelshin_Lab1Context' not found.")));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
