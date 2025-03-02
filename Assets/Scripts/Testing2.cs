using System.Linq;
using ConcreteClasses.Modifiers;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

public class Testing2 : MonoBehaviour
{
    private Stats _base;
    private WorldContexts _worldContexts;
    void Start()
    {
        _worldContexts = new WorldContexts();
        
        var hp = new Stat(StatType.Health, 10, 0, 100);
        var hpRegen = new Stat(StatType.HealthRegen, 2);
        var strength = new Stat(StatType.Strength, 5);

        _base = new Stats(hp, hpRegen, strength);

        var hpRegenStatus = new StatModifier(
            StatType.Health, 
            activePrerequisite: StatModifierActivationConditions.InvokeOnce, 
            operation: StatModifierOperations.Regen
            );
        
        _base.Mediator.AddModifier(hpRegenStatus);
        Debug.Log($"init: {_base}");
        _base[StatType.Health] -= 5;
        Debug.Log($"before: {_base}");
        _base.Update(_worldContexts);
        Debug.Log($"update1: {_base}");
        _base.Update(_worldContexts);
        Debug.Log($"update2: {_base}");
        _base.Update(_worldContexts);
        Debug.Log($"update3: {_base}");
        _base.Update(_worldContexts);
        Debug.Log($"update4: {_base}");
        _base.Update(_worldContexts);
        _base[StatType.Health] -= 5;
        Debug.Log($"update5-5: {_base}");
        _base.Update(_worldContexts);
        Debug.Log($"update6: {_base}");
    }
}

public static class StatModifierActivationConditions
{
    public static bool InvokeOnce(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        return contexts.ModifierMetadata.InvokedCount < 3;
    }
    
    public static bool AlwaysActive(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        return true;
    }
    
    public static bool ActiveFor2Seconds(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        if (Time.time - contexts.ModifierMetadata.CreatedTime > 2)
        {
            trigger.Expire();
        }
        return true;
    }
    
    public static bool OncePerSecondActivationAsLongAsQueriedStatsAreLessThanMaxElseEndInstantly(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        var query = contexts.Query;
        var stats = query.TemporaryStats;
        var hasAtLeastOneSecondPassed = contexts.ModifierMetadata.LastInvokeTime > 1;
        var allQueriedStatsAreLessThanMax = query.Types.All(type => stats[type].Value < (stats[type].Max ?? float.MaxValue)); // query.Types is guaranteed to be types available in the stats. 
        var result = allQueriedStatsAreLessThanMax && hasAtLeastOneSecondPassed;
        return result;
    }
}

public static class StatModifierOperations
{
    public static void Regen(Modifier.Contexts contexts)
    {
        var temporaryStats = contexts.Query.TemporaryStats;
        var referenceStats = contexts.Query.ReferenceStats;

        temporaryStats[StatType.Health].Max += 10;
        referenceStats[StatType.Health] += referenceStats[StatType.HealthRegen].Value;
    }
}