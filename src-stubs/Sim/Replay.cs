using System.Collections.Generic;
using Drakefighting.Domain;

namespace Drakefighting.Sim
{
    public enum EventType { Tick, Hit, Crit, Miss, Dodge, KO, TagIn, TagOut, EffectApplied }

    public record SimEvent(
        int Round, int Tick,
        string ActorId, string TargetId,
        EventType Type,
        float ActorHp, float ActorSta, float TargetHp, float TargetSta,
        string? Effect = null, float Magnitude = 0f);

    public record SimResult(
        MatchSpec Spec,
        IReadOnlyList<SimEvent> Events,
        string WinnerTeamId,
        int RoundsElapsed);
}
