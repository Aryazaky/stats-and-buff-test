namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public interface IModifier
        {
            public int Priority { get; }
        }
    }
}