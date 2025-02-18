﻿using EcommerceWebMvc.Models;
using EcommerceWebMvc.services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebMvc.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext Context { get; }
        private readonly int pageSize = 16;
        public StoreController(ApplicationDbContext context)
        {
            Context = context;
        }


        public IActionResult Index(int pageIndex, string? search, string? brand, string? category, string? sort)
        {
            IQueryable<Product> query = Context.Products;

            // search functionality
            if (search != null && search.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(search));
            }


            if (category != null && category.Length > 0)
            {
                query = query.Where(p => p.Category.Contains(category));
            }

            // sort functionality
            if (sort == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sort == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderByDescending(p => p.Brand);
            }



            // pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            // ex 24/16=2
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var products = query.ToList();

            ViewBag.Products = products;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            var storeSearchModel = new StoreSearchModel()
            {
                Search = search,
                Brand = brand,
                Category = category,
                Sort = sort
            };

            return View(storeSearchModel);
        }

        public IActionResult Details(int id)
        {
            var product = Context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Store");
            }

            return View(product);
        }
    }
}
