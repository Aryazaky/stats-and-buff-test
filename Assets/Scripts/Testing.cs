using UnityEngine;

public class Testing : MonoBehaviour
{
    Stats stats;

    void Start()
    {
        Stat hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        Stat mana = new Stat(Stat.StatType.Mana, 5, 0, 10);
        stats = new Stats(hp, mana);

        Stat.Modifier.ActivePrerequisite isHealthBelowHalf = (contexts) =>
        {
            // Access the health stat using the stats indexer will result in stackoverflow error. Must use sender and query
            float currentHealth = contexts.query.Stat.Value;
            float maxHealth = contexts.query.Stat.Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        };

        Stat.Modifier.ActivePrerequisite isManaMoreThanZero = (contexts) =>
        {
            // If you have to use outside contexts, make sure that you don't access the same StatType as contexts.query.type
            if (contexts.query.Stat.Type == Stat.StatType.Mana) 
            {
                // contexts.query.value contains the current modified contexts.query.type's value. No way to access the base value for now. TODO: May need to capture the base stats as a separate property in Query. 
                return contexts.query.Stat.Value > 0;
            }
            return stats[Stat.StatType.Mana].Value > 0; // Just imagine this is other people's stats or something
        };

        Stat.Modifier.Operation healthPlusInvokedCount = (contexts) =>
        {
            // This will change the stat, only as long as the modifier is active. 
            contexts.query.Stat.Value += 1;
            // Two ways to change the stats permanently: ApplyChange or directly replacing the stat with the modified query. 
            if (contexts.sender is Stats s) // Watch out for when the sender is not a Stats meanwhile the Stats are stored elsewhere. TODO: May need a dedicated 'captured' stats property as struct passed in the Query constructors. 
            {
                //s.ApplyChange(contexts.query.type, 1);
                s[contexts.query.Stat.Type] = contexts.query.Stat;
                // But unable to retrieve the current stat of query.type because it will result in stackoverflow. 
                // var current = s[contexts.query.type]; // Error
                // Of course this will also not work
                s[contexts.query.Stat.Type] += 1; // Error
            }
        };

        var healthHealModifier1 = new StatModifier(
            Stat.StatType.Health, // contexts.query.type will equal this value
            operation: healthPlusInvokedCount,
            priority: 1, // TODO: should maybe make a standardized guide for priority values. Like, multiplication operations first and stuff like that. 
            activePrerequisite: isHealthBelowHalf
        );

        var healthHealModifier2 = new StatModifier(
            Stat.StatType.Health,
            operation: healthPlusInvokedCount,
            priority: 2,
            activePrerequisite: isHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(3);
        expiryNotifier.TrackModifier(healthHealModifier1);
        expiryNotifier.TrackModifier(healthHealModifier2);

        stats.Mediator.AddModifier(healthHealModifier1);
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 11
        //stats.Mediator.AddModifier(healthHealModifier2);
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 12
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 13, 3 times invoke, time to get booted
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 13
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 13
    }
}

        // TODO: Utilizing ActivePrerequisite, How do I make it so that it "heal by one" per second?