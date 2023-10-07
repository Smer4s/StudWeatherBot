using MultiWeatherApi.Model;
using MultiWeatherApi.OpenWeather.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public static class WeatherExtensions
    {
        public static string ParseWeather(this TotalWeather weather)
        {
            var sb = new StringBuilder();
            sb.AddWeather(weather);

            return sb.ToString();
        }

        public static string ParseWeather(this TotalWeatherVerbose weather)
        {
            var sb = new StringBuilder();
            sb.AddWeather(weather);

            sb.AppendLine("Температура утром: " + weather.MorningTemperature + " °C");
            sb.AppendLine("Температура ночью: " + weather.EveningTemperature + " °C");
            sb.AppendLine("Влажность: " + weather.Humidity + " %");
            sb.AppendLine("Давление: " + weather.Pressure + " мм рт. ст.");

            return sb.ToString();
        }

        private static StringBuilder AddWeather(this StringBuilder sb, TotalWeather weather)
        {
            sb.AppendLine($"Информация о погоде в Минске в {DateTime.UtcNow.AddHours(3).ToShortTimeString()}");

            sb.AppendLine("Средняя температура: " + weather.Temperature + " °C");
            sb.AppendLine("Ощущается как: " + weather.ApparentTemperature + " °C");
            sb.AppendLine("Скорость ветра: " + weather.WindSpeed + " м/с");
            sb.AppendLine("Общее состояние: " + weather.Description);

            return sb;
        }
    }
}
