using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FSM
{
    public class FiniteStateMachine : IDisposable
    {
        private readonly BlockingCollection<Event> EventsQueue;
        private State CurrentState;
        private readonly IEnumerable<State> States;
        private readonly IEnumerable<Event> Events;
        private readonly CancellationTokenSource tokenSource;
        private readonly CancellationToken token;

        private void loopTask()
        {
            try
            {
                while (true)
                {
                    Event nextEvent = EventsQueue.Take(token);
                    if (nextEvent.sourceState == CurrentState)
                    {
                        CurrentState = nextEvent.targetState;
                        CurrentState.action();
                    }
                    if (token.IsCancellationRequested) break;
                }
            }
            catch (OperationCanceledException _) { }
            finally { EventsQueue.Dispose(); }
        }

        public FiniteStateMachine(IEnumerable<State> states, IEnumerable<Event> events, State currentState)
        {
            States = states;
            Events = events;
            CurrentState = currentState;
            EventsQueue = new BlockingCollection<Event>(new ConcurrentQueue<Event>());
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            new Task(() => loopTask(), token).Start();
        }

        public void TriggerEvent(Event newevent)
        {
            try { EventsQueue.Add(newevent); }
            catch (ObjectDisposedException _) { }
        }

        public State getState() => CurrentState;

        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
