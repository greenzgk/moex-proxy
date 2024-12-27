using System.Net.Http.Headers;
using MoexProxy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<BondRecalcService>();

builder.Services.AddHttpClient<BondRecalcService>(client =>
{
    var url = builder.Configuration["BaseMoexUrl"]!;
    client.BaseAddress = new Uri(builder.Configuration["BaseMoexUrl"]!);
    client.DefaultRequestHeaders.Host = "iss.moex.com";
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
