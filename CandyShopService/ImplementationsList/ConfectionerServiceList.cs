using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsList
{
    public class ConfectionerServiceList : IConfectionerService
    {
        private DataListSingleton source;

        public ConfectionerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ConfectionerViewModel> GetList()
        {
            List<ConfectionerViewModel> result = source.Confectioners
                .Select(rec => new ConfectionerViewModel
                {
                    Id = rec.Id,
                    ConfectionerFIO = rec.ConfectionerFIO
                })
                .ToList();
            return result;
        }

        public ConfectionerViewModel GetElement(int id)
        {
            Confectioner element = source.Confectioners.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ConfectionerViewModel
                {
                    Id = element.Id,
                    ConfectionerFIO = element.ConfectionerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ConfectionerBindingModel model)
        {
            Confectioner element = source.Confectioners.FirstOrDefault(rec => rec.ConfectionerFIO == model.ConfectionerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть кондитер с таким ФИО");
            }
            int maxId = source.Confectioners.Count > 0 ? source.Confectioners.Max(rec => rec.Id) : 0;
            source.Confectioners.Add(new Confectioner
            {
                Id = maxId + 1,
                ConfectionerFIO = model.ConfectionerFIO
            });
        }

        public void UpdElement(ConfectionerBindingModel model)
        {
            Confectioner element = source.Confectioners.FirstOrDefault(rec =>
                                        rec.ConfectionerFIO == model.ConfectionerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть кондитер с таким ФИО");
            }
            element = source.Confectioners.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ConfectionerFIO = model.ConfectionerFIO;
        }

        public void DelElement(int id)
        {
            Confectioner element = source.Confectioners.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Confectioners.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
