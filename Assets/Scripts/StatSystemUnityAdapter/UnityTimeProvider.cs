using StatSystem;
using UnityEngine;

namespace StatSystemUnityAdapter
{
#if UNITY_STANDALONE
    public class UnityTimeProvider : ITimeProvider
    {
        public float GetTime() => Time.time;
    }
#endif
}