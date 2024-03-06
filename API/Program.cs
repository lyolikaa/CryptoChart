using API;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CryptoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoContext")));

builder.Services.AddScoped<IDataProvider, BinanceDataProvider>();
builder.Services.AddHttpClient<IDataProvider, BinanceDataProvider>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ExchangeApiBase").Get<string>() ?? "http://localhost/");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CryptoContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapGet("/orderbook", ([FromServices] IDataProvider handler ) =>
    {
        var forecast = handler.GetSnapshot();
        return forecast;
    })
    .WithName("GetOrderBook")
    .WithOpenApi();

app.Run();

