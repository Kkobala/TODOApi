using Microsoft.EntityFrameworkCore;
using TODOAppApi.Auth;
using TODOAppApi.Db;
using TODOAppApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IAuthService, AuthService>();
AuthConfigurator.Configure(builder);

builder.Services.AddDbContextPool<AppDbContext>(c =>
    c.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.AddTransient<ISendEmailRequestRepository, SendEmailRequestRepository>();

//builder.Services.AddTransient<ITodoRepository, TodoRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(c =>
//    c.WithOrigins("http://localhost:5183")
//        .WithMethods("GET", "POST", "PUT", "DELETE")
//        .AllowCredentials()
//        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();