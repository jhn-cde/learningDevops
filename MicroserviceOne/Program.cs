using MicroserviceOne.DataService;
using MicroserviceOne.BusinessService;
using MicroserviceOne.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(
    policy  =>
    {
      policy.WithOrigins("*")
      .AllowAnyHeader()
      .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// connection string
var host = builder.Configuration["DBHOST"] ?? "localhost";
var port = builder.Configuration["DBPORT"] ?? "3307";
var password = builder.Configuration["MYSQL_PASSWORD"] ?? builder.Configuration.GetConnectionString("MYSQL_PASSWORD");
var userid = builder.Configuration["MYSQL_USER"] ?? builder.Configuration.GetConnectionString("MYSQL_USER");
var userDB = builder.Configuration["MYSQL_DATABASE"] ?? builder.Configuration.GetConnectionString("MYSQL_DATABASE");

var connString = $"server={host}; userid={userid}; pwd={password}; port={port}; database={userDB}";
//*/var connString = "server=localhost;Port=3307;Database=school;Uid=root;Pwd=140156";
//var connString = "server=mysql;Port=3306;database=school;Uid=root;Pwd=password";

builder.Services.AddDbContext<Context>(opt => 
  opt.UseMySQL(connString)
); 


builder.Services.AddTransient<StudentDataService>();
builder.Services.AddTransient<StudentBusinessService>();
builder.Services.AddTransient<StudentCourseDataService>();
builder.Services.AddTransient<StudentCourseBusinessService>();

builder.Services.AddTransient<DictionaryDataService>();
builder.Services.AddTransient<DictionaryBusinessService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
