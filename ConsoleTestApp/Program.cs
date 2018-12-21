using sisifo.FSM;
using System;
using System.Threading;
using static sisifo.FSM.StateExt;

namespace ConsoleTestApp
{
    class Program
    {
        enum States { state0, state1, state2 };
        enum Events { ev0, ev1, ev2 };

        static void Main(string[] args)
        {
            var sts = State<States, Events>.
                When(States.state0).
                    TransitionTo(States.state1, Events.ev0).
                When(States.state1).
                    Then(() => Console.WriteLine("then state 1")).
                    //Before(() => Console.WriteLine("before state 1")).
                    //After(() => Console.WriteLine("after state 1")).
                    TransitionTo(States.state0, Events.ev0).
                    TransitionTo(States.state2, Events.ev1).
                    Fallback(States.state1, 5000).
                When(States.state2).
                    Then(() => Console.WriteLine("then state 2")).
                    //Before(() => Console.WriteLine("before state 2")).
                    //After(() => Console.WriteLine("after state 2")).
                    TransitionTo(States.state1, Events.ev2).
                    Fallback(States.state1, 5000);

            var fsm = new FiniteStateMachine<States, Events>(sts, States.state0);

            fsm.TriggerEvent(Events.ev0);

            // TriggerEvent is Thread-Safe
            //new Thread(() => fsm.TriggerEvent(Events.ev1)).Start();
            //new Thread(() => fsm.TriggerEvent(Events.ev2)).Start();

            //Thread.Sleep(3000);

            //new Thread(() => fsm.TriggerEvent(Events.ev1)).Start();

            Console.ReadLine();
        }
    }
}