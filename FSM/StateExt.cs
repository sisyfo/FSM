using System;
using System.Collections.Generic;
using System.Linq;

namespace sisifo.FSM
{
    public static class StateExt
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
    }
}
