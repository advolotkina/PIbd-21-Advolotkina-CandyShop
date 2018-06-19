using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using System;
using System.Web.Http;

namespace CandyShopRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetWarehousesLoad()
        {
            var list = _service.GetWarehousesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerOrders(ReportBindingModel model)
        {
            var list = _service.GetCustomerOrders(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveCandyPrice(ReportBindingModel model)
        {
            _service.SaveCandyPrice(model);
        }

        [HttpPost]
        public void SaveWarehousesLoad(ReportBindingModel model)
        {
            _service.SaveWarehousesLoad(model);
        }

        [HttpPost]
        public void SaveCustomerOrders(ReportBindingModel model)
        {
            _service.SaveCustomerOrders(model);
        }
    }
}
