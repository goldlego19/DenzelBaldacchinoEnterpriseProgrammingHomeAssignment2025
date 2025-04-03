using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPollRepository
    {
        void CreatePoll(Poll poll);
        void CreatePolls(List<Poll> polls);
        IQueryable<Poll> GetPolls();
        void UpdatePolls(List<Poll> polls);
        void Vote(List<Poll> polls);
    }
}
