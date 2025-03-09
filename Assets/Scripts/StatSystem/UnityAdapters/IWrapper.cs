namespace StatSystem.UnityAdapters
{
    interface IWrapper<T> : IWriteOnlyWrapper<T>
    {
        T ToOriginal();
    }
}