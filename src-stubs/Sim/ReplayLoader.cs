using System.IO;
using System.Text.Json;

namespace Drakefighting.Sim
{
    public static class ReplayLoader
    {
        public static SimResult Load(string path)
            => JsonSerializer.Deserialize<SimResult>(File.ReadAllText(path))!;
    }
}
