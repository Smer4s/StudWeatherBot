using Telegram.Bot.Types;
using Telegram.Bot;
using StudWeatherBot.Bot.Logic;
using Telegram.Bot.Types.InlineQueryResults;
using StudWeatherBot.Bot.Data;
using Telegram.Bot.Exceptions;
using System.Linq.Expressions;

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
            try
            {
                updateOptions = await _botClient.GetUpdatesAsync(offset);

                foreach (var update in updateOptions)
                {
                    if (update.InlineQuery is not null)
                    {
                        await HandleInlineQuery(update.InlineQuery);
                    }
                    if (update.Message != null)
                    {
                        await HandleIncomingMessage(update.Message);
                    }

                    offset = update.Id + 1;
                }
            }
            catch (ApiRequestException)
            {
                // Doesn't need to stop updating when other instances exist
            }
            catch (RequestException ex)
            {
                Console.WriteLine(DateTime.UtcNow + " " + ex.ToString());
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }
    }

    private async Task HandleInlineQuery(InlineQuery query)
    {
        var queryId = query.Id;
        var weather = await TelegramBotCommands.GetWeather();
        var verboseWeather = await TelegramBotCommands.GetWeatherVerbose();
        var result = new List<InlineQueryResultArticle>()
        {
            new InlineQueryResultArticle(Guid.NewGuid().ToString(), "Погода в минске сейчас", new InputTextMessageContent(weather)),
            new InlineQueryResultArticle(Guid.NewGuid().ToString(), "Подробная погода в минске сейчас", new InputTextMessageContent(verboseWeather))
        };

        try
        {
            await _botClient.AnswerInlineQueryAsync(queryId, result);
        }
        catch (ApiRequestException)
        {
            Update[]? updates;
            int offset = 0;
            do
            {
                updates = await _botClient.GetUpdatesAsync(offset);
                offset = updates.Last().Id + 1;
            } while (updates.Any());
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
                "/start" => TelegramBotCommands.GetAllCommands(),
                "/weather" => await TelegramBotCommands.GetWeather(),
                "/weatherverbose" => await TelegramBotCommands.GetWeatherVerbose(),
                _ => "Неизвестная команда, используйте /help",
            };

            await _botClient.SendTextMessageAsync(chatId, replyText);
        }
    }
}