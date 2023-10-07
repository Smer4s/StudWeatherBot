using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class TotalWeatherVerbose : TotalWeather
    {
        public WeatherPair EveningTemperature { get; set; }
        public WeatherPair MorningTemperature { get; set; }
        public WeatherPair Humidity { get; set; }
        public WeatherPair Pressure { get; set; }

        public TotalWeatherVerbose() : base()
        {
            EveningTemperature = new();
            MorningTemperature = new();
            Humidity = new();
            Pressure = new();
        }

        public override TotalWeatherVerbose GetTotalWeather()
        {
            var result = new TotalWeatherVerbose()
            {
                EveningTemperature = WeatherPair.Evaluate(EveningTemperature),
                MorningTemperature = WeatherPair.Evaluate(MorningTemperature),
                Humidity = WeatherPair.Evaluate(Humidity,0),
                Pressure = WeatherPair.Evaluate(Pressure,0),
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
