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
    }
}
