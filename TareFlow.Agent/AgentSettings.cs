using System.IO;
using System.Text.Json;
using TareFlow.Core;

namespace TareFlow.Agent;

/// <summary>Kantar köprüsü ayarları (JSON dosyasında saklanır).</summary>
public sealed class AgentSettings
{
    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 9600;
    public int ListenPort { get; set; } = ScaleProtocol.DefaultPort;
    public bool AutoStart { get; set; } = true;

    private static string FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TareFlow", "agent-settings.json");

    public static AgentSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var s = JsonSerializer.Deserialize<AgentSettings>(json);
                if (s is not null)
                    return s;
            }
        }
        catch { /* bozuk ayar dosyası → varsayılan */ }
        return new AgentSettings();
    }

    public void Save()
    {
        var dir = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(dir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true }));
    }
}
