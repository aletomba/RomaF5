using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5.Models.Dtos;

namespace RomaF5.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager; 
        }

        // GET: UsuariosController
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<CrearUsuarioDto>();

            foreach (var user in users)
            {
                var userViewModel = new CrearUsuarioDto
                {
                    Id = user.Id,
                    Email = user.UserName,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };

                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }


        // GET: UsuariosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuariosController/Create
        public ActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = roles;
            return View();
        }

        // POST: UsuariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create(CrearUsuarioDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = roles;
            return View(model);
        }

        // GET: UsuariosController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);

            var model = new CrearUsuarioDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };

            return View(model);
        }

        // POST: UsuariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]         
        public async Task<IActionResult> Edit(string id, CrearUsuarioDto model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Actualizar los datos del usuario
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    // Actualizar el rol del usuario
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Cambiar la contraseña si se proporcionó una nueva
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, model.Password);
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    // Si la actualización falla, agregar los errores de validación al ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception)
                {
                    // Manejar cualquier excepción que pueda ocurrir durante la actualización
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el usuario.");
                }
            }

            return View(model);
        }      

        // GET: UsuariosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuariosController/Delete/5
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
