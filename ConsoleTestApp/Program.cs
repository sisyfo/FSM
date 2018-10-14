using FSM;
using System;
using System.Threading;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            State s0 = State.CreateState(0, () => { Console.WriteLine("hola desde s0"); });
            State s1 = State.CreateState(1, () => { Console.WriteLine("hola desde s1"); });

            Event e0 = Event.CreateEvent(0, s0, s1 );
            Event e1 = Event.CreateEvent(1, s1, s0 );

            FiniteStateMachine fsm = new FiniteStateMachine(new[] { s0, s1 }, new[] { e0, e1 }, s0);

                for (int i = 0; i < 100; i++)
                {
                    new Thread(() => fsm.TriggerEvent(e0)).Start();
                    new Thread(() => fsm.TriggerEvent(e1)).Start();

                    if (i == 80) fsm.Dispose();

                Console.WriteLine("Stado:" + fsm.getState().id);
                }




                



            
            
            //new Thread(() => fsm.TriggerEvent(e1)).Start();
            //new Thread(() => throw new Exception("sdfsdf")).Start();

            






            Console.WriteLine("terminado el hilo principal---------------------------------");





            Console.ReadLine();
        }
    }
}
