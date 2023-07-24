using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IRepository<Cliente> _clienteRepository;

        public ClientesController(IRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [Authorize(Roles ="ADMIN")]
        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            if(clientes == null)
            {
                return NotFound();
            }
            return View(clientes);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }
        [Authorize]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,NumeroCel")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteRepository.AddAsync(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,NumeroCel")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _clienteRepository.Update(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {                  
                   return NotFound();                  
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {          
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente != null)
            {
                await _clienteRepository.Delete(cliente);
            }           
           
            return RedirectToAction(nameof(Index));
        }       
    }
}
