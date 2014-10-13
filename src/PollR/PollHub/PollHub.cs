using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PollR.Model;

namespace PollR.PollHub
{
    [HubName("poll")]
    public class PollHub : Hub
    {
        private readonly PollRepository pollRepository;

        public PollHub(PollRepository pollRepository)
        {
            this.pollRepository = pollRepository;
        }

        public void Register()
        {
            foreach (var option in this.pollRepository.GetCurrentPoll().Options)
            {
                Clients.Caller.UpdateOption(option.Key, option.Value);
            }
        }

        public void Vote(string option)
        {
            var updated = this.pollRepository.GetCurrentPoll().Vote(option, Context.ConnectionId);
            foreach (var update in updated)
            {
                Clients.All.UpdateOption(update.Key, update.Value);
            }
        }
    }
}