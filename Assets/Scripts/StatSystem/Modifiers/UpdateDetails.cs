using System;
using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    /// <summary>
    /// Used to capture variables for updating stats
    /// </summary>
    public class UpdateDetails
    {
        /// <summary>
        /// This action will be applied to both baseStats and queriedStats.
        /// </summary>
        public Action<IMutableStatCollection> SyncedUpdate { get; }
    
        /// <summary>
        /// This action will be applied only to queriedStats.
        /// </summary>
        public Action<IMutableStatCollection> NonSyncedUpdate { get; }

        public UpdateDetails(
            Action<IMutableStatCollection> syncedUpdate = null,
            Action<IMutableStatCollection> nonSyncedUpdate = null)
        {
            SyncedUpdate = syncedUpdate ?? (_ => {});
            NonSyncedUpdate = nonSyncedUpdate ?? (_ => {});
        }
        
        public UpdateDetails(bool sync, Action<IMutableStatCollection> update = null) : this(
            sync ? update : null, !sync ? update : null)
        {
        }
    }

    /// <summary>
    /// Empty class just for easier code reading. The same as doing <c>new OnTickUpdateDetails()</c> with no arguments. 
    /// </summary>
    public class NoUpdateDetails : UpdateDetails
    {
    }
}