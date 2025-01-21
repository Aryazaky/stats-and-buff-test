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
            Stat.StatType.Health,
            operation: HealthPlusInvokedCount,
            activePrerequisite: IsHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(2);
        expiryNotifier.TrackModifier(modifier);

        _stats.Mediator.AddModifier(modifier);
        Debug.Log($"0:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update();
        Debug.Log($"1:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update();
        Debug.Log($"2:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update();
        Debug.Log($"3:Health: {_stats[Stat.StatType.Health]}");
        _stats.Update();
        Debug.Log($"4:Health: {_stats[Stat.StatType.Health]}");
        return;

        bool IsHealthBelowHalf(Stat.Modifier.Contexts contexts)
        {
            var health = contexts.Query.Stats[Stat.StatType.Health];
            float currentHealth = health.Value;
            float maxHealth = health.Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        }

        void HealthPlusInvokedCount(Stat.Modifier.Contexts contexts)
        {
            var stats = contexts.Query.Stats;
            var statsRef = contexts.Query.StatsRef;
            foreach (var type in contexts.Query.Types)
            {
                // How to: Temporary stat change, offset by 1
                // stats[type].Value += 1;
                
                // How to: Temporary stat change, offset by how many times this effect has been invoked
                stats[type].Value += 1 + contexts.Modifier.InvokedCount;
                
                // How to: Temporary stat change, set to 1
                // stats[type].Value = 1;
                
                // How to: Permanently change the stats by replacing the stats in the ref
                // statsRef[type] = statsRef[type].SetValue(1);
                
                // How to: Permanently change the stats, by offset
                // statsRef[type] += 1;
            }
        }
    }
}

        // TODO: Utilizing ActivePrerequisite, How do I make it so that it "heal by one" per second?