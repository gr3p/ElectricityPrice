using System.ComponentModel.DataAnnotations;
using ElectricityPrice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var app = builder.Build();



app.MapGet("/", () =>
{
    var scraper = new WebScraper();


    return scraper.GetS03Prices("https://www.elbruk.se/timpriser-se3-stockholm#aktuella\")");
});

app.MapGet("/s03", async (HttpRequest request, HttpResponse response, CancellationToken token) =>
{

    if (request.QueryString.HasValue)
    {
        //Todo...
    }
    var scraper = new WebScraper();

    var results = scraper.GetS03Prices("https://www.elbruk.se/timpriser-se3-stockholm#aktuella\")");
   
    await response.WriteAsJsonAsync(results, token).ConfigureAwait(false);
});


app.Run();
