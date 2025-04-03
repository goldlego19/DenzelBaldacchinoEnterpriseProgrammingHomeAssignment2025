using System;
using System.Collections.Generic;
using Domain.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext
{
    public class PollDbContext : IdentityDbContext<CustomUser>
    {
        public PollDbContext(DbContextOptions<PollDbContext> options) : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; }

   
    }
}
