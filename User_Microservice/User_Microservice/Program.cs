using Microsoft.EntityFrameworkCore;
using User_Microservice.Context;
using User_Microservice.Hashing;
using User_Microservice.Interface;
using User_Microservice.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UserContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<HashingPassword>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
