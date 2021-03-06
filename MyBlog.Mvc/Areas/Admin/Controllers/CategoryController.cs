using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Dtos.CategoryDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Utilities;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();
            //İşlemin hatalı mı , hatalıysa mesajını data içerisinde barındırdığımız için ekstra bir kontrole gerek duymuyoruz
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.Add(categoryAddDto, "Eray Bakır");
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel 
                    {
                        CategoryDto=result.Data,
                        CategoryAddPartial=await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                    });

                    return Json(categoryAjaxModel);
                }    
            }

            var categoryAjaxInvalidrModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
            });

            return Json(categoryAjaxInvalidrModel);
        }

       public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAll();
            var categories = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions {ReferenceHandler= ReferenceHandler.Preserve});

            return Json(categories);

        }


        [HttpPost]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.Delete(categoryId,"Eray Bakır");
            var ajaxResult = JsonSerializer.Serialize(result, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            return Json(ajaxResult);
        }

    }
}
