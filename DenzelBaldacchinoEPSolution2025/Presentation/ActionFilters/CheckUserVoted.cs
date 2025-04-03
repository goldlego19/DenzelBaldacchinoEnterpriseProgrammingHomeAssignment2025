using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Presentation.ActionFilters
{
    public class CheckUserVoted: ActionFilterAttribute
    {
        private IPollRepository GetPollRepository(ActionExecutingContext context)
        {
            IPollRepository _pollRepository = context.HttpContext.RequestServices.GetService<PollRepository>();
            IPollRepository _pollFileRepository = context.HttpContext.RequestServices.GetService<PollFileRepository>();
            // Get the IConfiguration service from the context
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            // Read the value from configuration
            bool useFileStorage = bool.Parse(configuration["PollFileSetting"]);

            return useFileStorage ? _pollFileRepository : _pollRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user == null)
            {
                var pollId = (int)context.ActionArguments["pollID"];
                var hasVoted = CheckifUserHasVoted(user.FindFirstValue(ClaimTypes.NameIdentifier), pollId, context);
                if (hasVoted)
                {
                    context.Result = new RedirectToActionResult("Index", "Poll", null);
                }
            }
            base.OnActionExecuting(context);
        }

        private bool CheckifUserHasVoted(string userId, int pollId, ActionExecutingContext context)
        {
           
            
            var poll = GetPollRepository(context).GetPolls().SingleOrDefault(p => p.PollId == pollId);
            if (poll.VotedUserIds.Contains(userId))
            {
                return true;
            }
            return false;
        }

        

    }
}
