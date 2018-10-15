# FSM
C# Implementation of a Thread-Safe Finite State Machine (FSM) with a fluent API.

The fundamental pieces of the FSM are the States and the Events, both are defined with an enum.

enum States { state1, state2 };
enum Events { ev1, ev2 };

The fluent API allows you to define the FSM states and events relationship in a very readable way.

var sts = State<States, Events>.
			When(States.state1).
				Then(() => Console.WriteLine("then state 1")).
                Before(() => Console.WriteLine("before state 1")).
                After(() => Console.WriteLine("after state 1")).
                TransitionTo(States.state2, Events.ev1).
			When(States.state2).
				Then(() => Console.WriteLine("then state 2")).
				Before(() => Console.WriteLine("before state 2")).
				After(() => Console.WriteLine("after state 2")).
				TransitionTo(States.state1, Events.ev2).
				Fallback(States.state1, 5000);

* When
	
	Use it to add a new state to the FSM

* Then

	Used to define an Action to trigger when the FSM enter into a State.

* Before

	Used to define an Action to trigger before the FSM enter into a State.

* After

 	Used to define an Action to trigger after the FSM leaves a State.

* TransitionTo

	Used to define the relationship between the States and the Events, building the transitions map.

* Fallback

	You can define a timeout and a fallback State for the current State, allowing the FSM to move to another State based on the amount of time spend on the State.