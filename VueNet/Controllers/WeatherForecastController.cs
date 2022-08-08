using System;
using System;
using System.Collections.Generic;
using System.Linq;
using VueNet.Models;
using Microsoft.AspNetCore.Mvc;
using VueNet.Authorization;
using Microsoft.Extensions.Logging;

namespace VueNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
            "會結冰", "舒適的冷", "寒冷", "涼", "柔和", "溫暖", "溫和", "熱", "令人不舒服的炎熱", "會烤焦"
        };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

