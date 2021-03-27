using FinalProject.ApiModels.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Services;
using System.Threading.Tasks;

namespace FinalProject.Filters
{
    public class ItemExistActionFilter : ActionFilterAttribute
    {

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ILogger<ItemExistActionFilter> logger = (ILogger<ItemExistActionFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ItemExistActionFilter>));

            context.ActionArguments.TryGetValue("itemId", out object itemIdObject);
            if (context.ActionArguments.TryGetValue("model", out object model)
                || itemIdObject != null)
            {
                int itemId;
                if (model != null)
                {
                    var requestModel = (IHasItemId)model;
                    itemId = requestModel.ItemId;
                }
                else
                {
                    itemId = int.Parse(itemIdObject.ToString());
                }
                IItemService itemService = (IItemService)context.HttpContext.RequestServices.GetService(typeof(IItemService));



                var item = await itemService.Get(itemId);
                if (item == null)
                {
                    logger.LogWarning("Item not found with itemId = {itemId}", itemId);
                    context.Result = new ContentResult()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Content = "Item does not exist!"
                    };
                    return;
                }
            }
            await next();
        }
    }
}
