using System.Linq;
using ConcreteClasses.Modifiers;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Collections.Generic;
using StatSystem.Modifiers;
using UnityEngine;

public class Testing2 : MonoBehaviour
{
    private Stats<MutableStat> _base;
    private WorldContexts _worldContexts;
    void Start()
    {
        _worldContexts = new WorldContexts();
        
        var hp = new MutableStat(StatType.Health, 10, 0, 100);
        var hpRegen = new MutableStat(StatType.HealthRegen, 2);
        var strength = new MutableStat(StatType.Strength, 5);

        _base = new Stats<MutableStat>(hp, hpRegen, strength);

        var hpRegenStatus = new StatModifier(
            StatType.Health, 
            activePrerequisite: StatModifierActivationConditions.ActiveFor2Seconds, 
            operation: StatModifierOperations.Regen
            );
        
        _base.Mediator.AddModifier(hpRegenStatus);
        _base[StatType.Health] -= 5;
        Debug.Log(_base);
    }

    // Update is called once per frame
    void Update()
    {
        _base.Update(_worldContexts);
    }
}

public static class StatModifierActivationConditions
{
    public static bool AlwaysActive(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        return true;
    }
    
    public static bool ActiveFor2Seconds(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        if (contexts.ModifierMetadata.LastInvokeTime > 2)
        {
            trigger.Expire();
        }
        return true;
    }
    
    public static bool OncePerSecondActivationAsLongAsQueriedStatsAreLessThanMaxElseEndInstantly(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    {
        var query = contexts.Query;
        var stats = query.Stats;
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
        var stats = contexts.Query.Stats;
        var statsRef = contexts.Query.BaseStats;
        if (statsRef is Stats<MutableStat> s && s.Contains(StatType.Health, StatType.HealthRegen))
        {
            s[StatType.Health] += s[StatType.HealthRegen].Value;
            Debug.Log(s);
        }
    }
    
    public static void ExampleOperations(Modifier.Contexts contexts)
    {
        var stats = contexts.Query.Stats;
        var statsRef = contexts.Query.BaseStats;
        foreach (var type in contexts.Query.Types)
        {
            // Uncomment one of these to try out their effect
            
            // How to: Temporary stat change, offset by 1
            // stats[type].Value += 1;
            
            // How to: Temporary stat change, offset by how many times this effect has been invoked
            // stats[type].Value += 1 + contexts.ModifierMetadata.InvokedCount;
            
            // How to: Temporary stat change, set to 1
            // stats[type].Value = 1;
            
            // How to: Permanently change the stats by replacing the stats in the ref
            // statsRef[type] = statsRef[type].SetValue(1);
            
            // How to: Permanently change the stats, by offset

            if (statsRef[type] is MutableStat mutableStat)
            {
                mutableStat.Value += 1;
                Debug.Log(mutableStat);
            }
        }
    }
}