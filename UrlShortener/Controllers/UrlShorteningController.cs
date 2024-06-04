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
    public class UrlShorteningController : Controller
    {
        private readonly DbStorageContext _context;
        private readonly UrlShorteningService _urlShorteningService;

        public UrlShorteningController(DbStorageContext context, UrlShorteningService urlShorteningService)
        {
            _context = context;
            _urlShorteningService = urlShorteningService;
        }

        [HttpPost]
        [Route("/add")]
        public async Task<IActionResult> Add([FromBody] UrlShorteningRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                return BadRequest("URL cannot be null or empty.");
            }

            var serviceDomain = $"{Request.Scheme}://{Request.Host}";
            var addedGuid = await _urlShorteningService.Add(serviceDomain, request.Url);

            var dbRecord = await _urlShorteningService.GetUrlById(addedGuid);
            var shortenedLink = dbRecord.ShortUrl;

            return Ok(shortenedLink);
        }

        [HttpGet]
        [Route("/{code}")]
        public async Task<IResult> RedirectRequest(string code)
        {
            var urlShorteningService = new UrlShorteningService(_context);
            var url = await urlShorteningService.FindUrlByCode(code);

            return url is null 
                ? Results.NotFound() 
                : Results.Redirect(url);
        }
    }
}
