// Add NUnit via your test project later; this is just the shape.
using Drakefighting.Domain;
using Drakefighting.Sim;
using System.Collections.Generic;

namespace Drakefighting.Tests
{
    public class FightSimTests
    {
        // [Test]  // Uncomment once NUnit is referenced
        public void OneVOne_DeterministicReplay()
        {
            var dA = new DrakeLoadout(new DrakeId("A"), "UncleDolan",
                new DrakeStats { SPD=70, STA=65, POW=60, SMT=55, Mood=10, Luck=20 }, Style.Brawler,
                new List<string> { "BreadHeal" });
            var dB = new DrakeLoadout(new DrakeId("B"), "Gooby",
                new DrakeStats { SPD=55, STA=70, POW=58, SMT=65, Mood=-5, Luck=10 }, Style.Tactician,
                new List<string>());

            var spec = new MatchSpec(MatchMode.OneVOne,
                new TeamLoadout(new TeamId("TA"), MatchMode.OneVOne, new List<DrakeLoadout> { dA }),
                new TeamLoadout(new TeamId("TB"), MatchMode.OneVOne, new List<DrakeLoadout> { dB }),
                seed: 12345);

            var sim = new FightSim();
            var result1 = sim.Run(spec);
            var result2 = sim.Run(spec);

            // Assert.AreEqual(result1.WinnerTeamId, result2.WinnerTeamId);
            // Assert.AreEqual(result1.Events.Count, result2.Events.Count);
            // Additional deep checks can compare a few key events.
        }
    }
}
