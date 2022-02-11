#nullable disable

using Biblioteca.Areas.Identity.Data;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class AccountController : Controller //: PageModel
    {
        private readonly BibliotecaContext _context;
        private readonly UserManager<BibliotecaUser> _userManager;
        private readonly ILogger<BibliotecaUser> _logger;
        private readonly RoleManager<IdentityRole> _rolerManager;

        public string ReturnUrl { get; set; }

        public AccountController(BibliotecaContext context, UserManager<BibliotecaUser> userManager, RoleManager<IdentityRole> rolerManager, ILogger<BibliotecaUser> logger)
        {
            _context = context;
            _userManager = userManager;
            _rolerManager = rolerManager;
            _logger = logger;
        }
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            BibliotecaUser registerViewModel = new BibliotecaUser();
            ViewBag.RoleId = new SelectList(_context.Roles.ToList().OrderBy(x => x.Name), "Name", "Name");
            ViewData["ReturnUrl"] = returnUrl;
            return View(registerViewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(BibliotecaUser model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ViewBag.RoleId = new SelectList(_context.Roles.ToList().OrderBy(x => x.Name), "Name", "Name");

                var user = new BibliotecaUser {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PasswordHash = model.PasswordHash,
                    UserName = model.Email                    
                };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    //-------------------atribuir role ao user------------------------------
                    var applicationRole = await _rolerManager.FindByNameAsync(model.Role);                    
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    }
                    //-------------------atribuir role ao user------------------------------
                    var userId = await _userManager.GetUserIdAsync(user);
                    this.TempData["mensagemsucesso"] = "Usuário Cadastrado com Sucesso!";
                    //return LocalRedirect(returnUrl);
                    return Redirect("/Account");
                }
                //this.TempData["mensagemerrovirgula"] = result;
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AddOrEdit(string id = null)
        {
            ViewBag.RoleId = new SelectList(_context.Roles.ToList().OrderBy(x => x.Name), "Name", "Name");
            if (id == null)
               return View(new BibliotecaUser());
            else
             return View(_context.Users.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(BibliotecaUser input, string returnUrl = null) //[Bind("FirstName,LastName, Email, PasswordHash")]
        {
            returnUrl ??= Url.Content("~/Account");
            if (ModelState.IsValid)
            {
                ViewBag.RoleId = new SelectList(_context.Roles.ToList().OrderBy(x => x.Name), "Name", "Name");
                //var user = new BibliotecaUser
                //{
                //    UserName = Input.UserName,
                //    Email = Input.Email,
                //    FirstName = Input.FirstName,
                //    LastName = Input.LastName
                //};

                var user = new BibliotecaUser
                {
                   
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    PasswordHash = input.PasswordHash,
                    UserName = input.Email
                };


                var verificacao = _userManager.Users.FirstOrDefault(c => c.Email == input.Email);
                if (verificacao != null) { this.TempData["mensagemerrovirgula"] = "JÁ POSSUI UM USUÁRIO COM ESTE E-MAIL"; }
                else
                {                   
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("O usuário criou uma nova conta com senha.");

                        var applicationRole = await _rolerManager.FindByNameAsync(input.Role);
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                        }

                        var userId = await _userManager.GetUserIdAsync(user);
                        this.TempData["mensagemsucesso"] = "Usuário Cadastrado com Sucesso!";
                        return LocalRedirect(returnUrl);

                    }                    
                    //string lastName = usuarios.LastName;
                    //this.TempData["mensagemerrovirgula"] = result; //"ERRO NÃO SEI DE QUE";                    
                }   
               
            }
            return View(input);//return View();

        }

        public async Task<IActionResult> Delete(string? Id)
        {
            var verificacao = _context.tabreservalivro.FirstOrDefault(c => c.UserID == Id);
            if (verificacao != null) { 
                this.TempData["mensagemerrovirgula"] = "Não é possível excluir o usuário, o mesmo possui livros em seu reservados em seu nome!";
            }
            else
            {
                var users = await _context.Users.FindAsync(Id);
                _context.Users.Remove(users);
                await _context.SaveChangesAsync();
                return Redirect("/Account");
                //return RedirectToAction(nameof(Index));
            }

            return Redirect("/Account");//return RedirectToAction(nameof(Index));

        }
    }
}
