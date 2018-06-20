using CandyShopService.Attributes;
using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с клиентами")]
    public interface ICustomerService
    {
        [CustomMethod("Метод олучения списка клиентов")]
        List<CustomerViewModel> GetList();

        [CustomMethod("Метод получения клиента по id")]
        CustomerViewModel GetElement(int id);

        [CustomMethod("Метод добавления клиента")]
        void AddElement(CustomerBindingModel model);

        [CustomMethod("Метод изменения данных по клиенту")]
        void UpdElement(CustomerBindingModel model);

        [CustomMethod("Метод удаления клиента")]
        void DelElement(int id);
    }
}
