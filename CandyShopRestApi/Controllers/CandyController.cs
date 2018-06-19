using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using System;
using System.Web.Http;

namespace CandyShopRestApi.Controllers
{
    public class CandyController : ApiController
    {
        private readonly ICandyService _service;

        public CandyController(ICandyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(CandyBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(CandyBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(CandyBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
