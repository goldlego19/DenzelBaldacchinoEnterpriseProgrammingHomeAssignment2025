using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Presentation.Models;
using System.Security.Claims;


namespace Presentation.Controllers
{
   
    public class PollController : Controller
    {
        
        PollRepository _pollRepository;
        IPollRepository _pollFileRepository;
        public readonly bool _useFileStorage;
        public PollController(PollRepository pollRepository, IPollRepository pollFileRepository, IConfiguration configuration)
        {
            _pollRepository = pollRepository;
            _pollFileRepository = pollFileRepository;
            _useFileStorage = configuration.GetValue<bool>("PollFileSetting");//Change This to work with DB or FIle
        }

        private IPollRepository GetPollRepository()
        {
            return _useFileStorage ? _pollFileRepository : _pollRepository;
        }

        public IActionResult Index()
        {
            List<Poll> polls = GetPollRepository().GetPolls()
                .OrderByDescending(p => p.CreatedAt) // Sorting by date in descending order
                .ToList();
            return View(polls);
        }
        [CheckUserVoted]
        [Authorize]
        [HttpGet]
        public IActionResult Vote(int polID)
        {
            Poll poll = GetPollRepository().GetPolls().SingleOrDefault(p => p.PollId == polID);
            if (poll ==null)
            {
                return NotFound();
            }
            return View(poll);
        }
        [HttpPost]
        public IActionResult Vote(int polID,string selectedOption)
        {
            Poll poll = GetPollRepository().GetPolls().SingleOrDefault(p => p.PollId == polID);
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null) return Unauthorized();

            if (poll == null)
            {
                return NotFound();
            }

            if (poll.VotedUserIds.Contains(userID))
            {
                TempData["message"] = "You have already voted on this poll";
                return RedirectToAction("Index");
            }

            switch (selectedOption)
            {
                case "Option1":
                    poll.Option1VotesCount++;
                    break;
                case "Option2":
                    poll.Option2VotesCount++;
                    break;
                case "Option3":
                    poll.Option3VotesCount++;
                    break;

            }
            poll.VotedUserIds.Add(userID);

            GetPollRepository().Vote(new List<Poll> { poll });
            TempData["message"] = "Your vote has been recorded!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View(new Poll());

        }

        [HttpPost]
        public IActionResult Create(Poll poll,bool update)
        {
            if (poll!=null)
            {
                if (update)
                {
                    GetPollRepository().UpdatePolls(new List<Poll> { poll });
                }
                else
                {
                    GetPollRepository().CreatePolls(new List<Poll> { poll });
                }
                TempData["message"] = "Poll saved successfully";

            }
            return RedirectToAction("Index");
        }
        
    }
}
