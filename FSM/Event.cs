namespace FSM
{
    public struct Event
    {
        public int id { get; }
        public State sourceState { get; }
        public State targetState { get; }

        private Event(int id, State sourceState, State targetState)
        {
            this.id = id;
            this.sourceState = sourceState;
            this.targetState = targetState;
        }

        public static Event CreateEvent(int id, State sourceState, State targetState) => new Event(id, sourceState, targetState);

        public override string ToString() => "Event " + id.ToString();
    }
}
