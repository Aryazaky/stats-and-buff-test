using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Collections.Generic
{
    // public class Stats<T> : IStatCollection<T>, IStatCollection, IStats where T : IStat
    // {
    //     private IStatCollection<T> _base;
    //     private IStatCollection<T> _modified;
    //
    //     public Stats(params T[] stats)
    //     {
    //         _base = new StatCollectionStruct<T>(stats);
    //         _modified = new StatCollectionStruct<T>(stats);
    //         Mediator = new Mediator(_ => IsDirty = true, _ => IsDirty = true);
    //     }
    //
    //     public Stats(IEnumerable<T> stats)
    //     {
    //         _base = new StatCollectionStruct<T>(stats);
    //         _modified = new StatCollectionStruct<T>(stats);
    //         Mediator = new Mediator(_ => IsDirty = true, _ => IsDirty = true);
    //     }
    //
    //     public Mediator Mediator { get; }
    //     public bool IsDirty { get; private set; }
    //
    //     public IEnumerable<StatType> Types => _base.Types;
    //
    //     public IStatCollection<T> Base
    //     {
    //         get
    //         {
    //             IsDirty = true;
    //             return _base;
    //         }
    //     }
    //
    //     public T this[StatType type]
    //     {
    //         get => _modified[type];
    //         set => _modified[type] = value;
    //     }
    //     
    //     IStat IIndexer.this[StatType type]
    //     {
    //         get => this[type];
    //         set => this[type] = value.ConvertTo<T>();
    //     }
    //
    //     public void Bake()
    //     {
    //         foreach (var type in _base.Types.ToArray())
    //         {
    //             _base[type] = _modified[type];
    //         }
    //     }
    //     
    //     public void Update(IReadOnlyWorldContexts worldContexts, params StatType[] types)
    //     {
    //         if (!types.Any()) types = Types.ToArray();
    //         foreach (var type in types)
    //         {
    //             _modified[type] = PerformQuery(worldContexts, type); // This will make it so that the value of buffs be temporary. 
    //         }
    //
    //         IsDirty = false;
    //     }
    //
    //     private T PerformQuery(IReadOnlyWorldContexts worldContexts, StatType type)
    //     {
    //         var query = new Query(this, worldContexts, type);
    //         Mediator.PerformQuery(query);
    //         var queryStat = query.TemporaryStats[type];
    //         if (queryStat is T stat)
    //         {
    //             return stat;
    //         }
    //         else
    //         {
    //             return (T)Activator.CreateInstance(typeof(T), queryStat.Type, queryStat.Value, queryStat.Min, queryStat.Max, queryStat.Precision);
    //         }
    //     }
    //
    //     private IStatCollection<T> PerformQuery(IReadOnlyWorldContexts worldContexts)
    //     {
    //         var query = new Query(this, worldContexts);
    //         Mediator.PerformQuery(query);
    //         return new StatCollectionStruct<T>(query.TemporaryStats.Cast<T>());
    //     }
    //     
    //     public bool Contains(params StatType[] type) => _base.Contains(type);
    //     
    //     public bool TryGetStat(StatType type, out IStat stat)
    //     {
    //         if (_modified.TryGetStat(type, out var value))
    //         {
    //             stat = value;
    //             return true;
    //         }
    //
    //         stat = null;
    //         return false;
    //     }
    //
    //     public bool TryGetStat(StatType type, out T stat) => _modified.TryGetStat(type, out stat);
    //
    //     public IEnumerator<T> GetEnumerator() => _modified.GetEnumerator();
    //
    //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //
    //     public override string ToString()
    //     {
    //         return _modified.ToString();
    //     }
    // }
}