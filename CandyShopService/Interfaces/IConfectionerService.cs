using CandyShopService.Attributes;
using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с работниками кондитерской")]
    public interface IConfectionerService
    {
        [CustomMethod("Метод получения списка работников кондитерской")]
        List<ConfectionerViewModel> GetList();

        [CustomMethod("Метод получения работника по id")]
        ConfectionerViewModel GetElement(int id);

        [CustomMethod("Метод добавления работника")]
        void AddElement(ConfectionerBindingModel model);

        [CustomMethod("Метод изменения информации о работнике")]
        void UpdElement(ConfectionerBindingModel model);

        [CustomMethod("Метод удаления работника")]
        void DelElement(int id);
    }
}
