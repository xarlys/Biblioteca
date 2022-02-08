using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authorization;
using Biblioteca.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Biblioteca.Areas.Identity.Data.VirtualModelReservaLivro;
using System.Linq;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class BookController : Controller
    {

        private readonly BibliotecaContext appLivro;


        private readonly UserManager<BibliotecaUser> _userInManager;

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public int LivroID { get; set; }

        }
        [BindProperty] public InputModel Input { get; set; }

        public BookController(BibliotecaContext context, UserManager<BibliotecaUser> userInManager)
        {
            appLivro = context;
            _userInManager = userInManager;
        }


        public async Task<IActionResult> Index(string order, string procurar, string FiltroAtual, int? page)
        {
            ViewBag.IdParam = order == "Id_Desc" ? "Id" : "Id_Desc";
            ViewBag.NameParam = order == "Name_Desc" ? "Name" : "Name_Desc";
            ViewBag.DescParam = order == "Description_Desc" ? "Description" : "Description_Desc";
            //ViewBag.DGravadoParam = String.IsNullOrEmpty(C) ? "DGravado" : "";
            //ViewBag.InfoLogin = _userInManager.GetUserId(User);


            if (procurar != null)
            {
                page = 1;
            }
            else
            {
                procurar = FiltroAtual;
            }

            ViewBag.FiltroAtual = procurar;

            //var list = (await appLivro.tabreservalivro.OrderBy(x => x.Id).ToListAsync()).AsQueryable();
            //var list = (await appLivro.tablivro.Join(appLivro.tabreservalivro, x => x.Id, x => x.LivroID,
            //    (b, d) => new
            //    {
            //        b.Id,
            //        b.Name,
            //        b.Description,
            //        d.LivroID,
            //        d.UserID
            //    }).ToListAsync());

            var list = await (from e in appLivro.tablivro
                              join d in appLivro.tabreservalivro on e.Id equals d.LivroID
                                   into d
                              from a in d.DefaultIfEmpty()
                              select new
                              {
                                  Id = e.Id,
                                  Name = e.Name,
                                  Description = e.Description,
                                  LivroID = (int?)a.LivroID,
                                  UserID = (string?)a.UserID
                              }).ToListAsync();

            var listLivros = new List<VirtualModelReservaLivro>();
            foreach (var data in list)
            {
                VirtualModelReservaLivro viewReserva = new VirtualModelReservaLivro()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    LivroID = data.LivroID ?? 0,
                    UserID = data.UserID ?? null                    
                };
                listLivros.Add(viewReserva);
            }

            if (!String.IsNullOrEmpty(procurar))
            {
                listLivros = listLivros.Where(s => s.Name.Contains(procurar)).ToList();
            }

            switch (order)
            {
                case "Id":
                    listLivros = listLivros.OrderBy(s => s.Id).ToList();
                    break;
                case "Id_Desc":
                    listLivros = listLivros.OrderByDescending(s => s.Id).ToList();
                    break;
                case "Name":
                    listLivros = listLivros.OrderBy(s => s.Name).ToList();
                    break;
                case "Name_Desc":
                    listLivros = listLivros.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Description":
                    listLivros = listLivros.OrderBy(s => s.Description).ToList();
                    break;
                case "Description_Desc":
                    listLivros = listLivros.OrderByDescending(s => s.Description).ToList();
                    break;
                default:
                    listLivros = listLivros.OrderBy(s => s.Id).ToList();
                    break;
            }
            //int pageSize = 20;
            //int pageNumber = (page ?? 1);
            return View(listLivros);
        }


        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new BibliotecaLivro());
            else
                return View(appLivro.tablivro.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Name,Description")] BibliotecaLivro livro)
        {
            if (ModelState.IsValid)
            {
                var verificacao = appLivro.tablivro.FirstOrDefault(c => c.Name == livro.Name);
                if (verificacao != null) { this.TempData["mensagemerrovirgula"] = "JÁ POSSUI UM LIVOR CADASTRADO COM ESSE NOME"; }
                else
                {
                    if (livro.Id == 0)
                        appLivro.Add(livro);
                    else
                        appLivro.Update(livro);
                    await appLivro.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(livro);

        }

        public async Task<IActionResult> Delete(int? Id)
        {
            var livro = await appLivro.tablivro.FindAsync(Id);
            appLivro.tablivro.Remove(livro);
            await appLivro.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reservar([Bind("Id")] BibliotecaReservaLivro reservaLivro)
        {

            //ViewBag.InfoLogin = _userInManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var reservaBook = new BibliotecaReservaLivro
                {
                    UserID = _userInManager.GetUserId(User),
                    LivroID = reservaLivro.Id // Input.LivroID
                };

                var verificacao = appLivro.tabreservalivro.FirstOrDefault(c => c.LivroID == reservaLivro.Id);
                if (verificacao != null)
                {
                    this.TempData["mensagemerrovirgula"] = "ESSE LIVRO JÁ ESTÁ RESERVADO!";
                    return Redirect("/Book");                    
                }
                else
                {
                    appLivro.Add(reservaBook);
                    await appLivro.SaveChangesAsync();

                    this.TempData["mensagemsucesso"] = "LIVRO RESERVADO COM SUCESSO!";
                    return Redirect("/Book");
                }
            }

            return View(reservaLivro);
        }
      
        private ActionResult HttpNotFound()
        {
            this.TempData["mensagemerro"] = "ERRO AO INSERIR REGISTRO!";
            return RedirectToAction("AcessoNegado", "Home");
        }


    }
}