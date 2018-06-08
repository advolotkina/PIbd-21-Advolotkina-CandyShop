using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    public interface IReportService
    {
        void SaveCandyPrice(ReportBindingModel model);

        List<WarehousesLoadViewModel> GetWarehousesLoad();

        void SaveWarehousesLoad(ReportBindingModel model);

        List<CustomerOrdersViewModel> GetCustomerOrders(ReportBindingModel model);

        void SaveCustomerOrders(ReportBindingModel model);
    }
}
