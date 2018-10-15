namespace sisifo.FSM
{
    public class Fallback<St>
    {
        public St State { get; }
        public int WaitTime { get; }

        private Fallback(St state, int waitTime)
        {
            this.State = state;
            this.WaitTime = waitTime;
        }

        public static Fallback<St> CreateFallback(St state, int waitTime) => new Fallback<St>(state, waitTime);
    }
}
