using Microsoft.EntityFrameworkCore;
using Library.API.Services;
using Library.Core.Interfaces;
using Library.Infra.Data;
using Library.Infra.Repositories;
using Library.API.Services.Read;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();

builder.Services.AddScoped<LibraryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IBookReadService, BookReadService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 500;
    options.CompactionPercentage = 0.2;
    options.TrackStatistics = true;
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
}
);
builder.Services.AddScoped<CacheService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    SeedData.Initialize(dbContext);
}

app.Use(async (context, next) =>
{
    var correlationId = Guid.NewGuid().ToString();
    context.Response.Headers.Add("X-Correlation-ID", correlationId);
    await next();
});


app.UseHttpsRedirection();

//app.Use(async (context, next) =>
//{
//    if (!context.Request.Headers.ContainsKey("X-API-Key"))
//    {
//        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//        await context.Response.WriteAsync("API key is missing.");
//        return;
//    }

//        if(context.Request.Headers["X-API-Key"] != "your-secure-api-key")
//        {
//            context.Response.StatusCode = StatusCodes.Status403Forbidden;
//            await context.Response.WriteAsync("Invalid API key.");
//            return;
//        }
//    await next();
//}
//);

var requestCount = new Dictionary<string, List<DateTime>>();

app.Use(async(context, next) =>
{
    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    if(!requestCount.ContainsKey(ip))
    {
        requestCount[ip] = new List<DateTime>();
    }
    requestCount[ip].RemoveAll(timestamp => timestamp < DateTime.UtcNow.AddMinutes(1));
    if (requestCount[ip].Count >= 100)
    {
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.Response.WriteAsync("Too many requests. Please try again later.");
        return;
    }
    requestCount[ip].Add(DateTime.UtcNow);
    await next();
}
);



app.MapControllers();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
