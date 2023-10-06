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
        private const string GISMETEOURLNOW = "https://www.gismeteo.by/weather-minsk-4248/now/";
        private const string GISMETEOURL = "https://www.gismeteo.by/weather-minsk-4248/";

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

                var docs = GetHtml(new Dictionary<string, string>()
                {
                    {"today" ,GISMETEOURL },
                    {"now", GISMETEOURLNOW},
                });

                _weather = new TotalWeather();

                GetOpenApiWeather(weather, _weather);
                GetGismeteoWeather(docs, _weather);

                _weatherVerbose = new TotalWeatherVerbose();

                GetOpenApiWeatherVerbose(weather, _weatherVerbose);
                GetGismeteoWeatherVerbose(docs, _weatherVerbose);
            }
        }

        private static IDictionary<string, HtmlDocument> GetHtml(Dictionary<string, string> urls)
        {
            using var webClient = new WebClient();
            var docs = new Dictionary<string, HtmlDocument>();
            foreach (var url in urls)
            {
                string htmlContent = webClient.DownloadString(url.Value);
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);
                docs.Add(url.Key, doc);
            }
            return docs;
        }

        private static void GetGismeteoWeatherVerbose(IDictionary<string, HtmlDocument> docs, TotalWeatherVerbose weatherVerbose)
        {
            GetGismeteoWeather(docs, weatherVerbose);

            var doc = docs["now"];
            weatherVerbose.Pressure += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='unit unit_pressure_mm_hg']")
                .InnerText.Where(c => char.IsDigit(c)).ToArray()!);
            var value = doc.DocumentNode.SelectSingleNode("//div[@class='now-info-item humidity']").InnerText.Where(c => char.IsDigit(c)).ToArray();
            weatherVerbose.Humidity += float.Parse(value);

            doc = docs["today"];
            var temperatureNode = doc.DocumentNode.SelectSingleNode("//div[@class='widget-row-chart widget-row-chart-temperature row-with-caption']");

            var temperatures = new List<HtmlNode>();
            foreach (var temperature in temperatureNode.ChildNodes.Last().ChildNodes.First().ChildNodes)
            {
                if (temperature.Attributes["class"].Value.Contains("value"))
                {
                    temperatures.Add(temperature);
                }
            }

            weatherVerbose.MorningTemperature += float.Parse(temperatures[3].ChildNodes.First().InnerText);

            weatherVerbose.EveningTemperature += float.Parse(temperatures[6].ChildNodes.First().InnerText);
        }

        private static void GetGismeteoWeather(IDictionary<string, HtmlDocument> docs, TotalWeather result)
        {
            var doc = docs["now"];
            result.Temperature += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='weather-value']")
                .SelectSingleNode("//span[@class='unit unit_temperature_c']").InnerText);

            result.ApparentTemperature += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='weather-feel']")
                .SelectSingleNode("//span[@class='measure']").SelectSingleNode("//span[@class='unit unit_temperature_c']").InnerText);

            result.WindSpeed += float.Parse(doc.DocumentNode.SelectSingleNode("//div[@class='unit unit_wind_m_s']").InnerText.Where(c => char.IsDigit(c)).ToArray()!);

            result.Description = doc.DocumentNode.SelectSingleNode("//div[@class='now-desc']").InnerText;

            result.WeatherCount++;
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

    }
}
