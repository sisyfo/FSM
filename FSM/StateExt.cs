using System;
using System.Collections.Generic;
using System.Linq;

namespace sisifo.FSM
{
    public static class StateExt
    {
        public static IEnumerable<State<St, Ev>> When<St, Ev>(this IEnumerable<State<St, Ev>> @this, State<St, Ev> state) =>
            state.ToEnumerable().Concat(@this);

        public static IEnumerable<State<St, Ev>> Then<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action action) =>
            @this.Any() ? @this.First().ApplyAction(action).ToEnumerable().Concat(@this.Skip(1)) : @this;

        public static IEnumerable<State<St, Ev>> Before<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action before) =>
            @this.Any() ? @this.First().ApplyBefore(before).ToEnumerable().Concat(@this.Skip(1)) : @this;

        public static IEnumerable<State<St, Ev>> After<St, Ev>(this IEnumerable<State<St, Ev>> @this, Action after) =>
            @this.Any() ? @this.First().ApplyAfter(after).ToEnumerable().Concat(@this.Skip(1)) : @this;

        public static IEnumerable<State<St, Ev>> TransitionTo<St, Ev>(this IEnumerable<State<St, Ev>> @this, St t, Ev ev) =>
            @this.Any() ? @this.First().ApplyTransition(t, ev).ToEnumerable().Concat(@this.Skip(1)) : @this;

        public static IEnumerable<State<St, Ev>> Fallback<St, Ev>(this IEnumerable<State<St, Ev>> @this, St t, int time) =>
            @this.Any() ? @this.First().ApplyFallback(t, time).ToEnumerable().Concat(@this.Skip(1)) : @this;
    }
}
