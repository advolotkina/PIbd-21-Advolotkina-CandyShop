using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    public interface ICandyService
    {
        List<CandyViewModel> GetList();

        CandyViewModel GetElement(int id);

        void AddElement(CandyBindingModel model);

        void UpdElement(CandyBindingModel model);

        void DelElement(int id);
    }
}
