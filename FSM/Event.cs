namespace sisifo.FSM
{
    public class Event<St, Ev>
    {
        public Ev id { get; }
        public St sourceState { get; }
        public St targetState { get; }

        private Event(Ev id, St sourceState, St targetState)
        {
            this.id = id;
            this.sourceState = sourceState;
            this.targetState = targetState;
        }

        public static Event<St, Ev> CreateEvent(Ev id, St sourceState, St targetState) => 
            new Event<St, Ev>(id, sourceState, targetState);

        public override string ToString() => "Event " + id.ToString();
    }
}
