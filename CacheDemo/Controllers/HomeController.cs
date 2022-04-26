using CacheDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CacheDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distCache;
        public HomeController(ILogger<HomeController> logger, IMemoryCache cache, IDistributedCache distCache)
        {
            _logger = logger;
            this._cache = cache;
            this._distCache = distCache;
        }

        public IActionResult Index()
        {
            // InMemory
            //DateTime cacheTimeStamp;
            //if(!_cache.TryGetValue("CachedTimeStamp", out cacheTimeStamp))
            //{
            //    var cacheOptions = new MemoryCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromSeconds(10));
            //    cacheTimeStamp = DateTime.Now;
            //    _cache.Set("CachedTimeStamp", cacheTimeStamp, cacheOptions);
            //}

            //Distributed Cache
            string cacheTimeStamp;
            cacheTimeStamp = _distCache.GetString("CachedTimeStamp");
            if(cacheTimeStamp == null)
            {
                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));
                cacheTimeStamp = DateTime.Now.ToString();
                _distCache.SetString("CachedTimeStamp", cacheTimeStamp, cacheOptions);
            }
            ViewBag.TimeStamp = cacheTimeStamp;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
