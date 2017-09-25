using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCIdentityApp.Models;
using MVCIdentityApp.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace MVCIdentityApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;

        public RoleController()
        {
            _context = new ApplicationDbContext();
        }

        //public RoleController(ApplicationRoleManager roleManager, ApplicationUserManager userManager)
        //{
        //    RoleManager = _roleManager;
        //    UserManager = userManager;
        //}

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

        // GET: Role
        //[Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                _context.Roles.Add(role);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            var members = new List<ApplicationUser>();
            var nonMember = new List<ApplicationUser>();

            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                var list = await UserManager.IsInRoleAsync(user.Id, role.Name)
                    ? members
                    : nonMember;
                list.Add(user);
            }

            return View(new EditRole
            {
                Role = role,
                Members = members,
                NonMembers = nonMember
            });

        }

        // POST: Role/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ModifyRole userRole)
        {
            IdentityResult result;
            try
            {
                //IdentityRole role = await RoleManager.FindByIdAsync(userRole.RoleId);
                //role.Name = userRole.RoleName;
                //IdentityResult result = await RoleManager.UpdateAsync(role);


                foreach (string userId in userRole.IdsToAdd ?? new string[]{})
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await UserManager.AddToRoleAsync(user.Id, userRole.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
