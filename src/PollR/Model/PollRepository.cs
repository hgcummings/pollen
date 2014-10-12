using System;

namespace PollR.Model
{
    public class PollRepository
    {
        private Poll defaultPoll;

        public PollRepository()
        {
            defaultPoll = new Poll();
            defaultPoll.AddOption("Hit");
            defaultPoll.AddOption("Miss");
        }

        public Poll GetCurrentPoll()
        {
            return defaultPoll;
        }
    }
}