using API.Context;
using API.Model.Email;
using API.Services.Implementations;
using API.Services.Interfaces;
using Application.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using NATS.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailService, EmailService>();
var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");
builder.Services
    .AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
builder.Services.AddSingleton<IConnection>(sp =>
  {
      var options = ConnectionFactory.GetDefaultOptions();
      options.Url = "nats://localhost:4222";
      return new ConnectionFactory().CreateConnection(options);
  }).AddHostedService<SubscriberService>();

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
