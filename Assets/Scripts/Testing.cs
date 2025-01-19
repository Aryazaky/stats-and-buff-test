using UnityEngine;

public class Testing : MonoBehaviour
{
    Stats stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Stat hp = new Stat(Stat.StatType.Health, 50, 50);
        var baseStats = new System.Collections.Generic.Dictionary<Stat.StatType, Stat> {
            { Stat.StatType.Health, hp }
        };

        // Assuming `Stat.Type.Health` is the type representing health
        stats = new Stats(hp);

        // Define the ActivePrerequisite
        Stat.Modifier.ActivePrerequisite isHealthBelowHalf = () =>
        {
            // Access the health stat using the stats indexer
            float currentHealth = stats[Stat.StatType.Health].Value;
            float maxHealth = baseStats[Stat.StatType.Health].Max ?? float.MaxValue;
            return currentHealth < (maxHealth / 2);
        };

        // Define the Operation
        Stat.Modifier.Operation healByOne = (sender, query) =>
        {
            // Simply add 1 to the current value
            query.value += 1;
        };

        // Create and Add the Modifier
        var healthHealModifier = new StatModifier(
            Stat.StatType.Health,   // Stat type to modify
            priority: 1,        // Modifier priority
            operation: healByOne,
            activePrerequisite: isHealthBelowHalf
        );

        // Add the modifier to the modifiers list
        stats.Modifiers.AddModifier(healthHealModifier);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Health: {stats[Stat.StatType.Health].Value}");
    }
}
