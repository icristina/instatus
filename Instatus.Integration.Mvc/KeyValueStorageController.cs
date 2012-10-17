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
            int pageIndex = 0, 
            int pageSize = 20)
        {
            ViewData.Model = KeyValueStorage.Query(null);

            return View();
        }

        public ActionResult Details(string id)
        {
            var model = KeyValueStorage.Get(id);

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
        public ActionResult Create(string key, TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<TModel>(viewModel);

                KeyValueStorage.AddOrUpdate(key, model);

                return RedirectToAction("Details", new { id = key });
            }
            else
            {
                ViewData.Model = viewModel;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = KeyValueStorage.Get(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = Mapper.Map<TViewModel>(model);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, TViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = KeyValueStorage.Get(id);

                if (model == null)
                {
                    return HttpNotFound();
                }

                Mapper.Inject(model, viewModel);

                KeyValueStorage.AddOrUpdate(id, model);

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                ViewData.Model = viewModel;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            KeyValueStorage.Delete(id);

            return RedirectToAction("Index");
        }

        public KeyValueStorageController(IKeyValueStorage<TModel> keyValueStorage, IMapper mapper)
        {
            KeyValueStorage = keyValueStorage;
            Mapper = mapper;
        }
    }
}
