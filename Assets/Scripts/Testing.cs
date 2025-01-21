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
            var health = contexts.QueryArgs.Query.Stats[Stat.StatType.Health];
            float currentHealth = health.Value;
            float maxHealth = health.Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
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
                
                // Trigger at certain events (Not recommended)
                // Example: Ticking time bomb
                if (contexts.ModifierMetadata.IsExpired)
                {
                    
                }
            }
        }
    }
}