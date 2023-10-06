using Telegram.Bot.Types;
using Telegram.Bot;
using StudWeatherBot.Bot.Logic;

public class TelegramBot
{
    private readonly TelegramBotClient _botClient;

    public TelegramBot(string token)
    {
        _botClient = new TelegramBotClient(token);
    }

    public async Task StartReceivingMessages()
    {
        Update[] updateOptions;
        int offset = 0;

        while (true)
        {
            updateOptions = await _botClient.GetUpdatesAsync(offset);

            foreach (var update in updateOptions)
            {
                if (update.Message != null)
                {
                    await HandleIncomingMessage(update.Message);
                }

                offset = update.Id + 1;
            }
        }
    }

    private async Task HandleIncomingMessage(Message message)
    {
        var chatId = message.Chat.Id;
        var text = message.Text;

        if (!string.IsNullOrEmpty(text))
        {
            string replyText = text switch
            {
                "/help" => TelegramBotCommands.GetAllCommands(),
                "/weather" => await TelegramBotCommands.GetWeather(),
                _ => "Неизвестная команда, используйте /help",
            } ;

            await _botClient.SendTextMessageAsync(chatId, replyText);
        }
    }
}