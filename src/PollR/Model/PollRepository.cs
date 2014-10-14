using System;

namespace PollR.Model
{
    public class PollRepository
    {
        private Poll defaultPoll;

        public PollRepository()
        {
            defaultPoll = new Poll();
            defaultPoll.AddOption("S.H.I.T.");
            defaultPoll.AddOption("MISS");
        }

        public Poll GetCurrentPoll()
        {
            return defaultPoll;
        }
    }
}