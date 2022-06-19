using GeneralTelegramBot;
using GeneralTelegramBot.DataAccess.Data;
using GeneralTelegramBot.DataAccess.Repository;
using GeneralTelegramBot.DataAccess.Repository.IRepository;
using GeneralTelegramBot.Web.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var telegramToken = builder.Configuration["TelegramToken"];

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(telegramToken, httpClient));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<TunnelService>();
}

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

app.MapControllerRoute(name: "tgwebhook",
    pattern: $"bot/{telegramToken}",
    new { controller = "Webhook", action = "Post" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Photo}/{action=Index}/{id?}");

app.Run();