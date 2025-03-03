namespace StatSystem.UnityAdapters
{
    interface IWrapper<T>
    {
        void Update(T obj);
        T ToOriginal();
    }
}