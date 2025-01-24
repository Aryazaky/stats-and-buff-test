public readonly partial struct Stat
{
    public interface IStat
    {
        public Stat.StatType Type { get; }
        public float Value { get; }
    }
}