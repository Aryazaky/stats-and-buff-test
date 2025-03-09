namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        public interface IModifierMetadata : IAgeMetadata
        {
            int Priority { get; }
            bool IsExpired { get; }
        }
    }
}