namespace StatSystem.Modifiers
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