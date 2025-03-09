namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        public interface IExpiryNotifier
        {
            void TrackModifier(Modifier modifier);
            void CheckLimit();
        }
    }
}