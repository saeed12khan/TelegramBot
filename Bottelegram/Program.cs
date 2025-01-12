using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot; 
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        string botToken = "7611783358:AAHsSrt4wxjgVSB0qxxYaosTTXBgFtyMS2M";

        var botClient = new TelegramBotClient(botToken);

        Console.WriteLine("Bot is starting...");

        // Set up a cancellation token
        using var cts = new CancellationTokenSource();

        // Start receiving updates
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() },
            cancellationToken: cts.Token
        );

        var botInfo = await botClient.GetMeAsync();
        Console.WriteLine($"Bot started: @{botInfo.Username}");

        Console.ReadLine();
        cts.Cancel(); // Stop bot on exit
    }

    // Handle incoming updates
    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;

        string chatId = message.Chat.Id.ToString();
        string messageText = message.Text;

        Console.WriteLine($"Received message from {chatId}: {messageText}");

        if (messageText == "/start")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Welcome to the bot! How can I help you?",
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"You said: {messageText}",
                cancellationToken: cancellationToken
            );
        }
    }

    // Handle errors
    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}
