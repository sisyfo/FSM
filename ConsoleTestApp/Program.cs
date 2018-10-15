using FSM;
using System;
using System.Collections.Generic;
using System.Threading;
using static FSM.StateStatic;
using System.Linq;

namespace ConsoleTestApp
{
    class Program
    {
        enum States { estado1, estado2 };
        enum Events { ev1, ev2 };

        static void Main(string[] args)
        {
            //State<States> p = States.estado1;

            //FSM.when(States.estado1).then(action).before(action).after(action).Transition(events[] ev, targetState).fallback(estado/evento, tiempo)
            //States.estado1
            //var yu = new [] { State.When(States.estado1), States.estado2 };

            var sts = State<States, Events>.
                When(States.estado1).
                    Then(() => Console.WriteLine("then estado 1")).
                    Before(() => Console.WriteLine("before estado1")).
                    After(() => Console.WriteLine("after estado 1")).
                    TransitionTo(States.estado2, Events.ev1).
                    //Fallback(States.estado1, 5000).
                When(States.estado2).
                    Then(() => Console.WriteLine("then estado 2")).
                    Before(() => Console.WriteLine("before estado 2")).
                    After(() => Console.WriteLine("after esatado 2")).
                    TransitionTo(States.estado1, Events.ev2).
                    Fallback(States.estado1, 5000);

            var fsm = new FiniteStateMachine<States, Events>(sts, States.estado1);

            fsm.TriggerEvent(Events.ev1);

            Thread.Sleep(20000);

            fsm.TriggerEvent(Events.ev1);











            Console.ReadLine();
        }
    }
}

