using GeneralTelegramBot.Contracts;
using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GeneralTelegramBot.Commands;

public class AnecdoteCommand : TelegramCommand
{
    private const int MaxAnecdote = 1140;

    public override string Name => "/anek";

    public override async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
            GetRandomAnecdote(),
            disableNotification: true,
            cancellationToken: cancellationToken);
    }

    public static string GetRandomAnecdote()
    {
        var anecdoteContent = string.Empty;
        try
        {
            var rand = new Random();
            var web = new HtmlWeb();
            var doc = web.Load($"https://baneks.ru/{rand.Next(1, MaxAnecdote)}");
            var htmlNodes = doc.DocumentNode.Descendants("meta").ToList();
            anecdoteContent = htmlNodes[3].Attributes["content"].Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return anecdoteContent;
    }


    public override bool Contains(Message message)
    {
        return message.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}