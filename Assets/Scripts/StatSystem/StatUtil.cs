using StatSystem.Collections;

namespace StatSystem
{
    public static class StatUtil
    {
        public static StatCollectionStruct ToStruct(this StatCollection statCollection)
        {
            return (StatCollectionStruct)statCollection;
        }
        public static StatCollection ToClass(this StatCollectionStruct statCollection)
        {
            return new StatCollection(statCollection);
        }
    }
}