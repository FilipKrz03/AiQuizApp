using Application;
using Domain.Entities;
using Infrastructure;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<QuizApplicationDbContext>()
    .AddApiEndpoints();

//builder.Host.UseSerilog((context, configuration) =>
//    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseWebAssemblyDebugging();
}

var group = app.MapGroup(prefix: "api");
group.MapIdentityApi<User>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
