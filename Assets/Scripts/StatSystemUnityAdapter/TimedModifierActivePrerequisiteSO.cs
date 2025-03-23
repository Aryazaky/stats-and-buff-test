using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
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
        [SerializeField, Min(0)] private int tickDelay = 0;
        [SerializeField, Min(0)] private int tickDuration = 3;
        [SerializeField, Min(0)] private int tickInterval = 0;

        [SerializeField, Min(0)] private float timeDelay = 0f;
        [SerializeField, Min(0)] private float timeDuration = 2f;
        [SerializeField, Min(0)] private float timeInterval = 0;

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

        private bool TickBasedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger, bool allowExpire = true)
        {
            if (contexts.Metadata.TotalTicksElapsed < tickDelay)
                return false; // Delay period

            if (tickInterval > 0)
                return IsWithinPulse(contexts.Metadata.TotalTicksElapsed, tickDelay, tickDuration, tickInterval);

            bool allow = contexts.Metadata.TotalTicksElapsed < (tickDelay + tickDuration);
            if (!allow && allowExpire)
            {
                trigger.Expire();
            }

            return allow;
        }

        private bool TimedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger, bool allowExpire = true)
        {
            if (contexts.Metadata.Age < timeDelay)
                return false; // Delay period

            if (timeInterval > 0)
                return IsWithinPulse(contexts.Metadata.Age, timeDelay, timeDuration, timeInterval);

            bool allow = contexts.Metadata.Age < (timeDelay + timeDuration);
            if (!allow && allowExpire)
            {
                trigger.Expire();
            }

            return allow;
        }

        private bool TimedOrTickBasedActivation(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
        {
            bool tickActive = TickBasedActivation(contexts, trigger, false);
            bool timeActive = TimedActivation(contexts, trigger, false);

            if (!tickActive && !timeActive)
            {
                trigger.Expire();
                return false;
            }

            return true;
        }

        private static bool IsWithinPulse(float current, float delay, float duration, float interval)
        {
            if (current < delay)
                return false; // Still in initial delay

            float cycleStart = delay + Mathf.Floor((current - delay) / interval) * interval;
            return current >= cycleStart && current < (cycleStart + duration);
        }
    }
}