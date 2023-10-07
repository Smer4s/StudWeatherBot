using System.Globalization;

namespace StudWeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBot telegramBot = new TelegramBot("6405433807:AAG1cCCh_cEmX76-rMfYgvOR6qrbkTX550o");
            CultureInfo belarusCulture = new CultureInfo("be-BY");
            Thread.CurrentThread.CurrentCulture = belarusCulture;
            Thread.CurrentThread.CurrentUICulture = belarusCulture;

            var botThread = new Thread(async () =>
            {
                await telegramBot.StartReceivingMessages();
            });

            botThread.Start();
            
            Console.ReadLine();
        }
    }
}