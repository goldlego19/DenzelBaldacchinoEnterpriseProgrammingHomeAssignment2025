using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        public void CreatePoll(Poll poll)
        {
            poll.CreatedAt = DateTime.Now;

            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        public void CreatePolls(List<Poll> polls)
        {
            var currentDateTime = DateTime.Now;
            foreach (var poll in polls)
            {
                poll.CreatedAt = currentDateTime;
                _context.Polls.Add(poll);
            }
            _context.SaveChanges();
        }
        public IQueryable<Poll> GetPolls()
        {
            return _context.Polls;
        }
        public void UpdatePolls(List<Poll> polls)
        {
            foreach (var poll in polls)
            {
                var currentPoll = GetPolls().SingleOrDefault(x => x.PollId == poll.PollId);
                currentPoll.Option1VotesCount = poll.Option1VotesCount;
                currentPoll.Option2VotesCount = poll.Option2VotesCount;
                currentPoll.Option3VotesCount = poll.Option3VotesCount;
            }
            _context.SaveChanges();
        }
        public void Vote(List<Poll> polls)
        {
            foreach (var poll in polls)
            {
                var currentPoll = GetPolls().SingleOrDefault(x => x.PollId == poll.PollId);
                currentPoll.Option1VotesCount = poll.Option1VotesCount;
                currentPoll.Option2VotesCount = poll.Option2VotesCount;
                currentPoll.Option3VotesCount = poll.Option3VotesCount;
            }
            _context.SaveChanges();
        }
    }
}
