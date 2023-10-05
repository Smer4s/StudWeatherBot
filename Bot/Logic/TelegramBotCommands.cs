using StudWeatherBot.Bot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudWeatherBot.Bot.Logic
{
    internal static class TelegramBotCommands
    {
        public static string GetAllCommands()
        {
            var commands = TelegramBotConsts.Commands;
            var sb = new StringBuilder();

            sb.AppendLine("С помощью этих команд ты можешь узнать информацию о погоде и получать уведомления.\n" +
                $"Команды, помеченые тегом {TelegramBotConsts.DevTag} пока что в разработке либо не работают:(\n");
            foreach (var command in commands)
            {
                sb.AppendLine($"{command.Key} : {command.Value}");
            }

            return sb.ToString();
        }
    }
}
