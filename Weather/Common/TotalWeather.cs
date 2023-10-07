using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class TotalWeather
    {
        public WeatherPair Temperature { get; set; }
        public WeatherPair ApparentTemperature { get; set; }
        public WeatherPair WindSpeed { get; set; }
        public string? Description { get; set; }
        public DateTime RequestedDateTime { get; set; }

        public TotalWeather()
        {
            Temperature = new();
            ApparentTemperature = new();
            WindSpeed = new();
        }

        public virtual TotalWeather GetTotalWeather()
        {
            return new TotalWeather()
            {
                Temperature = WeatherPair.Evaluate(Temperature),
                ApparentTemperature = WeatherPair.Evaluate(ApparentTemperature),
                WindSpeed = WeatherPair.Evaluate(WindSpeed),
                Description = Description
            };
        }
    }
}
