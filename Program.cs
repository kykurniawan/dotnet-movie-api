using MovieApi.Extensions;
using MovieApi.Http.Filters;
using MovieApi.Http.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddCommandLine(args);
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddHealthChecks();
builder.Services.AddControllers(cfg =>
{
    cfg.Filters.Add<ValidationFilter>();
    cfg.Filters.Add<ExceptionFilter>();
}).ConfigureApiBehaviorOptions(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.UseExampleMiddleware();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
