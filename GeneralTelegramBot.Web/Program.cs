using GeneralTelegramBot;
using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Repository;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using GeneralTelegramBot.Web;
using GeneralTelegramBot.Web.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);


var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

// Dummy business-logic service
builder.Services.AddScoped<TelegramMessageHandler>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddDbContext<GeneralTelegramBotDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

app.UseStaticFiles();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthorization();

var token = botConfig.BotToken;
app.MapControllerRoute(name: "tgwebhook",
    pattern: $"bot/{token}",
    new { controller = "Webhook", action = "Post" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Photo}/{action=Index}/{id?}");

app.Run();
