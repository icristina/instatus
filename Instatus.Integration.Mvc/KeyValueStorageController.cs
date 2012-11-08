using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;
using System.Web.SessionState;
using Instatus.Core.Models;
using Instatus.Core.Extensions;

namespace Instatus.Integration.Mvc
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class KeyValueStorageController<TModel, TViewModel> : Controller
        where TModel : class
        where TViewModel : class
    {
        public IKeyValueStorage<TModel> KeyValueStorage { get; private set; }
        public IMapper Mapper { get; private set; }

        public ActionResult Index(
            string partitionKey,
            int pageIndex = 0, 
            int pageSize = 20)
        {
            ViewData.Model = KeyValueStorage.Query(partitionKey, null);

            return View();
        }

        public ActionResult Details(string partitionKey, string rowKey)
        {
            var model = KeyValueStorage.Get(partitionKey, rowKey);

            if (model == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = Mapper.Map<TViewModel>(model);

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = Activator.CreateInstance<TViewModel>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string partitionKey, string rowKey, TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<TModel>(viewModel);

                KeyValueStorage.AddOrUpdate(partitionKey, rowKey, model);

                return RedirectToAction("Details", new { partitionKey = partitionKey, rowKey = rowKey });
            }
            else
            {
                ViewData.Model = viewModel;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(string partitionKey, string rowKey)
        {
            var model = KeyValueStorage.Get(partitionKey, rowKey);

            if (model == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = Mapper.Map<TViewModel>(model);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string partitionKey, string rowKey, TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = KeyValueStorage.Get(partitionKey, rowKey);

                if (model == null)
                {
                    return HttpNotFound();
                }

                Mapper.Inject(model, viewModel);

                KeyValueStorage.AddOrUpdate(partitionKey, rowKey, model);

                return RedirectToAction("Details", new { partitionKey = partitionKey, rowKey = rowKey });
            }
            else
            {
                ViewData.Model = viewModel;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string partitionKey, string rowKey)
        {
            KeyValueStorage.Delete(partitionKey, rowKey);

            return RedirectToAction("Index");
        }

        public KeyValueStorageController(IKeyValueStorage<TModel> keyValueStorage, IMapper mapper)
        {
            KeyValueStorage = keyValueStorage;
            Mapper = mapper;
        }
    }
}
