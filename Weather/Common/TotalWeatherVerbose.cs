using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Weather.Common
{
    public class TotalWeatherVerbose : TotalWeather
    {
        public override TotalWeatherVerbose GetTotalWeather()
        {
            return new TotalWeatherVerbose();

        }
    }
}
