using System;
using System.Collections.Generic;
using System.Linq;

namespace FSM
{
    public class Fallback<St>
    {
        public St State {get;}
        public int WaitTime { get; }

        private Fallback(St state, int waitTime)
        {
            this.State = state;
            this.WaitTime = waitTime;
        }

        public static Fallback<St> CreateFallback(St state, int waitTime) => new Fallback<St>(state, waitTime);
    }

    public class State<St, Ev> : IEquatable<State<St, Ev>>
    {
        public St id { get; }
        public Action action { get; }
        public Action before { get; }
        public Action after { get; }
        public IEnumerable<Event<St, Ev>> transitions { get; }
        public Fallback<St> fallback { get; }

        public State(St id) : this(id, null, null, null, Enumerable.Empty<Event<St, Ev>>()) { }

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


    public static class StateStatic
    {
        public static IEnumerable<State<St, Ev>> When<St, Ev>(this IEnumerable<State<St, Ev>> @this, State<St, Ev> state) =>
            (new[] { state }).Concat(@this);
        public static IEnumerable<State<St, Ev>> Then<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action action) => 
            @this.Select((p, i) => (i == 0 && p != null) ? p.SetAction(action) : p);
        public static IEnumerable<State<St, Ev>> Before<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action before) =>
            @this.Select((p, i) => (i == 0 && p != null) ? p.SetBefore(before) : p);
        public static IEnumerable<State<St, Ev>> After<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action after) =>
            @this.Select((p, i) => (i == 0 && p != null) ? p.SetAfter(after) : p);
        public static IEnumerable<State<St, Ev>> TransitionTo<St, Ev>(this IEnumerable<State<St, Ev>> @this, St t, Ev ev) =>
            @this.Select((p, i) => (i == 0 && p != null) ? p.AddTransition(t, ev) : p);
        public static IEnumerable<State<St, Ev>> Fallback<St, Ev>(this IEnumerable<State<St, Ev>> @this, St t, int time) =>
            @this.Select((p, i) => (i == 0 && p != null) ? p.SetFallback(t, time) : p);


        //public static State<St, Ev> GetState<St, Ev>(this IEnumerable<State<St, Ev>> @this, St t, Ev ev) =>
          //  @this.FirstOrDefault(p => (p.id.Equals(t)) && (p.transitions?.Any(tt => tt.Item2.Equals(ev)) ?? false));
            
            //@this.First(p => p.id.Equals(t)) : 
            //default(State<St, Ev>);

        //public static St GetTransition<St, Ev>(this State<St, Ev> @this, Ev stev) =>
          //  (stev != null && @this.transitions != null) ? @this.transitions.First(p => p.Item2.Equals(stev)).Item1 : default(St);
            
            






    }

}
