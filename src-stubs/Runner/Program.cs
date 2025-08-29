using System;
using Drakefighting.Domain;
using Drakefighting.Sim;

namespace Drakefighting.Runner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var seed = args.Length > 0 && int.TryParse(args[0], out var s) ? s : 12345;
            var spec = SampleData.OneVOneSpec(seed);
            var sim  = new FightSim();
            var res  = sim.Run(spec);

            Console.WriteLine($"Winner: {res.WinnerTeamId}  Seed: {seed}");
            var path = $"replay_{seed}.json";
            ReplaySerializer.Save(res, path);
            Console.WriteLine($"Replay saved -> {path}  Events: {res.Events.Count}");
        }
    }
}
