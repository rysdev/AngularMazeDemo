using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace ValantDemoApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MovesController : ControllerBase
  {
    private readonly ILogger<MovesController> _logger;
    private readonly IMemoryCache _memoryCache;

    public MovesController(ILogger<MovesController> logger, IMemoryCache memoryCache)
    {
      _logger = logger;
      _memoryCache = memoryCache;
    }

    [HttpGet]
    public IEnumerable<string> LoadMazeFile(string FileName)
    {
      var cacheExpiryOptions = new MemoryCacheEntryOptions
      {
        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
        Priority = CacheItemPriority.High,
        SlidingExpiration = TimeSpan.FromMinutes(2),
        Size = 1024
      };

      char[,] mazeArray;
      _memoryCache.TryGetValue(FileName, out mazeArray);

      if (mazeArray != null)
      {
        // check possible moves
      }

      return new List<string> { "Up", "Down", "Left", "Right" };
    }
  }
}
