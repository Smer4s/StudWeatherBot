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
        public static string ParseWeather(this TotalWeather conditions)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Информация о погоде в Минске в {DateTime.Now.ToShortTimeString()}");

            sb.AppendLine("Средняя температура: " + conditions.Temperature + " °C");
            sb.AppendLine("Ощущается как: " + conditions.ApparentTemperature + " °C");
            sb.AppendLine("Скорость ветра: " + conditions.WindSpeed + " м/с");
            sb.AppendLine("Общее состояние: " + conditions.Description);


            return sb.ToString();
        }
    }
}
