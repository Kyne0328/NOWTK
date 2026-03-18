using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NOWT.Helpers;
using NOWT.Objects;

namespace NOWT.ViewModels;

public partial class RoundStatsViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private int _countdownTime = 20;

    [ObservableProperty]
    private DispatcherTimer _countTimer;

    [ObservableProperty]
    private LoadingOverlay _overlay;

    [ObservableProperty]
    private string _refreshTime = "-";

    [ObservableProperty]
    private List<RoundStat> _roundStats;

    [ObservableProperty]
    private List<Player> _players;

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private bool _webSocketTrackingEnabled = false; // Disabled by default for safety

    [ObservableProperty]
    private string _webSocketWarning = "WebSocket tracking connects to Valorant's local game client. This is not officially supported by Riot Games and may result in account restrictions. Use at your own risk.";

    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cancellationTokenSource;
    
    // Player match history tracking
    private Dictionary<string, List<MatchHistoryEntry>> _playerMatchHistory = new();
    
    // Party tracking
    private Dictionary<string, string> _playerParties = new();

    public RoundStatsViewModel()
    {
        _countTimer = new DispatcherTimer();
        _countTimer.Tick += UpdateTimersAsync;
        _countTimer.Interval = new TimeSpan(0, 0, 1);

        Overlay = new LoadingOverlay
        {
            Header = "Loading",
            Content = "Getting Round Stats",
            IsBusy = false
        };

        RoundStats = new List<RoundStat>();
    }

    [ICommand]
    private void PassiveLoadAsync()
    {
        if (!_countTimer.IsEnabled)
            _countTimer.Start();
    }

    [ICommand]
    private async Task PassiveLoadCheckAsync()
    {
        if (!_countTimer.IsEnabled)
        {
            _countTimer.Start();
            await GetRoundStatsAsync().ConfigureAwait(false);
        }
    }

    [ICommand]
    private void StopPassiveLoadAsync()
    {
        CountTimer?.Stop();
        RefreshTime = "-";
    }

    private async void UpdateTimersAsync(object sender, EventArgs e)
    {
        RefreshTime = CountdownTime + "s";
        if (CountdownTime <= 0)
        {
            CountdownTime = 20;
            await GetRoundStatsAsync().ConfigureAwait(false);
        }

        CountdownTime--;
    }

    [ICommand]
    private async Task GetRoundStatsAsync()
    {
        Overlay = new LoadingOverlay
        {
            IsBusy = true,
            Header = "Loading",
            Content = "Getting Round Stats"
        };

        try
        {
            // TODO: Implement actual API call to get round stats
            // For now, use mock data
            await LoadMockRoundStatsAsync();
        }
        catch (Exception)
        {
            // Handle error
        }
        finally
        {
            Overlay.IsBusy = false;
        }
    }

    public event Action GoMatchEvent;

    [ICommand]
    private void GoMatch()
    {
        GoMatchEvent?.Invoke();
    }

    private async Task LoadMockRoundStatsAsync()
    {
        // This method now loads real data from Riot API
        await Task.Run(() =>
        {
            try
            {
                // For now, load mock round stats (real implementation would parse from match data)
                RoundStats = new List<RoundStat>
                {
                    new RoundStat { RoundNumber = 1, PlayerKills = 2, PlayerDeaths = 1, OpponentKills = 1, OpponentDeaths = 2 },
                    new RoundStat { RoundNumber = 2, PlayerKills = 1, PlayerDeaths = 2, OpponentKills = 2, OpponentDeaths = 1 },
                    new RoundStat { RoundNumber = 3, PlayerKills = 3, PlayerDeaths = 0, OpponentKills = 0, OpponentDeaths = 3 },
                    new RoundStat { RoundNumber = 4, PlayerKills = 0, PlayerDeaths = 3, OpponentKills = 3, OpponentDeaths = 0 },
                    new RoundStat { RoundNumber = 5, PlayerKills = 1, PlayerDeaths = 1, OpponentKills = 1, OpponentDeaths = 1 }
                };

                // Load players from current match (if available)
                // This would be populated from LiveMatch methods
            }
            catch (Exception ex)
            {
                Constants.Log.Error("Error loading round stats: {Message}", ex.Message);
            }
        });
    }

    // Method to update player stats from Riot API
    public async Task UpdatePlayerStatsAsync(List<Player> players)
    {
        foreach (var player in players)
        {
            try
            {
                // Get player stats (headshot %, KD)
                var stats = await LiveMatch.GetPlayerStatsAsync(player.PlayerUuid);
                if (stats != null)
                {
                    if (double.TryParse(stats.HeadshotPercent, out var hs))
                        player.HeadshotPercent = hs;
                    
                    if (double.TryParse(stats.KillDeathRatio, out var kd))
                        player.KillDeathRatio = kd;
                }

                // Get leaderboard position
                var leaderboardPos = await LiveMatch.GetLeaderboardPositionAsync(player.PlayerUuid);
                player.LeaderboardPosition = leaderboardPos;

                // Get match history for "times played together" feature
                var history = await LiveMatch.GetPlayerMatchHistoryAsync(player.PlayerUuid, 50);
                player.MatchesPlayedTogether = PlayerHistoryHelper.GetMatchesPlayedTogether(player.PlayerUuid);
                player.LastPlayedTime = PlayerHistoryHelper.GetLastPlayedTime(player.PlayerUuid);
            }
            catch (Exception ex)
            {
                Constants.Log.Error("Error updating player stats for {Player}: {Message}", 
                    player.PlayerName, ex.Message);
            }
        }
    }

    [ICommand]
    private async Task ConnectToWebSocketAsync()
    {
        // Safety warning: Connecting to Valorant's local WebSocket is not officially supported by Riot Games
        // and may result in account restrictions. Use at your own risk.
        
        if (!LockfileHelper.IsValorantRunning())
        {
            Constants.Log.Information("Valorant is not running");
            return;
        }

        var lockfileData = await LockfileHelper.GetLockfileDataAsync();
        if (lockfileData == null)
        {
            Constants.Log.Information("Could not read lockfile");
            return;
        }

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _webSocket = new ClientWebSocket();

            var uri = new Uri($"wss://127.0.0.1:{lockfileData.Port}");
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes($"riot:{lockfileData.Password}"));

            _webSocket.Options.SetRequestHeader("Authorization", $"Basic {password}");

            await _webSocket.ConnectAsync(uri, _cancellationTokenSource.Token);
            IsConnected = true;

            Constants.Log.Information("Connected to Valorant WebSocket");
            await ReceiveMessagesAsync();
        }
        catch (Exception ex)
        {
            Constants.Log.Error("WebSocket connection failed: {Message}", ex.Message);
            IsConnected = false;
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[4096];

        while (_webSocket?.State == WebSocketState.Open && !_cancellationTokenSource?.Token.IsCancellationRequested == true)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                ProcessWebSocketMessage(message);
            }
            catch (Exception ex)
            {
                Constants.Log.Error("Error receiving WebSocket message: {Message}", ex.Message);
                break;
            }
        }

        IsConnected = false;
    }

    private void ProcessWebSocketMessage(string message)
    {
        try
        {
            // Parse JSON message
            using JsonDocument doc = JsonDocument.Parse(message);
            var root = doc.RootElement;

            if (root.TryGetProperty("eventType", out JsonElement eventTypeElement))
            {
                var eventType = eventTypeElement.GetString();

                switch (eventType)
                {
                    case "player_kill":
                        HandlePlayerKill(root);
                        break;
                    case "round_start":
                        HandleRoundStart(root);
                        break;
                    case "round_end":
                        HandleRoundEnd(root);
                        break;
                    // Add more event types as needed
                }
            }
        }
        catch (Exception ex)
        {
            Constants.Log.Error("Error processing WebSocket message: {Message}", ex.Message);
        }
    }

    private void HandlePlayerKill(JsonElement eventElement)
    {
        // Extract kill information
        if (eventElement.TryGetProperty("data", out JsonElement dataElement))
        {
            var killer = dataElement.GetProperty("killer").GetString();
            var victim = dataElement.GetProperty("victim").GetString();
            
            // Update round stats based on kill
            // This is where you'd update the RoundStats list
        }
    }

    private void HandleRoundStart(JsonElement eventElement)
    {
        // Handle round start event
        // Could reset or initialize round stats
    }

    private void HandleRoundEnd(JsonElement eventElement)
    {
        // Handle round end event
        // Could finalize round stats
    }

    public void Disconnect()
    {
        _cancellationTokenSource?.Cancel();
        _webSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "User disconnected", CancellationToken.None);
        IsConnected = false;
    }

    public void Dispose()
    {
        Disconnect();
        _cancellationTokenSource?.Dispose();
        _webSocket?.Dispose();
    }
}
