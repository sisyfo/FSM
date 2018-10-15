using System;
using System.Collections.Generic;
using System.Linq;

namespace sisifo.FSM
{
    public class State<St, Ev> : IEquatable<State<St, Ev>>
    {
        public St id { get; }
        public Action action { get; }
        public Action before { get; }
        public Action after { get; }
        public IEnumerable<Event<St, Ev>> transitions { get; }
        public Fallback<St> fallback { get; }

        private State(St id) : this(id, null, null, null, Enumerable.Empty<Event<St, Ev>>()) { }

        private State(St id, Action action = null, Action before = null, Action after = null, 
            IEnumerable<Event<St, Ev>> transitions = null, Fallback<St> fallback = null)
        {
            this.id = id;
            this.action = action;
            this.before = before;
            this.after = after;
            this.transitions = transitions;
            this.fallback = fallback;
        }

        public State<St, Ev> SetAction(Action action) => 
            new State<St, Ev>(this.id, action, this.before, this.after, this.transitions);
        public State<St, Ev> SetBefore(Action before) => 
            new State<St, Ev>(this.id, this.action, before, this.after, this.transitions);
        public State<St, Ev> SetAfter(Action after) => 
            new State<St, Ev>(this.id, this.action, this.before, after, this.transitions);
        public State<St, Ev> AddTransition(St state, Ev ev) =>
            new State<St, Ev>(this.id, this.action, this.before, this.after, transitions.Append(Event<St, Ev>.CreateEvent(ev, this.id, state)));
        public State<St, Ev> SetFallback(St state, int time) =>
            new State<St, Ev>(this.id, this.action, this.before, this.after, this.transitions, Fallback<St>.CreateFallback(state, time));

        public static implicit operator State<St, Ev>(St t) => (new State<St, Ev>(t));

        public IEnumerable<State<St, Ev>> ToEnumerable()
        {
            yield return this;
        }

        public bool Equals(State<St, Ev> other) => other != null && id.Equals(other.id);

        public override string ToString() => "State " + id.ToString();

        public static IEnumerable<State<St, Ev>> When(St t)
        {
            yield return t;
        }
    }
}
