using Autofac;
using WebServerProject.Controllers;
using WebServerProject.Services;

namespace WebServerProject.Server
{
    public interface IConfigurator
    {
        void ConfigureMiddleware(MiddlewareBuilder builder);
        void ConfigureDependencies(ContainerBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<ToDoService>().As<IToDoService>();
            builder.RegisterType<AccountController>();
            builder.RegisterType<ToDoController>();
        }

        public void ConfigureMiddleware(MiddlewareBuilder builder)
        {
            builder.Use<StaticFilesMiddleware>().Use<AuthMiddleware>().Use<MvcMiddleware>();
        }
    }
}
