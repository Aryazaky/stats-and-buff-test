namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier
        {
            public interface IExpireTrigger
            {
                void Expire();
            }
        
            private class ExpireTrigger : IExpireTrigger
            {
                private readonly Modifier _modifier;

                public ExpireTrigger(Modifier modifier)
                {
                    _modifier = modifier;
                }

                public void Expire()
                {
                    _modifier.IsExpired = true;
                }
            }
        }
    }
}