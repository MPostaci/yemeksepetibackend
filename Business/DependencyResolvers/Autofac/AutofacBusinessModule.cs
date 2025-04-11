using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Configuration;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<EfProductDal>().As<IProductDal>();

            builder.RegisterType<CategoryManager>().As<ICategoryService>();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<RestaurantManager>().As<IRestaurantService>();
            builder.RegisterType<EfRestaurantDal>().As<IRestaurantDal>();

            builder.RegisterType<RestaurantUserManager>().As<IRestaurantUserService>();
            builder.RegisterType<EfRestaurantUserDal>().As<IRestaurantUserDal>();

            builder.RegisterType<CartItemManager>().As<ICartItemService>();
            builder.RegisterType<EfCartItemDal>().As<ICartItemDal>();

            builder.RegisterType<CartManager>().As<ICartService>();
            builder.RegisterType<EfCartDal>().As<ICartDal>();

            builder.RegisterType<OrderManager>().As<IOrderService>();
            builder.RegisterType<EfOrderDal>().As<IOrderDal>();

            builder.RegisterType<OrderDetailManager>().As<IOrderDetailService>();
            builder.RegisterType<EfOrderDetailDal>().As<IOrderDetailDal>();

            builder.RegisterType<OrderStatusManager>().As<IOrderStatusService>();
            builder.RegisterType<EfOrderStatusDal>().As<IOrderStatusDal>();

            builder.RegisterType<OrderBridgeStatusManager>().As<IOrderBridgeStatusService>();
            builder.RegisterType<EfOrderBridgeStatus>().As<IOrderBridgeStatusDal>();



            //builder.RegisterType<YemekSepetiDBContext>();
            // builder.RegisterType<YemekSepetiDBContext>().SingleInstance();




            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
