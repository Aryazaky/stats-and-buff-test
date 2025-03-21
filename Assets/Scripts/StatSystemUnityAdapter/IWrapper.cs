namespace StatSystemUnityAdapter
{
    internal interface IWrapper<T> : IWriteOnlyWrapper<T>
    {
        T ToOriginal();
    }
}