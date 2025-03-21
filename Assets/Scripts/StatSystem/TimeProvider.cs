using System;

namespace StatSystem
{
    internal static class TimeProvider
    {
        private static ITimeProvider _instance;

        public static ITimeProvider Instance => _instance ??= CreateDefaultProvider();

        private static ITimeProvider CreateDefaultProvider()
        {
            return new DefaultTimeProvider();
        }
    }

    internal class DefaultTimeProvider : ITimeProvider
    {
        public float GetTime() => (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}