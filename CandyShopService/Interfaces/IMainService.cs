using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    public interface IMainService
    {
        List<PurchaseOrderViewModel> GetList();

        void CreatePurchaseOrder(PurchaseOrderBindingModel model);

        void TakePurchaseOrderInWork(PurchaseOrderBindingModel model);

        void FinishPurchaseOrder(int id);

        void PayPurchaseOrder(int id);

        void PutIngredientOnStock(WarehouseIngredientBindingModel model);
    }
}
