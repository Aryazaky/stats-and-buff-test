using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Active Prerequisite",
        menuName = "Stat System/Modifiers/Active Prerequisites", order = 0)]
    public class TimedModifierActivePrerequisiteSO : ModifierActivePrerequisiteSO
    {
        private enum ActivationRequirement
        {
            AlwaysActive,
            TickBased,
            Timed,
            TimedOrTickBased
        }

        [SerializeField] private ActivationRequirement activationRequirement;
        [SerializeField] private int tickDelay = 0;
        [SerializeField] private int tickDuration = 3;
        [SerializeField] private int timeDelay = 0;
        [SerializeField] private int timeDuration = 2;

        protected override bool ActivePrerequisite(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
        {
            return activationRequirement switch
            {
                ActivationRequirement.AlwaysActive => true,
                ActivationRequirement.TickBased => TickBasedActivation(contexts, trigger),
                ActivationRequirement.Timed => TimedActivation(contexts, trigger),
                ActivationRequirement.TimedOrTickBased => TimedOrTickBasedActivation(contexts, trigger),
                _ => false
            };
        }

        private bool TickBasedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
        {
            if (contexts.ModifierMetadata is ITickableMetadata tickableMetadata)
            {
                if (tickableMetadata.TotalTicksElapsed < tickDelay)
                    return false; // Delay period

                bool allow = tickableMetadata.TotalTicksElapsed < (tickDelay + tickDuration);
                if (!allow)
                {
                    trigger.Expire();
                }

                return allow;
            }

            return false;
        }

        private bool TimedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
        {
            if (contexts.ModifierMetadata is IAgeMetadata ageMetadata)
            {
                if (ageMetadata.Age < timeDelay)
                    return false; // Delay period

                bool allow = ageMetadata.Age < (timeDelay + timeDuration);
                if (!allow)
                {
                    trigger.Expire();
                }

                return allow;
            }

            return false;
        }

        private bool TimedOrTickBasedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
        {
            bool timeExpired = false;
            bool tickExpired = false;

            if (contexts.ModifierMetadata is IAgeMetadata ageMetadata)
            {
                if (ageMetadata.Age >= (timeDelay + timeDuration))
                    timeExpired = true;
            }

            if (contexts.ModifierMetadata is ITickableMetadata tickableMetadata)
            {
                if (tickableMetadata.TotalTicksElapsed >= (tickDelay + tickDuration))
                    tickExpired = true;
            }

            if (timeExpired || tickExpired)
            {
                trigger.Expire();
                return false;
            }

            return true;
        }
    }
}