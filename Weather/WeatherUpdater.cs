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

namespace StudWeatherAsyncBot.WeatherAsync
{
    public static class WeatherAsyncUpdater
    {
        private static TotalWeather _weather;
        private static TotalWeatherVerbose _weatherVerbose;
        private static DateTime _lastUpdateTime;
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
            if (DateTime.Now.Hour > _lastUpdateTime.Hour)
            {
                _lastUpdateTime = DateTime.Now;

                _weather = new TotalWeather();

                await GetOpenApiWeatherAsync(_weather);
                GetGismeteoWeatherAsync(_weather);

                _weatherVerbose = new TotalWeatherVerbose();

                await GetOpenApiWeatherVerboseAsync(_weatherVerbose);
            }
        }

        private static Task GetOpenApiWeatherVerboseAsync(TotalWeatherVerbose weatherVerbose)
        {
            
        }

        private async static Task GetOpenApiWeatherAsync(TotalWeather result)
        {
            IOpenWeatherService client = new OpenWeatherService("544f02b0dd946f3d4dab2c3c89ade944");
            var weather = await client.GetCurrentWeather("minsk", OWUnit.Metric, MultiWeatherApi.Model.Language.Russian);

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

        private static void GetGismeteoWeatherAsync(TotalWeather result)
        {
            using var webClient = new WebClient();
            string htmlContent = webClient.DownloadString("https://www.gismeteo.by/weather-minsk-4248/now/");
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
