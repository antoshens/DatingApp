namespace DatingApp.Core.Events
{
    public abstract class BaseEvent
    {
        public DateTime Timestamp { get; protected set; }

        protected BaseEvent()
        {
            Timestamp = DateTime.Now;
        }
    }
}
