using Autofac;
using Repositories;
using Services;

namespace FinalProject
{
    public class ServiceModules: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.AddMappingProfiles();

            builder.RegisterType<ItemRepository>().As<IItemRepository>();
            builder.RegisterType<ItemService>().As<IItemService>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<ApiExceptionRepository>().As<IApiExceptionRepository>();
            builder.RegisterType<ApiExceptionService>().As<IApiExceptionService>();
        }
    }
}
