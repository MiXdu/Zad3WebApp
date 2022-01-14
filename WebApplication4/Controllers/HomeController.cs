using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Database;
using WebApplication4.Database.Entities;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult List()
        {
            IEnumerable<PersonEntity> people = _dbContext.People.AsEnumerable();

            return View(people);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonEntity model)
        {

            await _dbContext.People.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Read(int id)
        {
            return View(await _dbContext.People.FindAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return View(await _dbContext.People.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonEntity model)
        {
            _dbContext.People.Update(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _dbContext.People.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PersonEntity model)
        {
            _dbContext.People.Remove(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
