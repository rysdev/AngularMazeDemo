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
    private static readonly string _startKeyword = "start";

    public MovesController(ILogger<MovesController> logger, IMemoryCache memoryCache)
    {
      _logger = logger;
      _memoryCache = memoryCache;
    }

    [HttpGet]
    public IEnumerable<string> LoadMazeFile(string FileName, int XPos = -1, int YPos = -1)
    {
      int x = XPos;
      int y = YPos;
      List<string> response = new List<string>();

      var cacheExpiryOptions = new MemoryCacheEntryOptions
      {
        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
        Priority = CacheItemPriority.High,
        SlidingExpiration = TimeSpan.FromMinutes(2),
        Size = 1024
      };

      if (x == -1 || y == -1)
      {
        int[] positionArray;
        _memoryCache.TryGetValue(FileName + _startKeyword, out positionArray);
        x = positionArray[1];
        y = positionArray[0];
      }
      response.Add(x.ToString());
      response.Add(y.ToString());

      char[,] mazeArray;
      _memoryCache.TryGetValue(FileName, out mazeArray);
      char c;

      if (mazeArray != null)
      {
        // check for victory
        c = mazeArray[y, x];
        if (c == 'E')
        {
          response.Insert(0, "Victory");
          return response;
        }

        // check possible moves
        int mazeRows = mazeArray.GetLength(0);
        int mazeCols = mazeArray.GetLength(1);
        if (y > 0)
        {
          c = mazeArray[y - 1, x];
          if (c == 'O' || c == 'S' || c == 'E')
          {
            response.Add("Up");
          }
        }
        if (y < mazeRows - 1)
        {
          c = mazeArray[y + 1, x];
          if (c == 'O' || c == 'S' || c == 'E')
          {
            response.Add("Down");
          }
        }
        if (x > 0)
        {
          c = mazeArray[y, x - 1];
          if (c == 'O' || c == 'S' || c == 'E')
          {
            response.Add("Left");
          }
        }
        if (x < mazeCols - 1)
        {
          c = mazeArray[y, x + 1];
          if (c == 'O' || c == 'S' || c == 'E')
          {
            response.Add("Right");
          }
        }
      }
      return response;
    }
  }
}
