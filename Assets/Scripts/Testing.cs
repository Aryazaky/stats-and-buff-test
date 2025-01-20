using UnityEngine;

public class Testing : MonoBehaviour
{
    private Stats _stats;

    void Start()
    {
        Stat hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        Stat mana = new Stat(Stat.StatType.Mana, 5, 0, 10);
        _stats = new Stats(hp, mana);

        Stat.Modifier.ActivePrerequisite isManaMoreThanZero = (contexts) =>
        {
            // If you have to use outside contexts, make sure that you don't access the same StatType as contexts.query.type
            if (contexts.Query.Stat.Type == Stat.StatType.Mana) 
            {
                // contexts.query.value contains the current modified contexts.query.type's value. No way to access the base value for now. TODO: May need to capture the base stats as a separate property in Query. 
                return contexts.Query.Stat.Value > 0;
            }
            return _stats[Stat.StatType.Mana].Value > 0; // Just imagine this is other people's stats or something
        };

        var modifier = new StatModifier(
            Stat.StatType.Health, // contexts.query.type will equal this value
            operation: HealthPlusInvokedCount,
            activePrerequisite: IsHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(2);
        expiryNotifier.TrackModifier(modifier);

        _stats.Mediator.AddModifier(modifier);
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 11
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 12
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 13, 3 times invoke, time to get booted
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 13
        _stats.Update();
        Debug.Log($"Health: {_stats[Stat.StatType.Health]}"); // HP: 13
        return;

        bool IsHealthBelowHalf(Stat.Modifier.Contexts contexts)
        {
            // Access the health stat using the stats indexer will result in stackoverflow error. Must use sender and query
            float currentHealth = contexts.Query.Stat.Value;
            float maxHealth = contexts.Query.Stat.Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        }

        void HealthPlusInvokedCount(Stat.Modifier.Contexts contexts)
        {
            var stats = contexts.Query.Stats;
            var modifiable = contexts.Query.Modifiable;
            var type = contexts.Query.Stat.Type;
            
            // How to: Permanently change the stat
            stats[type] += 1; // No effect, why?
            modifiable[type] += 5;

            // How to: Temporary stat change


            // // This will change the stat, only as long as the modifier is active. 
            // contexts.Query.Stat.Value += 1;
            // // Two ways to change the stats permanently: ApplyChange or directly replacing the stat with the modified query. 
            // if (contexts.Sender is Stats s) // Watch out for when the sender is not a Stats meanwhile the Stats are stored elsewhere. TODO: May need a dedicated 'captured' stats property as struct passed in the Query constructors. 
            // {
            //     //s.ApplyChange(contexts.query.type, 1);
            //     s[contexts.Query.Stat.Type] = contexts.Query.Stat;
            //     // But unable to retrieve the current stat of query.type because it will result in stackoverflow. 
            //     // var current = s[contexts.query.type]; // Error
            //     // Of course this will also not work
            //     s[contexts.Query.Stat.Type] += 1; // Error
            // }
        }
    }
}

        // TODO: Utilizing ActivePrerequisite, How do I make it so that it "heal by one" per second?