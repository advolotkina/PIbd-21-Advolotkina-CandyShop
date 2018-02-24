using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<ConfectionerViewModel> result = new List<ConfectionerViewModel>();
            for (int i = 0; i < source.Confectioners.Count; ++i)
            {
                result.Add(new ConfectionerViewModel
                {
                    Id = source.Confectioners[i].Id,
                    ConfectionerFIO = source.Confectioners[i].ConfectionerFIO
                });
            }
            return result;
        }

        public ConfectionerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Confectioners.Count; ++i)
            {
                if (source.Confectioners[i].Id == id)
                {
                    return new ConfectionerViewModel
                    {
                        Id = source.Confectioners[i].Id,
                        ConfectionerFIO = source.Confectioners[i].ConfectionerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ConfectionerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Confectioners.Count; ++i)
            {
                if (source.Confectioners[i].Id > maxId)
                {
                    maxId = source.Confectioners[i].Id;
                }
                if (source.Confectioners[i].ConfectionerFIO == model.ConfectionerFIO)
                {
                    throw new Exception("Уже есть кондитер с таким ФИО");
                }
            }
            source.Confectioners.Add(new Confectioner
            {
                Id = maxId + 1,
                ConfectionerFIO = model.ConfectionerFIO
            });
        }

        public void UpdElement(ConfectionerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Confectioners.Count; ++i)
            {
                if (source.Confectioners[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Confectioners[i].ConfectionerFIO == model.ConfectionerFIO && 
                    source.Confectioners[i].Id != model.Id)
                {
                    throw new Exception("Уже есть кондитер с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Confectioners[index].ConfectionerFIO = model.ConfectionerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Confectioners.Count; ++i)
            {
                if (source.Confectioners[i].Id == id)
                {
                    source.Confectioners.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
