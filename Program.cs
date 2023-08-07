using CollegeScorePredictor;
using CollegeScorePredictor.AIModels.OverPrediction;
using CollegeScorePredictor.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Predictor");
builder.Services.AddHttpClient("espn", client =>
{
    client.BaseAddress = new Uri("https://site.api.espn.com/apis/site/v2/sports/football/college-football/");
});

builder.Services.AddHttpClient("action-network", client =>
{
    client.BaseAddress = new Uri("https://api.actionnetwork.com/web/v1/scoreboard/ncaaf?bookIds=15&division=FBS&seasonType=reg&week=");
    //client.DefaultRequestHeaders.Add("Authorization", "basic Y8L7=");
});

builder.Services.AddDbContextFactory<AppDbContext>((DbContextOptionsBuilder options) =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ConferenceTeamsService>();
builder.Services.AddSingleton<RecordGamesService>();
builder.Services.AddSingleton<GenerateSubModelService>();
builder.Services.AddSingleton<PredictOverPredictionService>();
builder.Services.AddSingleton<OverPredictionConferenceService>();
builder.Services.AddSingleton<BetHistoryService>();
builder.Services.AddSingleton<ActionBetService>();
builder.Services.AddSingleton<ActionConferenceBetService>();
builder.Services.AddSingleton<ActionCombinedBetService>();
builder.Services.AddSingleton<CombinedBetHistoryService>();
builder.Services.AddSingleton<ConferenceBetHistoryService>();
builder.Services.AddSingleton<BetHistoryCombinedService>();
builder.Services.AddSingleton<BothWinHistoryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
