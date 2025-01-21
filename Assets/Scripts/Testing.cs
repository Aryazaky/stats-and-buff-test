using UnityEngine;

public class Testing : MonoBehaviour
{
    private Stats _stats;

    void Start()
    {
        Stat hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        Stat mana = new Stat(Stat.StatType.Mana, 5, 0, 10);
        _stats = new Stats(hp, mana);

        var modifier = new StatModifier(
            Stat.StatType.Health, // contexts.Query.QueriedType will equal this value
            operation: HealthPlusInvokedCount,
            activePrerequisite: IsHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(2);
        expiryNotifier.TrackModifier(modifier);

        _stats.Mediator.AddModifier(modifier);
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 10
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 11
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 12
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 12, 2 times invoke, time to get booted
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 10 for temp, but stays 12 for the permanent
        return;

        bool IsHealthBelowHalf(Stat.Modifier.Contexts contexts)
        {
            // Access the health stat using the stats indexer will result in stackoverflow error. Must use sender and query
            float currentHealth = contexts.Query.Stats[Stat.StatType.Health].Value;
            float maxHealth = contexts.Query.Stats[Stat.StatType.Health].Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        }

        void HealthPlusInvokedCount(Stat.Modifier.Contexts contexts)
        {
            var stats = contexts.Query.Stats;
            var statsRef = contexts.Query.StatsRef;
            foreach (var type in contexts.Query.Types)
            {
                // // How to: Temporary stat change, offset by 1
                // stats[type] += 1;
                //
                // // How to: Temporary stat change, set to 1
                // stats[type] = stats[type].SetValue(1);
                //
                // // How to: Permanently change the stats by replacing the stats in the ref
                // statsRef[type] = stats[type];
                
                // How to: Permanently change the stats, by offset
                statsRef[type] += 1;
            }
        }
    }
}

        // TODO: Utilizing ActivePrerequisite, How do I make it so that it "heal by one" per second?