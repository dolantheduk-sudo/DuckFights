using System.Collections.Generic;

namespace Drakefighting.Domain
{
    public class TeamLoadout
    {
        public List<string> DrakeIds = new(); // roster by ID
        public MatchMode Mode;
    }
}
