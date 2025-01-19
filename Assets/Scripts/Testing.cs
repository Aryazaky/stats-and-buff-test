using UnityEngine;

public class Testing : MonoBehaviour
{
    Stats stats;

    void Start()
    {
        Stat hp = new Stat(Stat.StatType.Health, value: 10, min: 0, max: 50);
        stats = new Stats(hp);

        Stat.Modifier.ActivePrerequisite isHealthBelowHalf = (sender, query) =>
        {
            // Access the health stat using the stats indexer will result in stackoverflow error. Must use sender and query
            float currentHealth = query.value;
            float maxHealth = query.max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        };

        Stat.Modifier.Operation healByOne = (sender, query) =>
        {
            query.value += 1;
        };

        var healthHealModifier = new StatModifier(
            Stat.StatType.Health,
            priority: 1,
            operation: healByOne,
            activePrerequisite: isHealthBelowHalf
        );

        // Add the modifier to the modifiers list
        Debug.Log($"Health: {stats[Stat.StatType.Health]}");
        stats.Modifiers.AddModifier(healthHealModifier);
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 11
        Debug.Log($"Health: {stats[Stat.StatType.Health]}"); // HP: 12 in the same frame

        // How do I make it so that it "heal by one" per second?
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
