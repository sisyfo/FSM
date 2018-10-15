using sisifo.FSM;
using System;
using System.Threading;
using static sisifo.FSM.StateExt;

namespace ConsoleTestApp
{
    class Program
    {
        enum States { estado1, estado2 };
        enum Events { ev1, ev2 };

        static void Main(string[] args)
        {
            var sts = State<States, Events>.
                When(States.estado1).
                    Then(() => Console.WriteLine("then state 1")).
                    Before(() => Console.WriteLine("before state 1")).
                    After(() => Console.WriteLine("after state 1")).
                    TransitionTo(States.estado2, Events.ev1).
                When(States.estado2).
                    Then(() => Console.WriteLine("then state 2")).
                    Before(() => Console.WriteLine("before state 2")).
                    After(() => Console.WriteLine("after state 2")).
                    TransitionTo(States.estado1, Events.ev2).
                    Fallback(States.estado1, 5000);

            var fsm = new FiniteStateMachine<States, Events>(sts, States.estado1);

            // TriggerEvent is Thread-Safe
            new Thread(() => fsm.TriggerEvent(Events.ev1)).Start();
            new Thread(() => fsm.TriggerEvent(Events.ev2)).Start();

            Thread.Sleep(3000);

            new Thread(() => fsm.TriggerEvent(Events.ev1)).Start();

            Console.ReadLine();
        }
    }
}