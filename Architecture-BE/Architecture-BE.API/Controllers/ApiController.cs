using Architecture_BE.Helper.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;

namespace Architecture_BE.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected readonly IConfiguration _config;

        private static HttpContext _httpContext;
        private static ClaimsPrincipal userClaim => _httpContext.User;

        public ApiController(IHttpContextAccessor contextAccessor,
            IConfiguration config)
        {
            _httpContext = contextAccessor.HttpContext;
            _config = config;
        }

        protected void SetConfig()
        {
            Config.UserName = userClaim.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }

        protected void ClearConfig()
        {
            Config.UserName = null;
        }
    }
}
