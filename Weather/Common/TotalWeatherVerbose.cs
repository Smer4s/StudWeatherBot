using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class TotalWeatherVerbose : TotalWeather
    {
        public double EveningTemperature { get; set; }
        public double MorningTemperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }

        public override TotalWeatherVerbose GetTotalWeather()
        {
            var result = new TotalWeatherVerbose()
            {
                EveningTemperature = Math.Round(EveningTemperature / WeatherCount, 2),
                MorningTemperature = Math.Round(MorningTemperature / WeatherCount, 2),
                Humidity = Math.Round(Humidity/WeatherCount),
                Pressure = Math.Round(Pressure/WeatherCount)
            };
            AddWeather(result, base.GetTotalWeather());

            return result;
        }

        private static void AddWeather(TotalWeatherVerbose weatherVerbose, TotalWeather weather)
        {
            weatherVerbose.Temperature = weather.Temperature;
            weatherVerbose.ApparentTemperature = weather.ApparentTemperature;
            weatherVerbose.Description = weather.Description;
            weatherVerbose.WindSpeed = weather.WindSpeed;
        }
    }
}
