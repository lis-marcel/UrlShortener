using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UrlShortener.Database;
using UrlShortener.Models;
using UrlShortener.Service;

namespace UrlShortener.Controllers
{
    [EnableCors("DevCORS")]
    public class UrlShorteningController : Controller
    {
        private readonly DbStorageContext _context;

        public UrlShorteningController(DbStorageContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/add")]
        public async Task<IResult> Add()
        {
            var urlShorteningService = new UrlShorteningService(_context);

            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var myObject = JsonSerializer.Deserialize<UrlShorteningRequest>(body);

            var serviceDomain = $"{Request.Scheme}://{Request.Host}";
            var res = await urlShorteningService.Add(serviceDomain, myObject.Url);

            return Results.Ok(res);
        }

        [HttpGet]
        [Route("/{code}")]
        public async Task<IResult> RedirectRequest(string code)
        {
            var urlShorteningService = new UrlShorteningService(_context);
            var url = await urlShorteningService.FindUrl(code);

            return url is null 
                ? Results.NotFound() 
                : Results.Redirect(url);
        }
    }
}
