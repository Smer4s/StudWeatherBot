using HtmlAgilityPack;
using MultiWeatherApi.OpenWeather;
using MultiWeatherApi.OpenWeather.Model;
using StudWeatherBot.Weather.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather
{
    public static class WeatherUpdater
    {
        private const string OPENWEATHERAPIKEY = "544f02b0dd946f3d4dab2c3c89ade944";
        private const string GISMETEOURL = "https://www.gismeteo.by/weather-minsk-4248/now/";

        private static TotalWeather _weather;
        private static TotalWeatherVerbose _weatherVerbose;
        private static int _lastUpdateTime;
        public async static Task<TotalWeather> GetWeatherAsync()
        {

            await UpdateWeatherAsync();

            return _weather.GetTotalWeather();
        }

        public async static Task<TotalWeatherVerbose> GetWeatherVerboseAsync()
        {
            await UpdateWeatherAsync();

            return _weatherVerbose.GetTotalWeather();
        }

        private async static Task UpdateWeatherAsync()
        {
            if (DateTime.Now.Hour > _lastUpdateTime)
            {
                _lastUpdateTime = DateTime.Now.Hour;
                IOpenWeatherService client = new OpenWeatherService(OPENWEATHERAPIKEY);
                var weather = await client.GetCurrentWeather("minsk", OWUnit.Metric, MultiWeatherApi.Model.Language.Russian);

                _weather = new TotalWeather();

                GetOpenApiWeather(weather, _weather);
                GetGismeteoWeather(_weather);

                _weatherVerbose = new TotalWeatherVerbose();

                GetOpenApiWeatherVerbose(weather, _weatherVerbose);
            }
        }

        private static void GetOpenApiWeatherVerbose(WeatherConditions weather, TotalWeatherVerbose result)
        {
            GetOpenApiWeather(weather, result);

            if (weather.Temperature.Pressure is not null)
            {
                result.Pressure += (float)weather.Temperature.Pressure;
            }

            if (weather.Temperature.Min is not null)
            {
                result.EveningTemperature += (float)weather.Temperature.Min;
            }

            if (weather.Temperature.Max is not null)
            {
                result.MorningTemperature += (float)weather.Temperature.Max;
            }

            if (weather.Temperature.Humidity is not null)
            {
                result.Humidity += (float)weather.Temperature.Humidity;
            }
        }

        private static void GetOpenApiWeather(WeatherConditions weather, TotalWeather result)
        {
            if (weather.Temperature.Daily is not null)
            {
                result.Temperature += (float)weather.Temperature.Daily;
            }

            if (weather.ApparentTemperature.Daily is not null)
            {
                result.ApparentTemperature += (float)weather.ApparentTemperature.Daily;
            }

            result.WindSpeed += weather.Wind.Speed;

            result.WeatherCount++;
        }

        private static void GetGismeteoWeather(TotalWeather result)
        {
            using var webClient = new WebClient();
            string htmlContent = webClient.DownloadString(GISMETEOURL);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            result.Temperature += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='weather-value']")
                .SelectSingleNode("//span[@class='unit unit_temperature_c']").InnerText);

            result.ApparentTemperature += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='weather-feel']")
                .SelectSingleNode("//span[@class='measure']").SelectSingleNode("//span[@class='unit unit_temperature_c']").InnerText);

            result.WindSpeed += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='unit unit_wind_m_s']").InnerText.Where(c => char.IsDigit(c)).ToArray()!);

            result.Description = doc.DocumentNode.SelectSingleNode("//div[@class='now-desc']").InnerText;

            result.WeatherCount++;
        }
    }
}
