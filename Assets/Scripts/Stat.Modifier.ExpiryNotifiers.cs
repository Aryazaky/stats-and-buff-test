using System;
using System.Collections.Generic;

public class InvokeLimitExpiryNotifier
{
    private readonly int _invokeLimit;
    private readonly List<Stat.Modifier> _trackedModifiers = new();

    public InvokeLimitExpiryNotifier(int invokeLimit)
    {
        if (invokeLimit <= 0)
            throw new ArgumentException("Invoke limit must be greater than zero.", nameof(invokeLimit));

        _invokeLimit = invokeLimit;
    }

    public void TrackModifier(Stat.Modifier modifier)
    {
        if (_trackedModifiers.Contains(modifier))
            throw new InvalidOperationException("This modifier is already being tracked.");

        _trackedModifiers.Add(modifier);

        modifier.OnInvoke += CheckInvokeLimit;
        modifier.OnDispose += RemoveModifier;
    }

    private void CheckInvokeLimit(Stat.Modifier modifier)
    {
        if (modifier.InvokedCount >= _invokeLimit)
        {
            modifier.Dispose();
        }
    }

    private void RemoveModifier(Stat.Modifier modifier)
    {
        _trackedModifiers.Remove(modifier);
        modifier.OnInvoke -= CheckInvokeLimit;
        modifier.OnDispose -= RemoveModifier;
    }
}

