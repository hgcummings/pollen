using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PollR.Model
{
    public class Poll
    {
        private readonly ConcurrentDictionary<string, int> options;
        private readonly ConcurrentDictionary<string, string> votes;

        public Poll()
        {
            options = new ConcurrentDictionary<string, int>();
            votes = new ConcurrentDictionary<string, string>();
        }

        public void AddOption(string name)
        {
            options.GetOrAdd(name, 0);
        }

        public IEnumerable<KeyValuePair<string, int>> Options
        {
            get { return options; }
        }

        public IEnumerable<KeyValuePair<string, int>> Vote(string option, string connectionId)
        {
            return CancelPreviousVote(connectionId).Concat(RecordNewVote(option, connectionId));
        }

        private IEnumerable<KeyValuePair<string, int>> CancelPreviousVote(string connectionId)
        {
            string prevVote;

            if (votes.TryRemove(connectionId, out prevVote))
            {
                var newValue = options.AddOrUpdate(prevVote, x => 0, (x, prevValue) => prevValue - 1);
                return new[] { new KeyValuePair<string, int>(prevVote, newValue) };
            }

            return Enumerable.Empty<KeyValuePair<string, int>>();
        }

        private IEnumerable<KeyValuePair<string, int>> RecordNewVote(string option, string connectionId)
        {
            var newValue = options.AddOrUpdate(option, x => 1, (x, prevValue) => prevValue + 1);
            votes.AddOrUpdate(connectionId, x => option, (x, y) => option);
            return new[] { new KeyValuePair<string, int>(option, newValue) };
        }
    }
}