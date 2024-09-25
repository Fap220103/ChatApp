using ChatApp_Api.Data;
using ChatApp_Api.Extensions;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatApp_Api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            var userId = resultContext.HttpContext.User.GetUserId();
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await uow.UserRepository.GetByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();

        }
    }
}
