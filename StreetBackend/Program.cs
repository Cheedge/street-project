using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Application.CommandHandlers;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Application.Queries;
using StreetBackend.Resources.Street.Application.QueryHandlers;
using StreetBackend.Resources.Street.Infrastructure.DbContexts;
using StreetBackend.Resources.Street.Infrastructure.Mappers;
using StreetBackend.Resources.Street.Infrastructure.Repositories;


// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB connection
builder.Services.AddDbContext<StreetDbContext>(options =>
{
    //var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite().UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    );
});



// Automapper
builder.Services.AddAutoMapper(
    typeof(StreetDomainAndStreetDtoMapper).Assembly,
    typeof(StreetEntityAndStreetDomainMapper).Assembly
);

// IoC container
builder.Services.AddScoped<IStreetRepository, StreetRepository>();
builder.Services.AddScoped<ICommandHandler<CreateNewStreetCommand>, CreateNewStreetCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeletePointFromStreetCommand>, DeletePointFromStreetCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteStreetCommand>, DeleteStreetCommandHandler>();
builder.Services.AddScoped<ICommandHandler<PostPointToStreetCommand>, PostPointToStreetCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllStreetsQuery, List<StreetDto>>, GetAllStreetsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetStreetByFieldQuery, StreetDto>, GetStreetByFieldQueryHandler>();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();

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

