namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        private interface IModifier
        {
            public float LastInvokeTime { get; }
            public float CreatedTime { get; }
            public int InvokedCount { get; }
            public int Priority { get; }
        }
    }
}