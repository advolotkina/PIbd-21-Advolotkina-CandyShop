using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsBD
{
    public class ConfectionerServiceBD: IConfectionerService
    {
        private CandyShopDbContext context;

        public ConfectionerServiceBD(CandyShopDbContext context)
        {
            this.context = context;
        }

        public void AddElement(ConfectionerBindingModel model)
        {
            Confectioner element = context.Confectioners.FirstOrDefault(rec => rec.ConfectionerFIO == model.ConfectionerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть кондитер с таким ФИО");
            }
            context.Confectioners.Add(new Confectioner
            {
                ConfectionerFIO = model.ConfectionerFIO
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Confectioner element = context.Confectioners.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Confectioners.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public ConfectionerViewModel GetElement(int id)
        {
            Confectioner element = context.Confectioners.FirstOrDefault(rec => rec.Id == id);
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

        public List<ConfectionerViewModel> GetList()
        {
            List<ConfectionerViewModel> result = context.Confectioners
                .Select(rec => new ConfectionerViewModel
                {
                    Id = rec.Id,
                    ConfectionerFIO = rec.ConfectionerFIO
                })
                .ToList();
            return result;
        }

        public void UpdElement(ConfectionerBindingModel model)
        {
            Confectioner element = context.Confectioners.FirstOrDefault(rec =>
                                        rec.ConfectionerFIO == model.ConfectionerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть кондитер с таким ФИО");
            }
            element = context.Confectioners.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ConfectionerFIO = model.ConfectionerFIO;
            context.SaveChanges();
        }
    }
}
