using Microsoft.AspNet.Mvc;
using PollR.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PollR.Controllers
{
    public class AdminController : Controller
    {
        private readonly PollRepository pollRepository;
        private readonly IHubContext pollHub;

        public AdminController(PollRepository pollRepository, IConnectionManager connectionManager)
        {
            this.pollRepository = pollRepository;
            this.pollHub = connectionManager.GetHubContext<PollHub>();
        }

        public IActionResult Index()
        {
            ViewBag.CurrentSession = pollRepository.GetCurrentSession();
            return View(pollRepository.GetSnapshots());
        }

        [HttpPost]
        public IActionResult Admin()
        {
            return new RedirectResult("Index");
        }

        [HttpPost]
        public IActionResult StartSession(string sessionName)
        {
            pollRepository.StartSession(sessionName);
            return new RedirectResult("Index");
        }

        [HttpPost]
        public IActionResult Snapshot(string snapshotName)
        {
            pollRepository.Store(snapshotName);
            pollRepository.Reset();
            pollHub.Clients.All.Reset();
            return new RedirectResult("Index");
        }
    }
}
