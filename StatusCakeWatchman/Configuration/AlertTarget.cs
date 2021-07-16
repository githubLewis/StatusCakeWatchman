namespace StatusCakeWatchman.Configuration
{
    public abstract class AlertTarget
    {
    }

    public class AlertGroup
    {
        private string _group;
        public AlertGroup(string group)
        {
            _group = group;
        }
        public override string ToString()
        {
            return _group;
        }
    }

    public class AlertTag
    {
        private string _tag;

        public AlertTag(string tag)
        {
            _tag = tag;
        }

        public override string ToString()
        {
            return _tag;
        }
    }

    

}
