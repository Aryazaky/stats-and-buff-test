using System;
using System.Collections.Generic;

public class InvokeLimitExpiryNotifier
{
    private readonly int invokeLimit;
    private readonly List<Stat.Modifier> trackedModifiers = new();

    public InvokeLimitExpiryNotifier(int invokeLimit)
    {
        if (invokeLimit <= 0)
            throw new ArgumentException("Invoke limit must be greater than zero.", nameof(invokeLimit));

        this.invokeLimit = invokeLimit;
    }

    public void TrackModifier(Stat.Modifier modifier)
    {
        if (trackedModifiers.Contains(modifier))
            throw new InvalidOperationException("This modifier is already being tracked.");

        trackedModifiers.Add(modifier);

        modifier.OnInvoke += CheckInvokeLimit;
        modifier.OnDispose += RemoveModifier;
    }

    private void CheckInvokeLimit(Stat.Modifier modifier)
    {
        if (modifier.InvokedCount >= invokeLimit)
        {
            modifier.Dispose();
        }
    }

    private void RemoveModifier(Stat.Modifier modifier)
    {
        trackedModifiers.Remove(modifier);
        modifier.OnInvoke -= CheckInvokeLimit;
        modifier.OnDispose -= RemoveModifier;
    }
}

