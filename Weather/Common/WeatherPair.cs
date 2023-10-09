using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class WeatherPair
    {
        public int Count { get; private set; }
        private double value;
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                Count++;
            }
        }

        public WeatherPair()
        {
            Count = 0;
            value = 0;
        }

        public WeatherPair(int count, double value)
        {
            Count = count;
            this.value = value;
        }

        public static WeatherPair Evaluate(WeatherPair weather, int digits = 2)
        {
            var pair = new WeatherPair();

            pair.Count = weather.Count;
            pair.Value = Math.Round(weather.Value / (weather.Count == 0 ? 1 : weather.Count), digits);

            return pair;
        }
    }
}
