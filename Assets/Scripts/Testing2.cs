using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using StatSystem.UnityAdapters;
using UnityEngine;

public class Testing2 : MonoBehaviour
{
    private Stats _base;
    public StatWrapper statWrapper;
    public StatCollectionWrapper statCollectionWrapper;
    private WorldContexts _worldContexts;
    // void Start()
    // {
    //     _worldContexts = new WorldContexts();
    //     
    //     var hp = new Stat(StatType.Health, 10, 0, 100);
    //     var hpRegen = new Stat(StatType.HealthRegen, 2);
    //     var strength = new Stat(StatType.Strength, 5);
    //
    //     _base = new Stats(hp, hpRegen, strength);
    //
    //     var hpRegenStatus = new Modifier(
    //         activePrerequisite: StatModifierActivationConditions.InvokeOnce, 
    //         operation: StatModifierOperations.Regen
    //         );
    //     
    //     _base.Mediator.AddModifier(hpRegenStatus);
    //     Debug.Log($"init: {_base}");
    //     _base[StatType.Health] -= 5;
    //     Debug.Log($"before: {_base}");
    //     _base.Update(_worldContexts);
    //     Debug.Log($"update1: {_base}");
    //     _base.Update(_worldContexts);
    //     Debug.Log($"update2: {_base}");
    //     _base.Update(_worldContexts);
    //     Debug.Log($"update3: {_base}");
    //     _base.Update(_worldContexts);
    //     Debug.Log($"update4: {_base}");
    //     _base.Update(_worldContexts);
    //     _base[StatType.Health] -= 5;
    //     Debug.Log($"update5-5: {_base}");
    //     _base.Update(_worldContexts);
    //     Debug.Log($"update6: {_base}");
    // }
}

// public static class StatModifierActivationConditions
// {
//     public static bool InvokeOnce(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
//     {
//         if (contexts.ModifierMetadata is ITickableMetadata tickableMetadata)
//         {
//             // return tickableMetadata.TotalTicksElapsed < 3; // not expiring them will cause them to stay active forever, even if the effects does not proc
//             
//             // The recommended way:
//             var allow = tickableMetadata.TotalTicksElapsed < 3;
//             if (!allow)
//             {
//                 trigger.Expire();
//             }
//
//             return allow;
//         }
//
//         return false;
//     }
//     
//     public static bool AlwaysActive(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
//     {
//         return true;
//     }
//     
//     public static bool ActiveFor2Seconds(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
//     {
//         if (contexts.ModifierMetadata is IAgeMetadata { Age: > 2 })
//         {
//             trigger.Expire();
//         }
//         return true;
//     }
// }

// public static class StatModifierOperations
// {
//     /// <summary>
//     /// Regenerates health based on the Health Regen stat. 
//     /// This effect triggers on each tick.
//     /// </summary>
//     public static void Regen(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats; // Temporary stats (used for displaying modified values)
//         var baseStats = contexts.Query.BaseStats; // Permanent stats (updated across ticks)
//
//         if (contexts.ModifierMetadata is ITickableMetadata { HasUnprocessedTick: true } tickableMetadata)
//         {
//             var regenValue = baseStats[StatType.HealthRegen].Value; // Warning: it's unknown if the stats has a HealthRegen property. Use baseStats.TryGetStat(StatType.HealthRegen, out var stat) instead
//
//             // Update both reference and temporary stats for lasting effects
//             baseStats[StatType.Health] += regenValue; // Warning: same as the warning above. 
//             displayedStats[StatType.Health] += regenValue;
//
//             tickableMetadata.MarkTickProcessed(); // Prevents multiple applications within the same tick
//         }
//     }
//
//     /// <summary>
//     /// Applies a temporary Strength buff (+5 Strength per tick).
//     /// This buff does NOT modify the base stats, only affects calculations.
//     /// </summary>
//     public static void TemporaryStrengthBuff(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats;
//
//         // Only modifying queriedStats means this buff does NOT persist between ticks.
//         displayedStats[StatType.Strength] += 5;
//     }
//
//     /// <summary>
//     /// Applies a stacking Strength buff (+1 per tick) that permanently increases Strength.
//     /// </summary>
//     public static void StackingStrengthBuff(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats;
//         var baseStats = contexts.Query.BaseStats;
//
//         if (contexts.ModifierMetadata is ITickableMetadata { HasUnprocessedTick: true } tickableMetadata)
//         {
//             baseStats[StatType.Strength] += 1; // Permanently modifies Strength
//             displayedStats[StatType.Strength] += 1; // Ensures immediate visual feedback
//
//             tickableMetadata.MarkTickProcessed();
//         }
//     }
//
//     /// <summary>
//     /// Applies a temporary debuff that reduces Strength by 2 while active.
//     /// This does NOT persist beyond a single query.
//     /// </summary>
//     public static void TemporaryStrengthDebuff(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats;
//
//         // This only affects displayed values; it does NOT persist after removal
//         displayedStats[StatType.Strength] -= 2;
//     }
//
//     /// <summary>
//     /// Applies a burn effect that deals damage over time (-3 HP per tick).
//     /// This effect modifies both reference and temporary stats for consistency.
//     /// </summary>
//     public static void Burn(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats;
//         var baseStats = contexts.Query.BaseStats;
//
//         if (contexts.ModifierMetadata is ITickableMetadata { HasUnprocessedTick: true } tickableMetadata)
//         {
//             var burnDamage = 3;
//
//             baseStats[StatType.Health] -= burnDamage;
//             displayedStats[StatType.Health] -= burnDamage;
//
//             tickableMetadata.MarkTickProcessed();
//         }
//     }
//
//     /// <summary>
//     /// Applies a temporary Max HP buff (+20 Max HP while active).
//     /// This effect only modifies temporary stats and does not persist after removal.
//     /// </summary>
//     public static void TemporaryMaxHPBuff(Modifier.Contexts contexts)
//     {
//         var displayedStats = contexts.Query.QueriedStats;
//
//         // Only modifies temporary stats, meaning it disappears after effect duration
//         displayedStats[StatType.Health].Max += 20;
//     }
// }
