using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using BusinessLayer.Services.BookService;
using BusinessLayer.Services.UserService;
using DAL.Entities;
using DAL.Repository;
using library.Controllers;
using library.Maps;
using library.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace library
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new BookMapProfile());
                cfg.AddProfile(new UserMapProfile());
                cfg.AddProfile(new BorrowingBookMapProfile());
            } );
            return mappingConfig.CreateMapper();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

       
            var builder = new ContainerBuilder();
            object value = builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<HomeController>().AsSelf();
            builder.RegisterType<AccountsController>().AsSelf();
            builder.RegisterType<BookController>().AsSelf();
            builder.RegisterType<BorrowController>().AsSelf();
            builder.RegisterType<UserController>().AsSelf();
            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<BookAppServices>().As<IBookAppServices>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<UserAppServices>().As<IUserAppServices>();
            builder.Register(context => GetMapper()).As<IMapper>();

         


            var container = builder.Build();
       
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);



        }
    }
}
