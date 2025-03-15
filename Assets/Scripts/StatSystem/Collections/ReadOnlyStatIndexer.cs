namespace StatSystem.Collections
{
    public readonly struct ReadOnlyStatIndexer : IReadOnlyStatIndexer<Stat>
    {
        private readonly StatCollectionStruct _statCollection;

        public ReadOnlyStatIndexer(StatCollectionStruct statCollection)
        {
            _statCollection = statCollection;
        }

        public Stat this[StatType type] => _statCollection[type];

        public bool TryGetStat(StatType type, out Stat stat)
        {
            return _statCollection.TryGetStat(type, out stat);
        }
    }
}