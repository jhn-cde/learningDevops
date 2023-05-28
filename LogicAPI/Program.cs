using LogicAPI.Models;
using LogicAPI.DataService;
using LogicAPI.BusinessService;

var builder = WebApplication.CreateBuilder(args);

// Cors
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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Mysql connString 
builder.Services.AddDbContext<Context>(); 

builder.Services.AddTransient<DictionaryDataService>();
builder.Services.AddTransient<DictionaryBusinessService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
