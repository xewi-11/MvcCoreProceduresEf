using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEf.Models;
using MvcCoreProceduresEf.Repositories;

namespace MvcCoreProceduresEf.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repository;

        public EmpleadosController(RepositoryEmpleados repository)
        {
            this.repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            List<VistaEmpleado> empleados = await this.repository.GetEmpleadosAsync();
            return View(empleados);
        }
    }
}
