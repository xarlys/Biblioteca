#nullable disable

using Biblioteca.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly BibliotecaContext _context;

        public AccountController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: AccountController
        public async Task<IActionResult> Index(string procurar, string FiltroAtual, int? page)
        {
            if (procurar != null)
                page = 1;
            else
                procurar = FiltroAtual;

            ViewBag.FiltroAtual = procurar;

            var listUser = await (_context.Users).ToListAsync();

            if (!String.IsNullOrEmpty(procurar))
                listUser = listUser.Where(s => s.FirstName.Contains(procurar)).ToList();

            return View(listUser);
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
