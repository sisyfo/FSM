namespace sisifo.FSM
{
    public class Event<St, Ev>
    {
        public Ev EventId { get; }
        public St SourceState { get; }
        public St TargetState { get; }

        private Event(Ev eventId, St sourceState, St targetState)
        {
            this.EventId = eventId;
            this.SourceState = sourceState;
            this.TargetState = targetState;
        }

        public static Event<St, Ev> CreateEvent(Ev id, St sourceState, St targetState) => 
            new Event<St, Ev>(id, sourceState, targetState);

        public override string ToString() => "Event " + EventId.ToString();
    }
}
