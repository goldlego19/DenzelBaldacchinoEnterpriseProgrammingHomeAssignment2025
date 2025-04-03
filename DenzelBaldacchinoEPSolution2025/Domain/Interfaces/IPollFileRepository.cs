using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPollFileRepository
    {
        void AddPoll(Poll poll);
        IQueryable<Poll> LoadPolls();
    }
}
