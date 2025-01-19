using System;

public class TestExpiryNotifier : Stat.Modifier.IExpiryNotifier
{
    public event Action OnExpired;

    // "While HP is above 10"
    public TestExpiryNotifier() { }
}
