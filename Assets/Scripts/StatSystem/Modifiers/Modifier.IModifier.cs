namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        public interface IModifierMetadata : IAgeMetadata, ITickableMetadata
        {
            int Priority { get; }
            bool IsExpired { get; }
        }
    }
}