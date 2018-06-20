using CandyShopService.Attributes;
using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с кондитерскими изделиями")]
    public interface ICandyService
    {
        [CustomMethod("Метод получения списка кондитерских изделий")]
        List<CandyViewModel> GetList();

        [CustomMethod("Метод получения кондитерского изделия по id")]
        CandyViewModel GetElement(int id);

        [CustomMethod("Метод добавления кондитерского изделия")]
        void AddElement(CandyBindingModel model);

        [CustomMethod("Метод изменения информации о кондитерском изделии")]
        void UpdElement(CandyBindingModel model);

        [CustomMethod("Метод удаления кондитерского изделия")]
        void DelElement(int id);
    }
}
