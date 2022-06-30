using GeneralTelegramBot.Contracts;
using GeneralTelegramBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace GeneralTelegramBot.Commands;

public class AudioCommand : TelegramCommand
{
    public override string Name => "/audio";

    public override async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var inputWavFilePath = GeneralUtils.CreateTempFilePath("wav");
        var outputOggFilePath = GeneralUtils.CreateTempFilePath("ogg");
        GeneralUtils.CreateAudioWavFile(inputWavFilePath, message.ReplyToMessage.Text);
        await GeneralUtils.StartFFMPEGProcess(inputWavFilePath, outputOggFilePath, cancellationToken);
        var audioDuration = GeneralUtils.GetWavFileDuration(inputWavFilePath).Seconds;
        await using var stream = File.OpenRead(outputOggFilePath);
        await botClient.SendVoiceAsync(
            chatId: message.Chat.Id,
            voice: stream,
            duration: audioDuration,
            disableNotification: true,
            cancellationToken: cancellationToken);
        await stream.DisposeAsync();
        File.Delete(inputWavFilePath);
        File.Delete(outputOggFilePath);
    }

    public override bool Contains(Message message)
    {
        return message.Text != null &&
               message.ReplyToMessage?.Text != null &&
               message.Text.Split(' ', '@')[0].Contains(Name);
    }
}