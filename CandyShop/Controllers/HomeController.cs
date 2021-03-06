﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CandyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CandyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShopContext db;
        public HomeController(ShopContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var model = db.Cakes.ToList();
            return View(model);
        }
        public IActionResult Goods()
        {
            return View(db.Cakes.ToList());
        }

        //[Authorize]
        [HttpGet]
        public IActionResult AddCake()
        {
            var user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user.Role == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult AddCake(string name, string shortDesc, string longDesc, IFormFile resource, int price)
        {
            string fileName = resource.FileName;
            resource.CopyTo(new FileStream("wwwroot/Resources/" + fileName, FileMode.Create));
            db.Cakes.Add(new Cake { Name = name, ShortDesc = shortDesc, LongDesc = longDesc, Image = fileName, Price = price });
            db.SaveChanges();
            return RedirectToAction("Goods", "Home");
        }
        [Authorize]
        public IActionResult Delete(int id, string act)
        {
            //Удаление изображения
            string fileName = db.Cakes.Single(u => u.Id == id).Image;
            if (System.IO.File.Exists("wwwroot/Resources/" + fileName))
            {
                System.IO.File.Delete("wwwroot/Resources/" + fileName);
            }
            //Удаление из базы данных
            db.Cakes.Remove(db.Cakes.Single(u => u.Id == id));
            db.SaveChanges();
            return RedirectToAction(act, "Home");
        }

        public IActionResult Cake(int id)
        {
            ViewBag.Id = id;
            return View(db.Cakes.ToList());
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
