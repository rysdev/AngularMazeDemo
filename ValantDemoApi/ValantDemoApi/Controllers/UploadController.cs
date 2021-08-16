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
  public class UploadController : ControllerBase
  {
    private readonly ILogger<UploadController> _logger;
    private readonly IMemoryCache _memoryCache;
    private static readonly string _mazeNamesKey = "mazenames";
    private static readonly string _startKeyword = "start";

    public UploadController(ILogger<UploadController> logger, IMemoryCache memoryCache)
    {
      _logger = logger;
      _memoryCache = memoryCache;
    }

    [HttpPost]
    public IEnumerable<string> UploadMazeFile()
    {
      IFormFile file = Request.Form.Files[0];
      List<string> mazeLines = new List<string>();
      string line, startPositionKey;
      int mazeWidth = 0;

      var cacheExpiryOptions = new MemoryCacheEntryOptions
      {
        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
        Priority = CacheItemPriority.High,
        SlidingExpiration = TimeSpan.FromMinutes(2),
        Size = 1024
      };

      using (var reader = new StreamReader(file.OpenReadStream()))
      {
        while (reader.Peek() >= 0)
        {
          line = reader.ReadLine();
          mazeLines.Add(line);
          mazeWidth = line.Length > mazeWidth ? line.Length : mazeWidth;
        }
      }

      int i = 0, j;
      char[,] mazeArray = new char[mazeLines.Count, mazeWidth];
      foreach (var row in mazeLines)
      {
        j = 0;
        foreach (char c in row)
        {
          mazeArray[i, j] = c;
          if (c == 'S')
          {
            startPositionKey = file.FileName + _startKeyword;
            _memoryCache.Set(startPositionKey, new int[] { i, j }, cacheExpiryOptions);
          }
          j++;
        }
        i++;
      }

      _memoryCache.Set(file.FileName, mazeArray, cacheExpiryOptions);

      List<string> mazeNames;
      _memoryCache.TryGetValue(_mazeNamesKey, out mazeNames);
      if (mazeNames != null)
      {
        mazeNames.Add(file.FileName);
        _memoryCache.Set(_mazeNamesKey, mazeNames, cacheExpiryOptions);
      }
      else
      {
        mazeNames = new List<string>();
        mazeNames.Add(file.FileName);
        _memoryCache.Set(_mazeNamesKey, mazeNames, cacheExpiryOptions);
      }

      return mazeNames;
    }
  }
}
