using System;
using System.Collections.Generic;

namespace PollR.Model
{
    public class Snapshot
    {
        public DateTimeOffset Timestamp { get; private set; }
        public String Name { get; private set; }
        public Dictionary<string, int> Votes { get; private set; }

        public Snapshot(DateTimeOffset timestamp, string name)
        {
            Timestamp = timestamp;
            Name = name;
            Votes = new Dictionary<string, int>();
        }

    }
}