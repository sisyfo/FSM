using System;

namespace FSM
{
    public struct State : IEquatable<State>
    {
        public int id { get; }
        public Action action { get; }

        private State(int id, Action action)
        {
            this.id = id;
            this.action = action;
        }

        public static State CreateState(int id, Action action) => new State(id, action);

        public static bool operator ==(State state1, State state2) => state1.Equals(state2);
        public static bool operator !=(State state1, State state2) => !(state1 == state2);
        public bool Equals(State other) => id == other.id;

        public override string ToString() => "State " + id.ToString();
        public override int GetHashCode() => id;
    }
}
