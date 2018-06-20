using CandyShopService.Attributes;
using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportService
    {
        [CustomMethod("Метод сохранения цен на конфеты")]
        void SaveCandyPrice(ReportBindingModel model);

        [CustomMethod("Метод получения списка складов с количеством имеющихся ингредиентов")]
        List<WarehousesLoadViewModel> GetWarehousesLoad();

        [CustomMethod("Метод сохранения списка списка складов")]
        void SaveWarehousesLoad(ReportBindingModel model);

        [CustomMethod("Метод получения списка заказов клиентов")]
        List<CustomerOrdersViewModel> GetCustomerOrders(ReportBindingModel model);

        [CustomMethod("Метод сохранения списка заказов клиентов")]
        void SaveCustomerOrders(ReportBindingModel model);
    }
}
