using StudWeatherBot.Weather.Common;
using System.Globalization;
using Telegram.Bot.Exceptions;

namespace StudWeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBot telegramBot = new TelegramBot("6405433807:AAG1cCCh_cEmX76-rMfYgvOR6qrbkTX550o");
            CultureInfo belarusCulture = new CultureInfo("be-BY");
            Console.WriteLine("bot started");
            Console.WriteLine("Write /stop to stop bot");

            bool stop = false;
            while (!stop)
            {
                var botThread = new Thread(async () =>
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = belarusCulture;
                        Thread.CurrentThread.CurrentUICulture = belarusCulture;
                        await telegramBot.StartReceivingMessages();
                    }
                    catch (ApiRequestException e)
                    {
                        Console.WriteLine(e.Message + ".\nException handled at a time: " + DateTime.UtcNow.AddHours(3));
                        stop = false;
                    }
                });
                botThread.Start();

                stop = Console.ReadLine() == "/stop";
            }
        }
    }
}