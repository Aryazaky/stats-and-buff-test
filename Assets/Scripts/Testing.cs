using System;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private StatCollection _statCollection;

    void Start()
    {
        var hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        var mana = new Stat(Stat.StatType.Mana, 5, 0, 10);
        _statCollection = new StatCollection(hp, mana);

        var modifier = new StatModifier(
            Stat.StatType.Health,
            operation: ExampleOperations,
            activePrerequisite: IsHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(3);
        expiryNotifier.TrackModifier(modifier);

        _statCollection.Mediator.AddModifier(modifier);
        Debug.Log($"0:Health: {_statCollection[Stat.StatType.Health]}");
        _statCollection.Update();
        Debug.Log($"1:Health: {_statCollection[Stat.StatType.Health]}");
        _statCollection.Update();
        Debug.Log($"2:Health: {_statCollection[Stat.StatType.Health]}");
        _statCollection.Update();
        Debug.Log($"3:Health: {_statCollection[Stat.StatType.Health]}");
        _statCollection.Update();
        Debug.Log($"4:Health: {_statCollection[Stat.StatType.Health]}");
        return;

        bool IsHealthBelowHalf(Stat.Modifier.Contexts contexts, Stat.Modifier.IExpireTrigger trigger)
        {
            var health = contexts.QueryArgs.Query.Stats[Stat.StatType.Health]; // Unsafe as there might not be a health stat
            float currentHealth = health.Value;
            float maxHealth = health.Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        }

        bool ExampleOncePerSecondActivationAsLongAsQueriedStatsAreMoreThanZeroElseEndInstantly(Stat.Modifier.Contexts contexts, Stat.Modifier.IExpireTrigger trigger)
        {
            var query = contexts.QueryArgs.Query;
            var stats = query.Stats;
            var hasAtLeastOneSecondPassed = contexts.ModifierMetadata.LastInvokeTime > 1;
            var allQueriedStatsAreMoreThan0 = query.Types.All(type => stats[type].Value > 0); // query.Types is guaranteed to be types available in the stats. 
            var result = allQueriedStatsAreMoreThan0 && hasAtLeastOneSecondPassed;
            if (!result) trigger.Expire(); // Example how to kill the modifier after it's no longer useful. 
            return result;
        }

        void ExampleOperations(Stat.Modifier.Contexts contexts)
        {
            var stats = contexts.QueryArgs.Query.Stats;
            var statsRef = contexts.QueryArgs.Query.StatsRef;
            foreach (var type in contexts.QueryArgs.Query.Types)
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
            }
        }
    }

    private void Update()
    {
        _statCollection.Update();
    }
}