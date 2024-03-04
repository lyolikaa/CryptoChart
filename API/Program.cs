using API;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CryptoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoContext")));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IDataProvider, FakeDataProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.UseMigrationsEndPoint();
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
    // DbInitializer.Initialize(context);
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

