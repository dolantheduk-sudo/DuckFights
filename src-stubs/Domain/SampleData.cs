using System.Collections.Generic;

namespace Drakefighting.Domain
{
    public static class SampleData
    {
        public static DrakeLoadout DolanA => new(
            new DrakeId("A"), "UncleDolan",
            new DrakeStats { SPD=70, STA=65, POW=60, SMT=55, Mood=10, Luck=20 },
            Style.Brawler,
            new List<string> { "BreadHeal" });

        public static DrakeLoadout GoobyB => new(
            new DrakeId("B"), "Gooby",
            new DrakeStats { SPD=55, STA=70, POW=58, SMT=65, Mood=-5, Luck=10 },
            Style.Tactician,
            new List<string>());

        public static MatchSpec OneVOneSpec(int seed)
        {
            var ta = new TeamLoadout(new TeamId("TA"), MatchMode.OneVOne, new List<DrakeLoadout> { DolanA });
            var tb = new TeamLoadout(new TeamId("TB"), MatchMode.OneVOne, new List<DrakeLoadout> { GoobyB });
            return new MatchSpec(MatchMode.OneVOne, ta, tb, seed);
        }
    }
}
