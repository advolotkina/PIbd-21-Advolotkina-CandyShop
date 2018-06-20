using CandyShopService.Attributes;
using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<PurchaseOrderViewModel> GetList();

        [CustomMethod("Метод создания заказа")]
        void CreatePurchaseOrder(PurchaseOrderBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void TakePurchaseOrderInWork(PurchaseOrderBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishPurchaseOrder(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayPurchaseOrder(int id);

        [CustomMethod("Метод пополнения компонент на складе")]
        void PutIngredientOnStock(WarehouseIngredientBindingModel model);
    }
}
