using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataEntityFramework;
using SocialNetwork.DataEntityFramework.Repositories;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.Services;
using SocialNetwork.IdentityPasswordHasherLib;
using SocilalNetwork.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var msSqlConfig = builder.Configuration
   .GetRequiredSection("MsSqlConfig")
   .Get<MsSqlConfig>();
if (msSqlConfig is null)
{
    throw new InvalidOperationException("MsSqlConfig is not configured");
}

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer
($"Server = {msSqlConfig.ServerName}; Database = SocialNetwork; " +
$"User Id = {msSqlConfig.UserName}; Password = {msSqlConfig.Password}; TrustServerCertificate=True"));

builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IApplicationPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
