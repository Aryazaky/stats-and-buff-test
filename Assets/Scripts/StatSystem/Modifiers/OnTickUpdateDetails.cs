using System;
using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    /// <summary>
    /// Used to capture variables for updating stats
    /// </summary>
    public class OnTickUpdateDetails
    {
        /// <summary>
        /// These actions will be applied to both baseStats and queriedStats.
        /// </summary>
        public Action<IMutableStatCollection> SynchronizedUpdate { get; }
    
        /// <summary>
        /// These actions will be applied only to queriedStats.
        /// </summary>
        public Action<IMutableStatCollection> NonSynchronizedUpdate { get; }

        public OnTickUpdateDetails(
            Action<IMutableStatCollection> synchronizedUpdates,
            Action<IMutableStatCollection> nonSynchronizedUpdates)
        {
            SynchronizedUpdate = synchronizedUpdates ?? (_ => {});
            NonSynchronizedUpdate = nonSynchronizedUpdates ?? (_ => {});
        }
    }
}