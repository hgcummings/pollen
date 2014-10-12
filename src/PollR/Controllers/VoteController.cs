using Microsoft.AspNet.Mvc;
using PollR.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PollR.Controllers
{
    public class VoteController : Controller
    {
        private readonly PollRepository pollRepository;

        public VoteController(PollRepository pollRepository)
        {
            this.pollRepository = pollRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Poll CurrentPoll()
        {
            return pollRepository.GetCurrentPoll();
        }
    }
}
