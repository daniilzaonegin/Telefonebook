using Microsoft.EntityFrameworkCore;
using System.Reflection;
using telefone_book_api.Data;
using telefone_book_api.Extensions;
using telefone_book_api.Models.Dto;
using telefone_book_api.Services;

var builder = WebApplication.CreateBuilder(args);

//Add Authentication
builder.AddAuthentication();

// Add the service to generate JWT tokens
builder.Services.AddTokenService();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var serviceConfiguration = builder.Configuration;
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<TelephoneBookDbContext>(
    opt => opt.UseSqlite(serviceConfiguration.GetConnectionString(nameof(TelephoneBookDbContext))));

// Configure identity
builder.Services.AddIdentityCore<TelephoneBookUser>()
                .AddEntityFrameworkStores<TelephoneBookDbContext>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().SetIsOriginAllowed(_ => true));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
