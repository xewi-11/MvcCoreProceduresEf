using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEf.Models;
using MvcCoreProceduresEf.Repositories;

namespace MvcCoreProceduresEf.Controllers
{
    public class TrabajadoresController : Controller
    {
        private RepositoryTrabjadores repo;
        public TrabajadoresController(RepositoryTrabjadores repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<string> oficios = await this.repo.GetOficiosAsync();
            TrabajadoresModel model = await this.repo.GetTrabajadoresAsync();
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            List<string> oficios = await this.repo.GetOficiosAsync();
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelPorOficioASync(oficio);
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
    }
}
