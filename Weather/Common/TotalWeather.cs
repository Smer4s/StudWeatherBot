using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class TotalWeather
    {
        public int WeatherCount { get; set; }
        public double Temperature { get; set; }
        public double ApparentTemperature { get; set; }
        public double WindSpeed { get; set; }
        public string? Description { get; set; }
        public DateTime RequestedDateTime { get; set; }

        public virtual TotalWeather GetTotalWeather()
        {
            var result = new TotalWeather
            {
                Temperature = Math.Round(Temperature / WeatherCount, 2),
                ApparentTemperature = Math.Round(ApparentTemperature / WeatherCount, 2),
                WindSpeed = Math.Round(WindSpeed / WeatherCount, 2),
                Description = Description
            };

            return result;
        }
    }
}
