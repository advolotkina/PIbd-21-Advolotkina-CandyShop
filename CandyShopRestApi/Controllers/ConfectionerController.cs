using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using System;
using System.Web.Http;

namespace CandyShopRestApi.Controllers
{
    public class ConfectionerController : ApiController
    {
        private readonly IConfectionerService _service;

        public ConfectionerController(IConfectionerService service)
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
        public void AddElement(ConfectionerBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(ConfectionerBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(ConfectionerBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
