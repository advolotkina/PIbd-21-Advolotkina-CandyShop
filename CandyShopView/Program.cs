using CandyShopService.ImplementationsList;
using CandyShopService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace CandyShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIngredientService, IngredientServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IConfectionerService, ConfectionerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICandyService, CandyServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IWarehouseService, WarehouseServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());
            
            return currentContainer;
        }
    }
}
