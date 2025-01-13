using Base62;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortLinkGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUrlDataRepository, UrlDataRepository>();

builder.Services.AddDbContext<Db>(options =>
{
    options.UseInMemoryDatabase(databaseName: "ShortLinkGenerator");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/generate", (string url, IUrlDataRepository urlDataRepository, HttpRequest request) =>
    {
        var shortUrl = urlDataRepository.SaveUrlData(url);
        
        return $"https://{request.Host}/{shortUrl}";
    })
    .WithName("Generate")
    .WithOpenApi();


// !! httpS:\\ 
app.MapGet("/{text}", (string text, [FromServices] IUrlDataRepository urlDataRepository, HttpContext context) =>
    {
        var originalUrl = urlDataRepository.GetOriginalUrl(text);
        
        context.Response.Redirect(originalUrl);
    })
    .WithOpenApi();
    
app.Run();

