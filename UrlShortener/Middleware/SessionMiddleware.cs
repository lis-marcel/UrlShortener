using UrlShortener.Service;

namespace UrlShortener.Middleware
{
    public class SessionRefreshMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionRefreshMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, SessionService sessionService)
        {
            // Extract user email or session key from the request
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                await sessionService.RefreshSessionExpiration(token);
            }

            await _next(context);
        }
    }
}
