using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System.Collections.Generic;

namespace CandyShopService.Interfaces
{
    public interface IConfectionerService
    {
        List<ConfectionerViewModel> GetList();

        ConfectionerViewModel GetElement(int id);

        void AddElement(ConfectionerBindingModel model);

        void UpdElement(ConfectionerBindingModel model);

        void DelElement(int id);
    }
}
