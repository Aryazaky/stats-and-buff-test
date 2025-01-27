public readonly partial struct Stat
{
    public interface IStat
    {
        public StatType Type { get; }
        public float Value { get; }
    }
}