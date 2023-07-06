using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Tickets.WebAPI.Middlewares;
using Tickets.Infrastructure.Services.Implementations;
using Tickets.Infrastructure.Services.Interfaces;
using Tickets.Infrastructure.Common;
using Tickets.WebAPI.Services.Interfaces;
using Tickets.Infrastructure.Contexts;
using Tickets.WebAPI.Services.Implementations;
using Microsoft.Extensions.Options;
using Tickets.WebAPI.Options.Implementations;

var builder = WebApplication.CreateBuilder(args);

string schemasPath = AppContext.BaseDirectory + builder.Configuration.GetValue<string>("Schemas:SchemasPath");
string schemasTemplatePath = builder.Configuration.GetValue<string>("Schemas:SchemasTemplatePath");
string sqlQueriesPath = AppContext.BaseDirectory + builder.Configuration.GetValue<string>("Sql:SqlQueriesPath");
builder.Services.AddControllers().AddJsonOptions(option =>
            option.JsonSerializerOptions.AllowTrailingCommas = true);
builder.Services.AddOptions<JsonSchemas>().Configure(x => x.LoadSchemas(schemasPath));
builder.Services.AddOptions<SqlQueries>().Configure(x => x.LoadQueries(sqlQueriesPath));
builder.Services.AddTransient<ISchemasValidatorService>(x =>
    new JsonSchemasValidatorService(x.GetRequiredService<IOptions<JsonSchemas>>(), schemasTemplatePath));
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Tickets.WebAPI"))
    .UseSnakeCaseNamingConvention()
    );
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddScoped<IMappingService, MappingService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseMiddleware<BufferingEnablerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.Run();