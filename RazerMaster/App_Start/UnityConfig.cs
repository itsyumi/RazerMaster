using RazerMasterLibrary.DbContextFactory;
using RazerMasterLibrary.DbContextFactory.Interfaces;
using RazerMasterLibrary.UnitOfWork;
using RazerMasterLibrary.UnitOfWork.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace RazerMaster
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IDbContextFactory, DbContextFactory>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}