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
            float currentHealth = contexts.query.value;
            float maxHealth = contexts.query.max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        };

        Stat.Modifier.ActivePrerequisite isManaMoreThanZero = (contexts) =>
        {
            // If you have to use outside contexts, make sure that you don't access the same StatType
            return stats[Stat.StatType.Mana].Value > 0;
        };

        Stat.Modifier.Operation healthPlusInvokedCount = (contexts) =>
        {
            contexts.query.value += contexts.modifier.InvokedCount;
        };

        var healthHealModifier1 = new StatModifier(
            Stat.StatType.Health,
            priority: 1,
            operation: healthPlusInvokedCount,
            activePrerequisite: isHealthBelowHalf
        );

        var healthHealModifier2 = new StatModifier(
            Stat.StatType.Health,
            priority: 2,
            operation: healthPlusInvokedCount,
            activePrerequisite: isHealthBelowHalf
        );

        var expiryNotifier = new InvokeLimitExpiryNotifier(3);
        expiryNotifier.TrackModifier(healthHealModifier1);
        expiryNotifier.TrackModifier(healthHealModifier2);

        stats.Modifiers.AddModifier(healthHealModifier1);
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 10
        stats.Modifiers.AddModifier(healthHealModifier2);
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 11
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 13
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 12
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 10
    }
}

        // TODO: Utilizing ActivePrerequisite, How do I make it so that it "heal by one" per second?