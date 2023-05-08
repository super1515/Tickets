using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Tickets.Infrastructure;
using Tickets.Services.Implementations;
using Tickets.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string schemasPath = AppContext.BaseDirectory + builder.Configuration.GetValue<string>("Schemas:SchemasPath");
string schemasTemplate = builder.Configuration.GetValue<string>("Schemas:SchemasTemplate");

builder.Services.AddControllers();
builder.Services.AddSingleton<ISchemasStorageService>(x =>
    new FileSchemasStorageService(schemasPath));
builder.Services.AddTransient<ISchemasValidatorService>(x =>
    new JsonSchemasValidatorService(x.GetRequiredService<ISchemasStorageService>(), schemasTemplate));
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention()
    );
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();