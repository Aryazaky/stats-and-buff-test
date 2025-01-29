namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public static class PriorityType
        {
            public const int Default = 0;
            public const int Boost = 1; // Equipment stat boosts and such. They're additive BUT done before multiplication to simulate as if the base stats were always like that. 
            public const int Multiplicative = 2; // Buffs or debuffs
            public const int Offset = 3; // This is also additive, but done normally. For flat stat increase/decrease unaffected by buffs
            public const int Override = 4; // Evil, the ultimate No U. Could set the hard work we all did to calculate all that to 0 for all I know. 
            public const int Clamp = 5;
        }
    }
}