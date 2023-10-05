namespace StudWeatherBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBot telegramBot = new TelegramBot("6405433807:AAG1cCCh_cEmX76-rMfYgvOR6qrbkTX550o");

            var botThread = new Thread(async () =>
            {
                await telegramBot.StartReceivingMessages();
            });

            botThread.Start();
            
            Console.ReadLine();
        }
    }
}