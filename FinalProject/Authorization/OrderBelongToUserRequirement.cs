using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinalProject.Authorization
{
    public class OrderBelongToUserRequirement : IAuthorizationRequirement
    {
    }

    public class OrderBelongToUserHandler : AuthorizationHandler<OrderBelongToUserRequirement, Order>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public OrderBelongToUserHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderBelongToUserRequirement requirement, Order resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }

            if (resource.CreatedBy == appUser.Id)
            {
                context.Succeed(requirement);
            }
        }
    }
}
