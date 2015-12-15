using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Simple.OData.Client;
using ProductManager.MVC.Models;
using ProductManager.Entities;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        //Adicionado cliente OData para criar registo na tabela de empregados em simultaneo com o utilizador a aplicação
        private readonly ODataClient _client;

        public UsersAdminController()
        {
            _client = ProductManagerODataClient.GetDefaultClient();
        }

        public UsersAdminController(string address)
        {
            _client = ProductManagerODataClient.GetCustomClient(address);
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email, FullName = String.Format("{0} {1}", userViewModel.FirstName, userViewModel.LastName) };
                

                //Registo de empregado na base de dados via webservice OData
                var employee = await _client
                        .For<Employee>()
                        .Set(new
                        {
                            Title = userViewModel.Title,
                            FirstName = userViewModel.FirstName,
                            MiddleName = userViewModel.MiddleName,
                            LastName = userViewModel.LastName,
                            Email = userViewModel.Email
                        })
                        .InsertEntryAsync();

                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);


                //Add User to the selected Roles 
                if (adminresult.Succeeded && employee.EmployeeId > 0 && employee.Email == userViewModel.Email)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            //Obter dados do Empregados para edição do utilizador
            var employee = await _client
                .For<Employee>()
                .Filter(e => e.Email == user.Email)
                .FindEntryAsync();

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                }),
                Title = employee.Title,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Rowguid = employee.Rowguid
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,Title,FirstName,MiddleName,LastName,RowGuid")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                //Alterar dados Empregado via Web Service
                string oldEmail = user.Email;
                var employee = await _client
                    .For<Employee>()
                    .Filter(e => e.Email == oldEmail)
                    .Set(new
                    {
                        Title = editUser.Title,
                        FirstName = editUser.FirstName,
                        MiddleName = editUser.MiddleName,
                        LastName = editUser.LastName,
                        Email = editUser.Email,
                        Rowguid = editUser.Rowguid
                    })
                    .UpdateEntryAsync();


                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.FullName = String.Format("{0} {1}", editUser.FirstName, editUser.LastName);

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                //Eliminar Empregado via Web Service
                //TODO: Limpar empregado da tabela de Produtos
                await _client
                    .For<Employee>()
                    .Filter(e => e.Email == user.Email)
                    .DeleteEntryAsync();

                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
