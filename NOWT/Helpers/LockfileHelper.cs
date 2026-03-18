using System;
using System.IO;
using System.Threading.Tasks;

namespace NOWT.Helpers;

public static class LockfileHelper
{
    private static string LockfilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Riot Games", "Riot Client", "Config", "lockfile"
    );

    public static async Task<LockfileData?> GetLockfileDataAsync()
    {
        try
        {
            if (!File.Exists(LockfilePath))
                return null;

            var content = await File.ReadAllTextAsync(LockfilePath);
            var parts = content.Split(':');
            
            if (parts.Length < 5)
                return null;

            return new LockfileData
            {
                Name = parts[0],
                Pid = int.Parse(parts[1]),
                Port = int.Parse(parts[2]),
                Password = parts[3],
                Protocol = parts[4]
            };
        }
        catch
        {
            return null;
        }
    }

    public static bool IsValorantRunning()
    {
        return File.Exists(LockfilePath);
    }
}

public class LockfileData
{
    public string Name { get; set; } = string.Empty;
    public int Pid { get; set; }
    public int Port { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Protocol { get; set; } = string.Empty;
}
