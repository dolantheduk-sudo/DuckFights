using System.IO;
using System.Text.Json;

namespace Drakefighting.Sim
{
    public static class ReplaySerializer
    {
        private static readonly JsonSerializerOptions Opts = new()
        {
            WriteIndented = true
        };

        public static void Save(SimResult result, string path)
        {
            var json = JsonSerializer.Serialize(result, Opts);
            File.WriteAllText(path, json);
        }
    }
}
