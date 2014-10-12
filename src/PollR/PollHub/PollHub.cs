using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace PollR.PollHub
{
    [HubName("poll")]
    public class PollHub : Hub
    {
    }
}