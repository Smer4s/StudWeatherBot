using System.Globalization;

namespace StudWeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBot telegramBot = new TelegramBot("6405433807:AAG1cCCh_cEmX76-rMfYgvOR6qrbkTX550o");
            CultureInfo belarusCulture = new CultureInfo("be-BY");
            Console.WriteLine("bot started");
           
            var botThread = new Thread(async () =>
            {
                Thread.CurrentThread.CurrentCulture = belarusCulture;
                Thread.CurrentThread.CurrentUICulture = belarusCulture;
                await telegramBot.StartReceivingMessages();
            });

            botThread.Start();
            
            Console.ReadLine();
        }
    }
}