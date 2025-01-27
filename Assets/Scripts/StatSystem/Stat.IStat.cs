namespace StatSystem
{
    public readonly partial struct Stat
    {
        public interface IStat
        {
            public StatType Type { get; }
            public float Value { get; }
            public float? Min { get; }
            public float? Max { get; }
            public int Precision { get; }
        }
    }
}