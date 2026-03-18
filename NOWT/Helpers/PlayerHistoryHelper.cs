using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NOWT.Objects;

namespace NOWT.Helpers;

public static class PlayerHistoryHelper
{
    private static string HistoryPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "NOWT",
        "player_history.json"
    );

    private static Dictionary<string, List<MatchHistoryEntry>> _history = new();

    public static async Task LoadHistoryAsync()
    {
        try
        {
            if (!File.Exists(HistoryPath))
            {
                _history = new Dictionary<string, List<MatchHistoryEntry>>();
                return;
            }

            var json = await File.ReadAllTextAsync(HistoryPath);
            _history = JsonSerializer.Deserialize<Dictionary<string, List<MatchHistoryEntry>>>(json) 
                      ?? new Dictionary<string, List<MatchHistoryEntry>>();
        }
        catch
        {
            _history = new Dictionary<string, List<MatchHistoryEntry>>();
        }
    }

    public static async Task SaveHistoryAsync()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(HistoryPath));
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(_history, options);
            await File.WriteAllTextAsync(HistoryPath, json);
        }
        catch
        {
            // Ignore save errors
        }
    }

    public static int GetMatchesPlayedTogether(string playerId)
    {
        if (_history.TryGetValue(playerId, out var entries))
        {
            return entries.Count;
        }
        return 0;
    }

    public static string GetLastPlayedTime(string playerId)
    {
        if (_history.TryGetValue(playerId, out var entries) && entries.Count > 0)
        {
            var lastEntry = entries.Last();
            var timeDiff = DateTime.Now - lastEntry.Timestamp;
            
            if (timeDiff.TotalDays < 1)
                return "Today";
            if (timeDiff.TotalDays < 7)
                return $"{(int)timeDiff.TotalDays} days ago";
            if (timeDiff.TotalDays < 30)
                return $"{(int)(timeDiff.TotalDays / 7)} weeks ago";
            
            return $"{(int)(timeDiff.TotalDays / 30)} months ago";
        }
        return "Never";
    }

    public static void AddMatchEntry(string playerId, MatchHistoryEntry entry)
    {
        if (!_history.ContainsKey(playerId))
        {
            _history[playerId] = new List<MatchHistoryEntry>();
        }

        _history[playerId].Add(entry);

        // Keep only last 50 matches per player
        if (_history[playerId].Count > 50)
        {
            _history[playerId].RemoveRange(0, _history[playerId].Count - 50);
        }
    }

    public static void AddMatchEntries(List<string> playerIds, MatchHistoryEntry entry)
    {
        foreach (var playerId in playerIds)
        {
            AddMatchEntry(playerId, entry);
        }
    }
}
