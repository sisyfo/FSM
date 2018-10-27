using System;
using System.Collections.Generic;
using System.Linq;

namespace sisifo.FSM
{
    public class State<St, Ev> : IEquatable<State<St, Ev>>
    {
        public St StateId { get; }
        public Action Action { get; }
        public Action Before { get; }
        public Action After { get; }
        public IEnumerable<Event<St, Ev>> Transitions { get; }
        public Fallback<St> Fallback { get; }

        private State(St stateId) : this(stateId, null, null, null, Enumerable.Empty<Event<St, Ev>>()) { }

        private State(St stateId, Action action = null, Action before = null, Action after = null, 
            IEnumerable<Event<St, Ev>> transitions = null, Fallback<St> fallback = null)
        {
            this.StateId = stateId;
            this.Action = action;
            this.Before = before;
            this.After = after;
            this.Transitions = transitions;
            this.Fallback = fallback;
        }

        internal State<St, Ev> ApplyAction(Action action) => 
            new State<St, Ev>(this.StateId, action, this.Before, this.After, this.Transitions, this.Fallback);

        internal State<St, Ev> ApplyBefore(Action before) => 
            new State<St, Ev>(this.StateId, this.Action, before, this.After, this.Transitions, this.Fallback);

        internal State<St, Ev> ApplyAfter(Action after) => 
            new State<St, Ev>(this.StateId, this.Action, this.Before, after, this.Transitions, this.Fallback);

        internal State<St, Ev> ApplyTransition(St state, Ev ev) =>
            new State<St, Ev>(this.StateId, this.Action, this.Before, this.After, 
                Transitions.Append(Event<St, Ev>.CreateEvent(ev, this.StateId, state)), this.Fallback);

        internal State<St, Ev> ApplyFallback(St state, int time) =>
            new State<St, Ev>(this.StateId, this.Action, this.Before, this.After, this.Transitions, 
                Fallback<St>.CreateFallback(state, time));

        public static implicit operator State<St, Ev>(St t) => (new State<St, Ev>(t));

        public IEnumerable<State<St, Ev>> ToEnumerable()
        {
            yield return this;
        }

        public bool Equals(State<St, Ev> other) => other != null && StateId.Equals(other.StateId);

        public override string ToString() => "State " + StateId.ToString();

        public static IEnumerable<State<St, Ev>> When(St t)
        {
            yield return t;
        }
    }
}
