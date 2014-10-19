using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PollR.Model;
using System.Threading.Tasks;

namespace PollR.Controllers
{
    [HubName("pollHub")]
    public class PollHub : Hub
    {
        private readonly PollRepository pollRepository;

        public PollHub(PollRepository pollRepository)
        {
            this.pollRepository = pollRepository;
        }

        public void Register()
        {
            foreach (var option in pollRepository.GetCurrentPoll().Options)
            {
                Clients.Caller.UpdateOption(option.Key, option.Value);
            }
        }

        public void Vote(string option)
        {
            var updated = pollRepository.GetCurrentPoll().Vote(option, Context.ConnectionId);
            foreach (var update in updated)
            {
                Clients.All.UpdateOption(update.Key, update.Value);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            pollRepository.GetCurrentPoll().CancelPreviousVote(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

    }
}