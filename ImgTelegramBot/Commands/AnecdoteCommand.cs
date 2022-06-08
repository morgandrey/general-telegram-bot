﻿using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImgTelegramBot.Commands;

public static class AnecdoteCommand
{
    private const int MaxAnecdote = 1140;
    public static async Task Execute(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, GetRandomAnecdote());
    }

    private static string GetRandomAnecdote()
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
}