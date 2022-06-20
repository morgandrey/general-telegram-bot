using System.Text.Json.Nodes;
using CliWrap;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GeneralTelegramBot.Web.Services
{
    public class TunnelService : BackgroundService
    {
        private readonly IServer server;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly IConfiguration config;
        private readonly IServiceProvider services;
        private readonly ILogger<TunnelService> logger;

        public TunnelService(
            IServer server,
            IHostApplicationLifetime hostApplicationLifetime,
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            ILogger<TunnelService> logger
        )
        {
            this.server = server;
            this.hostApplicationLifetime = hostApplicationLifetime;
            this.config = configuration;
            this.services = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await StopNgrokProcess(cancellationToken);

            await WaitForApplicationStarted();

            var localUrl = "http://morgan93-001-site1.gtempurl.com/";

            logger.LogInformation("Starting ngrok tunnel for {LocalUrl}", localUrl);
            var ngrokTask = StartNgrokTunnel(localUrl, cancellationToken);

            var publicUrl = await GetNgrokPublicUrl();
            logger.LogInformation("Public ngrok URL: {NgrokPublicUrl}", publicUrl);

            await ConfigureTelegramWebhook(publicUrl, cancellationToken);

            await ngrokTask;
        }

        private Task WaitForApplicationStarted()
        {
            var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            hostApplicationLifetime.ApplicationStarted.Register(() => completionSource.TrySetResult());
            return completionSource.Task;
        }

        private CommandTask<CommandResult> StopNgrokProcess(CancellationToken cancellationToken)
        {
            var ngrokTask = Cli.Wrap("taskkill")
                .WithArguments(args => args
                    .Add("/f")
                    .Add("/im")
                    .Add("ngrok.exe"))
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync(cancellationToken);
            return ngrokTask;
        }

        private CommandTask<CommandResult> StartNgrokTunnel(string localUrl, CancellationToken cancellationToken)
        {
            var ngrokTask = Cli.Wrap("Ngrok/ngrok.exe")
                .WithArguments(args => args
                    .Add("http")
                    .Add("--host-header=rewrite")
                    .Add(localUrl)
                    .Add("--log")
                    .Add("stdout"))
                .WithStandardOutputPipe(PipeTarget.ToDelegate(s => logger.LogDebug(s)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(s => logger.LogError(s)))
                .ExecuteAsync(cancellationToken);

            return ngrokTask;
        }

        private async Task<string> GetNgrokPublicUrl()
        {
            using var httpClient = new HttpClient();
            for (var ngrokRetryCount = 0; ngrokRetryCount < 10; ngrokRetryCount++)
            {
                logger.LogDebug("Get ngrok tunnels attempt: {RetryCount}", ngrokRetryCount + 1);

                try
                {
                    var json = await httpClient.GetFromJsonAsync<JsonNode>("http://127.0.0.1:4040/api/tunnels");
                    var publicUrl = json["tunnels"].AsArray()
                        .Select(e => e["public_url"].GetValue<string>())
                        .SingleOrDefault(u => u.StartsWith("https://"));
                    if (!string.IsNullOrEmpty(publicUrl)) return publicUrl;
                }
                catch
                {
                    // ignored
                }

                await Task.Delay(200);
            }

            throw new Exception("Ngrok dashboard did not start in 10 tries");
        }

        private async Task ConfigureTelegramWebhook(string hostAddress, CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = @$"{hostAddress}/bot/{config["TelegramToken"]}";
            logger.LogInformation("Setting webhook: {webhookAddress}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }
    }
}
