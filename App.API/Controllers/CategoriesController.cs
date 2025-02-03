﻿using App.API.Filters;
using App.Application.Features.Categories;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Update;
using App.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class CategoriesController : CustomControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return CreateActionResult(await _categoryService.GetAllListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _categoryService.GetByIdAsync(id));
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts(int id)
        {
            return CreateActionResult(await _categoryService.GetCategoryWithProductsAsync(id));
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetCategoryWithProducts()
        {
            return CreateActionResult(await _categoryService.GetCategoryWithProductsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            return CreateActionResult(await _categoryService.CreateAsync(request));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest request)
        {
            return CreateActionResult(await _categoryService.UpdateAsync(request));
        }

        [ServiceFilter(typeof(NotFoundFilter<Category, int>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return CreateActionResult(await _categoryService.DeleteAsync(id));
        }
    }
}
