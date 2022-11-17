using System.ComponentModel.DataAnnotations;
using ElectricityPrice;
using Microsoft.Extensions.Caching.Memory;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

var app = builder.Build();



app.MapGet("/", (IMemoryCache memoryCache) =>
{
    return "Online...";
});

app.MapGet("/s03", async (HttpRequest request, HttpResponse response, CancellationToken token, IMemoryCache memoryCache) =>
{
   
    if (request.QueryString.HasValue)
    {
        //Todo...
    }
    var scraper = new WebScraper(memoryCache);

    var results = scraper.GetS03Prices("https://www.elbruk.se/timpriser-se3-stockholm#aktuella\")");
   
    await response.WriteAsJsonAsync(results, token).ConfigureAwait(false);
});


app.Run();
