namespace StatSystemUnityAdapter
{
    internal interface IWriteOnlyWrapper<in T>
    {
        void Update(T obj);
    }
}