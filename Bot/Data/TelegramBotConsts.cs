using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Bot.Data
{
    public static class TelegramBotConsts
    {
        private static readonly Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            {"/help", "Все команды" },
            {"/weather", $"Погода сейчас {DevTag}" },
            {"/weatherverbose", $"Подробная информация о погоде сейчас {DevTag}" },
            {"/notify", $"Переключить режим уведомлений {DevTag}" },
            {"/setnotifytime", $"Установить время получения погоды {DevTag}" }
        };

        public const string DevTag = "(dev)";

        public static IDictionary<string, string> Commands
        {
            get { return commands; }
        }
    }
}
