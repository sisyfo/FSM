using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace FSM
{
    public class FiniteStateMachine<St, Ev> : IDisposable
    {
        private const int InfiniteWaitTime = -1;
        private readonly BlockingCollection<Ev> EventsQueue;
        private St CurrentState;
        private readonly IEnumerable<State<St,Ev>> StateMachineBuild;
        private readonly CancellationTokenSource tokenSource;
        private readonly CancellationToken token;

        private void loopTask()
        {
            try
            {
                (St state, int waitTime) fallbackData = (default(St), InfiniteWaitTime);
                while (true)
                {
                    Ev nextEvent;
                    EventsQueue.TryTake(out nextEvent, fallbackData.waitTime, token);

                    var currentStEv = StateMachineBuild?.FirstOrDefault(p => p.id.Equals(CurrentState));
                    var eventTransition = currentStEv?.transitions?.FirstOrDefault(p => p.id.Equals(nextEvent));
                    var targetStEv = (fallbackData.waitTime != InfiniteWaitTime) ? 
                        StateMachineBuild?.FirstOrDefault(p => p.id.Equals(fallbackData.state)) :
                        StateMachineBuild?.FirstOrDefault(p => eventTransition != null && p.id.Equals(eventTransition.targetState));
                    var fallback = targetStEv?.fallback;

                    if (targetStEv != null)
                    {
                        CurrentState = (fallbackData.waitTime != InfiniteWaitTime) ? 
                            fallbackData.state : 
                            eventTransition.targetState;
                        currentStEv.after?.Invoke();
                        targetStEv.before?.Invoke();
                        targetStEv.action?.Invoke();
                        fallbackData = (fallback != null) ? (fallback.State, fallback.WaitTime) : (default(St), InfiniteWaitTime);
                    }
                    if (token.IsCancellationRequested) break;
                }
            }
            catch (OperationCanceledException _) { }
            finally { EventsQueue.Dispose(); }
        }

        public FiniteStateMachine(IEnumerable<State<St, Ev>> stateMachineBuild, St currentState)
        {
            this.StateMachineBuild = stateMachineBuild;
            this.CurrentState = currentState;
            this.EventsQueue = new BlockingCollection<Ev>(new ConcurrentQueue<Ev>());
            this.tokenSource = new CancellationTokenSource();
            this.token = tokenSource.Token;
            new Task(() => loopTask(), token).Start();
        }

        public void TriggerEvent(Ev newevent)
        {
            try { EventsQueue.Add(newevent); }
            catch (ObjectDisposedException _) { }
        }

        public State<St, Ev> getState() => CurrentState;

        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
