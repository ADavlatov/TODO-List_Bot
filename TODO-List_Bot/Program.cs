using Telegram.Bot;
using TODO_List_Bot;
using TODO_List_Bot.Services;

var builder = WebApplication.CreateBuilder(args);

var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

builder.Services.AddScoped<HandleUpdateService>();

//из Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    var token = botConfig.BotToken;
    endpoints.MapControllerRoute(name: "tgwebhook",
                                 pattern: $"bot/{token}",
                                 new { controller = "Webhook", action = "Post" });
    endpoints.MapControllers();
});

app.Run();