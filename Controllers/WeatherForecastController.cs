using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;

using wheather_api.models.WheatherForecast;

namespace wheather_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

    /// <summary>
    /// Geting current wheather forecast by city name
    /// </summary>
    /// <param name="city">Name of the city for which you want to get the weather forecast</param>
    ///<remarks>
    /// Sample request:
    ///
    ///     GET /WheatherForecast/CurrentWheatherByCityName?city=dnipro
    ///     {
    ///        "dt": "1611144000",  
    ///        "temp": 3,
    ///        "wind": 34
    ///        "clouds": 1
    ///     }
    ///     dt - date in UTC Unix
    ///     temp - temperature in C
    ///     wind - wind speed
    ///     clouds - clouds in city
    /// </remarks>
    /// <response code="200">Returns the current wheather forecast for city</response>
    /// <response code="400">If the city not found</response> 
        [HttpGet("CurrentWheatherByCityName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentWhetherByCityName(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Constants.url);
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={Constants.apiKey}&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<Wheather>(stringResult);

                    return Ok(new {
                        dt = rawWeather.dt,
                        temp = rawWeather.main.temp,
                        wind = rawWeather.wind.speed,
                        clouds = rawWeather.clouds.all
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
        


    /// <summary>
    /// Geting 5 days wheather forecast
    /// </summary>
    /// <param name="city">Name of the city for which you want to get the weather forecast</param>
    ///<remarks>
    /// Sample request:
    ///
    ///      GET /WheatherForecast/WheatherForecastByCityName?city=dnipro
    ///
    ///     "list": [
    ///         {
    ///          "dt": 1611144000,
    ///          "main": {
    ///             "temp": -12.59,
    ///             "temp_min": -12.59,
    ///             "temp_max": -10.89
    ///             },
    ///          "wind": {
    ///             "speed": 1.23
    ///           },
    ///          "clouds": {
    ///             "all": 13
    ///            }
    ///           }]
    ///
    ///        list.dt = date in UTC Unix
    ///        list.main.temp - current temperatue C
    ///        list.main.temp_min - minimal temperature C
    ///        list.main.temp_max - maximal temperature C
    ///        list.wind.speed - wind speed
    ///        list.clouds.all - clouds 
    ///
    /// </remarks>
    /// <response code="200">Returns the 5 days wheather forecast for city</response>
    /// <response code="400">If the city not found</response> 
        [HttpGet("WheatherForecastBycity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWhetherForecastByCityName(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Constants.url);
                    var response = await client.GetAsync($"/data/2.5/forecast?q={city}&appid={Constants.apiKey}&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<WheatherForecast>(stringResult);

                    return Ok(rawWeather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }
}
