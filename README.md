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
	
License
-------

The MIT License

Copyright (c) 2018 Jose Chocron

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
