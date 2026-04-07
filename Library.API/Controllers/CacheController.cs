using Microsoft.AspNetCore.Mvc;
using Library.API.Services;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly CacheService _cacheService;
        public CacheController(CacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            var health = _cacheService.GetHealth();
            var hitRate = health.Hits + health.Misses > 0 ? (double)health.Hits / (health.Hits + health.Misses) * 100 : 0;
            return Ok(new
            {
                health.Hits,
                health.Misses,
                health.Evictions,
                HitRate = hitRate,
                Staus = hitRate > 80 ? "Healthy" : "Unhealthy"
            });
        }
    }
}