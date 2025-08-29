using System.Collections.Generic;

namespace Drakefighting.Domain
{
    public record DrakeId(string Value);
    public record TeamId(string Value);

    public enum Style { Brawler, Tactician, Counter, Wildcard }

    public struct DrakeStats
    {
        public int SPD, STA, POW, SMT, Mood, Luck; // 0..100 baseline
    }

    public record DrakeLoadout(
        DrakeId Id,
        string Archetype,             // e.g., "UncleDolan"
        DrakeStats Stats,
        Style Style,
        IReadOnlyList<string> Traits  // e.g., "BreadHeal","WebStun"
    );

    public record TeamLoadout(TeamId Id, MatchMode Mode, IReadOnlyList<DrakeLoadout> Ducks);

    public record MatchSpec(
        MatchMode Mode,
        TeamLoadout TeamA,
        TeamLoadout TeamB,
        int Seed,               // determinism
        bool Simultaneous3v3 = false // variant: all-at-once vs sequential
    );
}
