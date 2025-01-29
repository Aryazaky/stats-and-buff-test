using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Concrete_Classes.Modifiers
{
    public class StatModifier : Modifier
    {
        private readonly Stat.StatType[] _types;
        public StatModifier(IEnumerable<Stat.StatType> types, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
        {
            _types = types.ToArray();
        }
        public StatModifier(Stat.StatType type, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
        {
            _types = new[] { type };
        }

        public override void Handle(StatQuery query)
        {
            if (query.Types.Intersect(_types).Any())
            {
                base.Handle(query);
            }
        }
    }
}