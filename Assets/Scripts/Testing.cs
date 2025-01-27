using System;
using System.Linq;
using StatSystem;
using StatSystem.Concrete_Classes.Expiry_Notifiers;
using StatSystem.Concrete_Classes.Modifiers;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Stats _stats;
    private WorldContexts worldContexts = new WorldContexts(); // Just imagine this is getting the world contexts from somewhere else

    void Start()
    {
        var hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        var mana = new Stat(Stat.StatType.Mana, 5, 0, 10);
        _stats = new Stats(hp, mana); // This is a params. Can put any number of stats. Duplicates types get a last one survive treatment

        var modifier = new StatModifier(
            Stat.StatType.Health,
            operation: ExampleOperations,
            activePrerequisite: ExampleIsHealthBelowHalf
        );
        
        // Example adding world context
        worldContexts.Add(new ExampleIsRaining());

        // You can expire them externally with ExpiryNotifier class like these, or use IExpireTrigger that gets passed on to activePrerequisite
        var expiryNotifier = new InvokeLimitExpiryNotifier(3);
        expiryNotifier.TrackModifier(modifier);

        _stats.Mediator.AddModifier(modifier);
        Debug.Log($"0:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update(worldContexts);
        Debug.Log($"1:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update(worldContexts);
        Debug.Log($"2:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update(worldContexts);
        Debug.Log($"3:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update(worldContexts);
        Debug.Log($"4:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update(worldContexts);
        Debug.Log($"5:Health: {_stats[Stat.StatType.Health]}");
        return;

        bool ExampleIsHealthBelowHalf(Stat.Modifier.Contexts contexts, Stat.Modifier.IExpireTrigger trigger)
        {
            // var health = contexts.QueryArgs.Query.Stats[Stat.StatType.Health]; // Unsafe as there might not be a health stat
            if (contexts.Query.ModifiableStats.TryGetValue(Stat.StatType.Health, out var health))
            {
                float currentHealth = health.Value;
                float maxHealth = health.Max ?? float.MaxValue;
                return currentHealth < (maxHealth / 2);
            }
            else return false;
        }

        bool ExampleUsingContexts(Stat.Modifier.Contexts contexts, Stat.Modifier.IExpireTrigger trigger)
        {
            return contexts.Query.WorldContexts.Contains<ExampleIsRaining>();
        }

        bool ExampleOncePerSecondActivationAsLongAsQueriedStatsAreMoreThanZeroElseEndInstantly(Stat.Modifier.Contexts contexts, Stat.Modifier.IExpireTrigger trigger)
        {
            var query = contexts.Query;
            var stats = query.ModifiableStats;
            var hasAtLeastOneSecondPassed = contexts.ModifierMetadata.LastInvokeTime > 1;
            var allQueriedStatsAreMoreThan0 = query.Types.All(type => stats[type].Value > 0); // query.Types is guaranteed to be types available in the stats. 
            var result = allQueriedStatsAreMoreThan0 && hasAtLeastOneSecondPassed;
            if (!result) trigger.Expire(); // Example how to kill this modifier after it's no longer useful. 
            return result;
        }

        void ExampleOperations(Stat.Modifier.Contexts contexts)
        {
            var stats = contexts.Query.ModifiableStats;
            // Capturing outside variables like this are not recommended. Make sure you know what you're doing. Avoid calling Update() to avoid stackoverflow. 
            // var statsRef = _stats; // To access the base stats, use Query.BaseStats instead. 
            var statsRef = contexts.Query.BaseStats;
            foreach (var type in contexts.Query.Types)
            {
                // How to: Temporary stat change, offset by 1
                // stats[type].Value += 1;
                
                // How to: Temporary stat change, offset by how many times this effect has been invoked
                // stats[type].Value += 1 + contexts.ModifierMetadata.InvokedCount;
                
                // How to: Temporary stat change, set to 1
                // stats[type].Value = 1;
                
                // How to: Permanently change the stats by replacing the stats in the ref
                // statsRef[type] = statsRef[type].SetValue(1);
                
                // How to: Permanently change the stats, by offset
                // statsRef[type] += 1;
                
                // NOTE: changing StatsRef will change the base stats.
                // Since the stats indexer accessed the processed stats,
                // and processed stats aren't updated with the new base stats until Update() is called,
                // the changes will only be visible after 1 more Update(). 
                // This has little-to-no effect on real-time games. But it will affect turn based games.
                // For cases like these, there are multiple ways to do it. First is by using QueryArgs to pass in what the current events are,
                // or using a global static instance that stores the current active events.
                // But these will be need to be checked on each unity update. For a true event-based triggers, I don't know how. 
            }
        }
    }

    private void Update()
    {
        _stats.Update(worldContexts);
    }
}