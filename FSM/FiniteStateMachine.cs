using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace sisifo.FSM
{
    public class FiniteStateMachine<St, Ev>
    {
        private const int InfiniteWaitTime = -1;
        private readonly BlockingCollection<Ev> EventsQueue;
        private St CurrentStateId;
        private readonly IEnumerable<State<St,Ev>> StateMachineBuild;
        private readonly CancellationTokenSource TokenSource;
        private readonly CancellationToken Token;

        private State<St, Ev> CurrentState => GetStateById(CurrentStateId);

        private State<St, Ev> GetStateById(St stateId) => 
            StateMachineBuild?.FirstOrDefault(p => p.StateId.Equals(stateId));

        private Event<St, Ev> GetTransitionById(St stateId, Ev @event) =>
            GetStateById(stateId)?.Transitions?.FirstOrDefault(p => p.EventId.Equals(@event));

        private void LoopTask()
        {
            (St state, int waitTime) fallbackData = (default(St), InfiniteWaitTime);
            bool IsFallback() => (fallbackData.waitTime != InfiniteWaitTime);

            while (true)
            {
                Ev currentEventId;
                try
                {
                    EventsQueue.TryTake(out currentEventId, fallbackData.waitTime, Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                var currentEvent = GetTransitionById(CurrentStateId, currentEventId);

                // Event not allowed for the currentState => ignore event
                if (!IsFallback() && currentEvent == null) continue;

                var targetState = IsFallback() ? GetStateById(fallbackData.state) : GetStateById(currentEvent.TargetState);

                // State not declared in the statemachine => ignore event
                if (targetState == null) continue;

                CurrentState.After?.Invoke();
                targetState.Before?.Invoke();
                targetState.Action?.Invoke();

                CurrentStateId = targetState.StateId;

                fallbackData = (targetState.Fallback != null) ?
                    (targetState.Fallback.State, targetState.Fallback.WaitTime) :
                    (default(St), InfiniteWaitTime);

                if (Token.IsCancellationRequested) break;
            }
            EventsQueue.Dispose();
        }

        public FiniteStateMachine(IEnumerable<State<St, Ev>> stateMachineBuild, St currentState)
        {
            this.StateMachineBuild = stateMachineBuild;
            this.CurrentStateId = currentState;
            this.EventsQueue = new BlockingCollection<Ev>(new ConcurrentQueue<Ev>());
            this.TokenSource = new CancellationTokenSource();
            this.Token = TokenSource.Token;
            new Task(() => LoopTask(), Token).Start();
        }

        public void TriggerEvent(Ev newevent)
        {
            try { EventsQueue.Add(newevent); }
            catch (ObjectDisposedException) { }
        }

        public St GetState() => CurrentStateId;

        public void Dispose()
        {
            TokenSource.Cancel();
        }
    }
}
