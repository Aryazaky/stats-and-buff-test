namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier
        {
            public interface IExpiryNotifier
            {
                void TrackModifier(Modifier modifier);
                void CheckLimit();
            }
        }
    }
}