using EmployeApp.Models;
using EmployeApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeApp.Controllers
{
    public class TrabajadorController : Controller
    {
        private readonly TrabajadorContext _context;

        public TrabajadorController(TrabajadorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sexo)
        {
            var trabajadoresQuery = _context.Trabajadores
                .Include(t => t.IdDepartamentoNavigation)
                .Include(t => t.IdProvinciaNavigation)
                .Include(t => t.IdDistritoNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(sexo))
            {
                trabajadoresQuery = trabajadoresQuery.Where(t => t.Sexo == sexo);
            }

            var viewModel = new TrabajadorViewModel
            {
                Trabajador = new Trabajadore(),
                Departamentos = await _context.Departamentos.ToListAsync(),
                Provincias = await _context.Provincia.ToListAsync(),
                Distritos = await _context.Distritos.ToListAsync(),
                Trabajadores = await trabajadoresQuery.ToListAsync()
            };

            ViewBag.FiltroSexo = sexo;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TrabajadorViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.Trabajadores.Add(vm.Trabajador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            vm.Departamentos = await _context.Departamentos.ToListAsync();
            vm.Provincias = await _context.Provincia.ToListAsync();
            vm.Distritos = await _context.Distritos.ToListAsync();
            vm.Trabajadores = await _context.Trabajadores
                .Include(t => t.IdDepartamentoNavigation)
                .Include(t => t.IdProvinciaNavigation)
                .Include(t => t.IdDistritoNavigation)
                .ToListAsync();

            ViewBag.AbrirModal = true;
            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Trabajadore t)
        {
            if (ModelState.IsValid)
            {
                _context.Trabajadores.Update(t);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador != null)
            {
                _context.Trabajadores.Remove(trabajador);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProvincias(int idDepartamento)
        {
            var provincias = await _context.Provincia
                .Where(p => p.IdDepartamento == idDepartamento)
                .Select(p => new { p.Id, p.NombreProvincia })
                .ToListAsync();

            return Json(provincias);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDistritos(int idProvincia)
        {
            var distritos = await _context.Distritos
                .Where(d => d.IdProvincia == idProvincia)
                .Select(d => new { d.Id, d.NombreDistrito })
                .ToListAsync();

            return Json(distritos);
        }

    }
}
