﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NToastNotify;
using Portal.core.News;
using Portal.Service.News;
using Portal.Standard.Core.News;
using Portal.Standard.Service.Media;
using Portal.Web.Framework.Filters;
using Presentation3.Portal.Areas.Admin.Models.Content;
using QTasMarketing.Web.Areas.Admin.Models.Content;
using QTasMarketing.Web.Areas.Admin.Models.Media;

namespace Presentation3.Portal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContentController : Controller
    {
        #region Feild

        private readonly IMapper _mapper;
        private readonly INewsService _newsService;
        private readonly IToastNotification _toastNotification;
        private readonly IPictureService _pictureService;


        #endregion

        #region  Ctor

        public ContentController(INewsService newsService, IMapper mapper, IToastNotification toastNotification,
            IPictureService pictureService)
        {
            _newsService = newsService;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _pictureService = pictureService;
        }


        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            var groups = PrepareGroupSelectedListItem();
            return View(new ContentViewModel()
            {
                SelectListItems = groups
            });
        }
        
        private List<SelectListItem> PrepareGroupSelectedListItem()
        {
            var groups = _newsService.GetGroups().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.GetFormattedBreadCrumb(_newsService)
            }).ToList();
            return groups;
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContentViewModel contentViewModel, bool continueEditing)
        {

            if (ModelState.IsValid)
            {

                var content = _mapper.Map<Content>(contentViewModel);

                _newsService.AddContent(content);
                if (content.Id > 0)
                {
                    _toastNotification.AddSuccessToastMessage("عملیات  با موفقیت صورت پذیرفت");


                    if (continueEditing)
                    {
                        return RedirectToAction("Edit", new { contentId = content.Id });
                    }

                    return RedirectToAction("List");
                }

                _toastNotification.AddErrorToastMessage("خطا در انجام عملیات");

            }

            return RedirectToAction("List");
        }
        
        public IActionResult Edit(long? contentId)
        {
            if (contentId == null)
                _toastNotification.AddErrorToastMessage("خطا در پار متر ورودی");

            var content = _newsService.GetContentById(contentId.GetValueOrDefault());
                        var contentViewModel = _mapper.Map<ContentViewModel>(content);
            var groups = PrepareGroupSelectedListItem();

            contentViewModel.SelectListItems = groups;

            return View(contentViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ContentViewModel model)
        {
            if (model == null)
                _toastNotification.AddErrorToastMessage("خطا در پار متر ورودی");

            var content = _newsService.GetContentById(model.Id);
            if (content == null)
                RedirectToAction("List");
            content = _mapper.Map<Content>(model);
            
            _newsService.EditContent(content);
            _toastNotification.AddSuccessToastMessage("عملیات  با موفقیت صورت پذیرفت");

            return RedirectToAction("List");
        }

        public JsonResult Content_Read([DataSourceRequest] DataSourceRequest request)
        {
            var contents = _newsService.GetContents();

            var groupsModel = _mapper.Map<List<Content>, List<ContentViewModel>>(contents);

            return Json(groupsModel.ToDataSourceResult(request));
        }

        public JsonResult ComboBox_Read()
        {
            var groups = _newsService.GetGroups();

            var groupsModel = _mapper.Map<List<Group>, List<GroupViewModel>>(groups);

            return Json(groupsModel);
        }

        public ActionResult Save(IEnumerable<IFormFile> files, byte[] imageByteArray)
        {
            if (imageByteArray == null) throw new ArgumentNullException(nameof(imageByteArray));

            foreach (var file in files)
            {
                // The Name of the Upload component is "files"
                if (file != null)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        imageByteArray = imageBytes;

                    }
                }
            }


            // Return an empty string to signify success
            return Content("");
        }

        [HttpPost]
        public IActionResult AddContentPicture([FromBody]ContentPictureViewModel contentPictureViewModel)
        {
           
                var contePicture = _mapper.Map<ContentPicture>(contentPictureViewModel);
                _newsService.AddContentPicture(contePicture);
              return Json(new
                {
                    success = true,

                 
                }, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = { new StringEnumConverter() }
                });
            
        }

        public IActionResult ContentPicture_Read([DataSourceRequest] DataSourceRequest request, long? contentId)
        {
            if (contentId == null)
                _toastNotification.AddErrorToastMessage("خطا در پار متر ورودی");

            var contentPictures = _newsService.GetContentPictures(contentId.GetValueOrDefault());
            var contentPictureViewModels = contentPictures.Select(x =>
                {
                    var picture = _pictureService.GetPictureById(x.PictureId);
                    if (picture == null)
                        throw new Exception("تصاویر نمی توانند بارگذاری  شوند");
                    var m = new ContentPictureViewModel
                    {
                        Id = x.Id,
                        ContentId = x.ContentId,
                        PictureUrl = _pictureService.GetPictureUrl(picture),
                        DisplayOrder = x.DisplayOrder,
                        PictureId = x.PictureId


                    };
                    return m;}).ToList();
             
            return Json(contentPictureViewModels.ToDataSourceResult(request));
        }

        public IActionResult Delete_ContentPicture()
        {
            throw new NotImplementedException();
        }
    }
}