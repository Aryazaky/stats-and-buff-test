using System;
using System.Linq;
using ConcreteClasses.Modifiers;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Collections.Generic;
using StatSystem.Concrete_Classes.Expiry_Notifiers;
using StatSystem.Modifiers;
using UnityEngine;
using Modifier = StatSystem.Modifiers.Modifier;

public class Testing : MonoBehaviour
{
    // private Stats _stats;
    // private StatCollection _statCollection;
    // private StatCollection<MutableStat> _base;
    // private Mediator _booster;
    // private StatCollection<MutableStat> _boosted; // Base stats after equip
    // private StatCollection<MutableStat> _scaled;
    // private Mediator _mediator = new();
    // private WorldContexts _worldContexts = new WorldContexts(); // Just imagine this is getting the world contexts from somewhere else
    //
    // void Start()
    // {
    //     var hp = new Stat(StatType.Health, value: 10, min: 0, max: 50);
    //     var mana = new Stat(StatType.Mana, 5, 0, 10);
    //     var strength = new Stat(StatType.Strength, 10); // Uncapped stats is also possible!
    //
    //     _base = new StatCollection<MutableStat>(new MutableStat(hp), new MutableStat(mana), new MutableStat(strength));
    //     Debug.Log(_base);
    //     _boosted = new StatCollection<MutableStat>(_base);
    //     _boosted[StatType.Health].Max += 10; // Expected result: 10/60
    //     _boosted[StatType.Health].Value += 100; // Expected result: 60/60
    //     Debug.Log(_boosted);
    //     
    //     _statCollection = new StatCollection(hp, mana, strength);
    //     
    //     _stats = new Stats(_worldContexts, hp, mana); // This is a params. Can put any number of stats. Duplicates types get a last one survive treatment. Keep in mind, once a stats object is created, no new stat types can be added or removed. 
    //     
    //     var modifier = new StatModifier( // We're using a StatModifier class, but you can create your own!
    //         StatType.Health, // Also accept a collection of stat types!
    //         operation: ExampleOperations, 
    //         activePrerequisite: ExampleIsHealthBelowHalf,
    //         priority: Modifier.PriorityType.Boost // Example setting priority in case there's multiple modifiers. This is an integer value. 
    //     );
    //     
    //     // Example adding world context
    //     _worldContexts.Add(new ExampleIsRaining());
    //
    //     // You can expire them externally with ExpiryNotifier class like these, or use IExpireTrigger that gets passed on to activePrerequisite
    //     var expiryNotifier = new InvokeLimitExpiryNotifier(3); // You can create a custom class. For example, to auto expire after 3 turns, or after 5 seconds, etc. 
    //     expiryNotifier.TrackModifier(modifier);
    //     
    //     _mediator.AddModifier(modifier);
    //     var query = new Query(_statCollection, _worldContexts);
    //     _mediator.PerformQuery(query);
    //     _statCollection[StatType.Health] = query.Stats[StatType.Health];
    //
    //     _stats.Mediator.AddModifier(modifier);
    //     Debug.Log($"0:Health: {_stats[StatType.Health]}");
    //     _stats.Update(TODO);
    //     Debug.Log($"1:Health: {_stats[StatType.Health]}");
    //     _stats.Update(TODO);
    //     Debug.Log($"2:Health: {_stats[StatType.Health]}");
    //     _stats.Update(TODO);
    //     Debug.Log($"3:Health: {_stats[StatType.Health]}");
    //     _stats.Update(TODO);
    //     Debug.Log($"4:Health: {_stats[StatType.Health]}");
    //     _stats.Update(TODO);
    //     Debug.Log($"5:Health: {_stats[StatType.Health]}");
    //     return;
    //
    //     bool ExampleIsHealthBelowHalf(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    //     {
    //         // var health = contexts.QueryArgs.Query.Stats[Stat.StatType.Health]; // Unsafe as there might not be a health stat
    //         if (contexts.Query.Stats.TryGetStat(StatType.Health, out var health))
    //         {
    //             float currentHealth = health.Value;
    //             float maxHealth = health.Max ?? float.MaxValue;
    //             return currentHealth < (maxHealth / 2);
    //         }
    //         else return false;
    //     }
    //
    //     bool ExampleUsingContexts(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    //     {
    //         return contexts.Query.WorldContexts.Contains<ExampleIsRaining>();
    //     }
    //
    //     bool ExampleOncePerSecondActivationAsLongAsQueriedStatsAreMoreThanZeroElseEndInstantly(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger)
    //     {
    //         var query = contexts.Query;
    //         var stats = query.Stats;
    //         var hasAtLeastOneSecondPassed = contexts.ModifierMetadata.LastInvokeTime > 1;
    //         var allQueriedStatsAreMoreThan0 = query.Types.All(type => stats[type].Value > 0); // query.Types is guaranteed to be types available in the stats. 
    //         var result = allQueriedStatsAreMoreThan0 && hasAtLeastOneSecondPassed;
    //         if (!result) trigger.Expire(); // Example how to kill this modifier after it's no longer useful. 
    //         return result;
    //     }
    //
    //     void ExampleOperations(Modifier.Contexts contexts)
    //     {
    //         var stats = contexts.Query.Stats;
    //         // Capturing outside variables like this are not recommended. Make sure you know what you're doing. Avoid calling Update() to avoid stackoverflow. 
    //         // var statsRef = _stats; // To access the base stats, use Query.BaseStats instead. 
    //         var statsRef = contexts.Query.BaseStats;
    //         foreach (var type in contexts.Query.Types)
    //         {
    //             // Uncomment one of these to try out their effect
    //             
    //             // How to: Temporary stat change, offset by 1
    //             // stats[type].Value += 1;
    //             
    //             // How to: Temporary stat change, offset by how many times this effect has been invoked
    //             // stats[type].Value += 1 + contexts.ModifierMetadata.InvokedCount;
    //             
    //             // How to: Temporary stat change, set to 1
    //             // stats[type].Value = 1;
    //             
    //             // How to: Permanently change the stats by replacing the stats in the ref
    //             // statsRef[type] = statsRef[type].SetValue(1);
    //             
    //             // How to: Permanently change the stats, by offset
    //             // statsRef[type] += 1;
    //             
    //             // NOTE: changing StatsRef will change the base stats.
    //             // Since the stats indexer accessed the processed stats,
    //             // and processed stats aren't updated with the new base stats until Update() is called,
    //             // the changes will only be visible after 1 more Update(). 
    //             // This has little-to-no effect on real-time games. But it will affect turn based games.
    //             // For cases like these, there are multiple ways to do it.
    //             // First is by using Query to pass in what the current events are,
    //             // or using a global static instance that stores the current active events.
    //             // But these will be need to be checked on each unity update. For a true event-based triggers, I don't know how. 
    //         }
    //     }
    // }
    //
    // private void Update()
    // {
    //     _stats.Update(TODO);
    //
    //     // if (Input.GetKeyUp(KeyCode.Q))
    //     // {
    //     //     _stats.Base[Stat.StatType.Health] -= 5;
    //     // }
    // }
}


public class ExampleIsRaining : IWorldContext
{
    public string Name => "Hungry player";
}