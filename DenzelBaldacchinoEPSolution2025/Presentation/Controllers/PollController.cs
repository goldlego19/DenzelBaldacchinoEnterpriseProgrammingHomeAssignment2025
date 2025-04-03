using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        PollRepository _pollRepository;
        public PollController(PollRepository pollRepository) { _pollRepository = pollRepository; }


        public IActionResult Index()
        {
            List<Poll> polls = _pollRepository.GetPolls()
                .OrderByDescending(p => p.CreatedAt) // Sorting by date in descending order
                .ToList();
            return View(polls);
        }
        [HttpGet]
        public IActionResult Vote(int polID)
        {
            Poll poll = _pollRepository.GetPolls().SingleOrDefault(p => p.PollId == polID);
            if (poll ==null)
            {
                return NotFound();
            }
            return View(poll);
        }
        [HttpPost]
        public IActionResult Vote(int polID,string selectedOption)
        {
            Poll poll = _pollRepository.GetPolls().SingleOrDefault(p => p.PollId == polID);
            if (poll == null)
            {
                return NotFound();
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
            _pollRepository.Vote(new List<Poll> { poll });
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
                    _pollRepository.UpdatePolls(new List<Poll> { poll });
                }
                else
                {
                    _pollRepository.CreatePolls(new List<Poll> { poll });
                }
                TempData["message"] = "Poll saved successfully";

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {

            Poll poll = _pollRepository.GetPolls().SingleOrDefault(p => p.PollId == id);

            if (poll == null)
            {
                return NotFound(); // Handle case where poll doesn't exist
            }

            return View(poll); // Return the view with the poll data
        }
        [HttpPost]
        public IActionResult Update(int id,Poll updatedPoll)
        {
            if (ModelState.IsValid)
            {
                Poll pastPoll =_pollRepository.GetPolls().SingleOrDefault(pastPoll => pastPoll.PollId == id);
                if(pastPoll == null)
                {  return NotFound(); }

                pastPoll.Title = updatedPoll.Title;
                pastPoll.Option1Text = updatedPoll.Option1Text;
                pastPoll.Option2Text = updatedPoll.Option2Text;
                pastPoll.Option3Text = updatedPoll.Option3Text;

                _pollRepository.UpdatePolls(new List<Poll> { pastPoll });
                TempData["message"] = "Poll Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(updatedPoll);
        }
    }
}
