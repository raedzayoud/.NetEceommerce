using EcommerceWebMvc.Models;
using EcommerceWebMvc.services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceWebMvc.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext Context { get; }

        public HomeController(ApplicationDbContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            var products =Context.Products.OrderBy(p=>p.Id).Take(6).ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
