using Microsoft.AspNetCore.Http;

namespace ChristmasTree.Services.Token
{
    public class TokenAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? GetChristmasToken()
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext.Request.Headers.TryGetValue("Christmas-Token", out var token))
            {
                return token.ToString();
            }

            return null;
        }
    }
}
