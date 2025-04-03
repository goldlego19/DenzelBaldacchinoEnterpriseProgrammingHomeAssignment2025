using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filename;

        public PollFileRepository(IConfiguration configuration)
        {
            _filename = configuration["PollsFileName"];
            if (string.IsNullOrEmpty(_filename))
            {
                throw new ArgumentException("Poll File path is not configured in appsettings.json");
            }
        }

        public void CreatePoll(Poll poll)
        {
            // Load existing polls
            var polls = LoadPolls().ToList();

            // Add the new poll
            poll.CreatedAt = DateTime.Now;
            if (polls.Any())
            {
                poll.PollId = polls.Any() ? polls.Max(p => p.PollId) + 1 : 1; // Set the next available PollId
            }
            else
            {
                poll.PollId = 1; // First poll, set PollId to 1
            }
            
            polls.Add(poll);

            // Save the updated list of polls back to the file
            SavePolls(polls);
        }
        public void CreatePolls(List<Poll> polls)
        {
            // Load existing polls
            var existingPolls = LoadPolls().ToList();

            // Get current time to set for all polls
            var currentDateTime = DateTime.Now;

            // Set PollId for each poll and CreatedAt time
            foreach (var poll in polls)
            {
                poll.CreatedAt = currentDateTime;

                // Set PollId based on the existing polls
                if (existingPolls.Any())
                {
                    poll.PollId = existingPolls.Max(p => p.PollId) + 1; // Set the next available PollId
                }
                else
                {
                    poll.PollId = 1; // First poll, set PollId to 1
                }
            }

            // Add the new polls to the existing ones
            existingPolls.AddRange(polls);

            // Save the updated list of polls back to the file
            SavePolls(existingPolls);
        }



        public IQueryable<Poll> GetPolls()
        {
            return LoadPolls();
        }

        public void UpdatePolls(List<Poll> polls)
        {
            var existingPolls = LoadPolls().ToList();
            foreach (var poll in polls)
            {
                var existingPoll = existingPolls.SingleOrDefault(x => x.PollId == poll.PollId);
                if (existingPoll != null)
                {
                    existingPoll.Option1VotesCount = poll.Option1VotesCount;
                    existingPoll.Option2VotesCount = poll.Option2VotesCount;
                    existingPoll.Option3VotesCount = poll.Option3VotesCount;
                }
            }
            SavePolls(existingPolls);
        }

        public void Vote(List<Poll> polls)
        {
            var existingPolls = LoadPolls().ToList();
            foreach (var poll in polls)
            {
                var existingPoll = existingPolls.SingleOrDefault(x => x.PollId == poll.PollId);
                if (existingPoll != null)
                {
                    existingPoll.Option1VotesCount = poll.Option1VotesCount;
                    existingPoll.Option2VotesCount = poll.Option2VotesCount;
                    existingPoll.Option3VotesCount = poll.Option3VotesCount;
                }
            }
            SavePolls(existingPolls);
        }

        private IQueryable<Poll> LoadPolls()
        {
            if (!File.Exists(_filename))
            {
                return new List<Poll>().AsQueryable(); // Return an empty IQueryable if the file does not exist
            }

            try
            {
                var contents = File.ReadAllText(_filename);
                var listOfPolls = JsonConvert.DeserializeObject<List<Poll>>(contents);

                // Return an empty IQueryable if the deserialization fails (listOfPolls is null)
                return listOfPolls?.AsQueryable() ?? new List<Poll>().AsQueryable();
            }
            catch (Exception)
            {
                // If there is an error during deserialization, return an empty IQueryable
                return new List<Poll>().AsQueryable();
            }
        }



        private void SavePolls(List<Poll> polls)
        {
            // Serialize the list of polls and write it to the file
            var json = JsonConvert.SerializeObject(polls, Formatting.Indented);
            File.WriteAllText(_filename, json);
        }
    }
}
