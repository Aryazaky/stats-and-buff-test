namespace StatSystem
{
    public abstract class QueryArgs
    {
        public object Sender { get; }
        public Stat.Query Query { get; }

        protected QueryArgs(object sender, Stat.Query query)
        {
            Sender = sender;
            Query = query;
        }
    }

    public class NoQueryArgs : QueryArgs
    {
        public NoQueryArgs(object sender, Stat.Query query) : base(sender, query)
        {
        }
    }
}