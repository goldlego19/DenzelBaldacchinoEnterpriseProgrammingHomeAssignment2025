using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollFileRepository: IPollFileRepository
    {
        private string _filename;

        public PollFileRepository(IConfiguration configuration)
        {
            _filename = configuration["PollsFile"];
            if (string.IsNullOrEmpty(_filename))
            {
                throw new ArgumentException("Poll File path is not configured in appsettings.json");
            }
        }
        public void AddPoll(Poll myPoll)
        {
            var ListofExistingPolls = LoadPolls().ToList();
            ListofExistingPolls.Add(myPoll);
            System.IO.File.WriteAllText(_filename, JsonConvert.SerializeObject(ListofExistingPolls));
        }
        public IQueryable<Poll> LoadPolls()
        {
            if (System.IO.File.Exists(_filename) == false)
            {
                return new List<Poll>().AsQueryable();
            }
            string contents = System.IO.File.ReadAllText(_filename);
            var listofPolls = JsonConvert.DeserializeObject<List<Poll>>(contents);
            return listofPolls.AsQueryable();
        }
    }
}
