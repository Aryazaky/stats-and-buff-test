namespace StatSystem.UnityAdapters
{
    interface IWriteOnlyWrapper<in T>
    {
        void Update(T obj);
    }
}