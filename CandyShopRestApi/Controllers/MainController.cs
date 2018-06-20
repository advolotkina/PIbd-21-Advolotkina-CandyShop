using CandyShopService;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using System;
using System.Web.Http;

namespace CandyShopRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
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

        [HttpPost]
        public void CreatePurchaseOrder(PurchaseOrderBindingModel model)
        {
            _service.CreatePurchaseOrder(model);
        }

        [HttpPost]
        public void TakePurchaseOrderInWork(PurchaseOrderBindingModel model)
        {
            _service.TakePurchaseOrderInWork(model);
        }

        [HttpPost]
        public void FinishPurchaseOrder(PurchaseOrderBindingModel model)
        {
            _service.FinishPurchaseOrder(model.Id);
        }

        [HttpPost]
        public void PayPurchaseOrder(PurchaseOrderBindingModel model)
        {
            _service.PayPurchaseOrder(model.Id);
        }

        [HttpPost]
        public void PutIngredientOnStock(WarehouseIngredientBindingModel model)
        {
            _service.PutIngredientOnStock(model);
        }

        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}
