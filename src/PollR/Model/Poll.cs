using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PollR.Model
{
    public class Poll
    {
        private readonly ConcurrentDictionary<string, int> votes;

        public Poll()
        {
            votes = new ConcurrentDictionary<string, int>();
        }

        public void AddOption(string name)
        {
            votes.GetOrAdd(name, 0);
        }

        public IEnumerable<string> Options
        {
            get { return votes.Keys; }
        }
        
    }
}