using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NOWT.Objects;

namespace NOWT.Helpers;

public class ConfigHelper
{
    private static string ConfigPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "NOWT",
        "config.json"
    );

    public static async Task<ConfigData> LoadConfigAsync()
    {
        try
        {
            if (!File.Exists(ConfigPath))
            {
                return CreateDefaultConfig();
            }

            var json = await File.ReadAllTextAsync(ConfigPath);
            var config = JsonSerializer.Deserialize<ConfigData>(json);
            
            // Ensure all features exist
            if (config.FeatureFlags == null)
                config.FeatureFlags = GetDefaultFeatureFlags();
            
            return config ?? CreateDefaultConfig();
        }
        catch (Exception ex)
        {
            Constants.Log.Error("Failed to load config: {Message}", ex.Message);
            return CreateDefaultConfig();
        }
    }

    public static async Task SaveConfigAsync(ConfigData config)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(config, options);
            await File.WriteAllTextAsync(ConfigPath, json);
        }
        catch (Exception ex)
        {
            Constants.Log.Error("Failed to save config: {Message}", ex.Message);
        }
    }

    private static ConfigData CreateDefaultConfig()
    {
        return new ConfigData
        {
            Cooldown = 10,
            WebSocketTrackingEnabled = false,
            FeatureFlags = GetDefaultFeatureFlags()
        };
    }

    private static Dictionary<string, bool> GetDefaultFeatureFlags()
    {
        return new Dictionary<string, bool>
        {
            { "ShowHeadshotPercent", true },
            { "ShowWinRate", true },
            { "ShowLeaderboard", true },
            { "ShowPartyIndicator", true },
            { "ShowAccountLevel", true },
            { "ShowPeakRank", true },
            { "ShowPreviousRank", true },
            { "ShowMatchHistory", true },
            { "ShowSkin", true },
            { "DiscordRpc", false },
            { "AutoHideLeaderboard", true },
            { "ShortRanks", false }
        };
    }
}

public class ConfigData
{
    public int Cooldown { get; set; } = 10;
    public bool WebSocketTrackingEnabled { get; set; } = false;
    public Dictionary<string, bool> FeatureFlags { get; set; } = new();
}
