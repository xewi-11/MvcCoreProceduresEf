using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEf.Models;
using MvcCoreProceduresEf.Repositories;

namespace MvcCoreProceduresEf.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;
        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermosAsync();
            return View(enfermos);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Enfermo enfermo)
        {
            await this.repo.InsertEnfermoRaw(enfermo.Apellido, enfermo.Direccion, enfermo.FechaNacimiento, enfermo.Genero, enfermo.Nss);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string inscripcion)
        {
            Enfermo enfermo = await this.repo.FindEnfermoAsync(inscripcion);
            return View(enfermo);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Enfermo enfermo)
        {
            await this.repo.UpdateEnfermoRaw(enfermo.Inscripcion, enfermo.Apellido, enfermo.Direccion, enfermo.FechaNacimiento, enfermo.Genero, enfermo.Nss);
            return View(enfermo);
        }
        public async Task<IActionResult> Details(string inscripcion)
        {
            Enfermo enfermo = await this.repo.FindEnfermoAsync(inscripcion);
            return View(enfermo);
        }
        public async Task<IActionResult> Delete(string inscripcion)
        {
            await this.repo.DeleteEnfermoAsync(inscripcion);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteRaw(string inscripcion)
        {
            await this.repo.DeleteEnfermoRawAsync(inscripcion);
            return RedirectToAction("Index");
        }
    }
}
