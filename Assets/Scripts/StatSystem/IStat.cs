namespace StatSystem
{
    public interface IStat
    {
        public Stat.StatType Type { get; }
        public float Value { get; }
        public float? Min { get; }
        public float? Max { get; }
        public int Precision { get; }
    }
}