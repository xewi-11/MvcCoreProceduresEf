using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEf.Models;
using MvcCoreProceduresEf.Repositories;

namespace MvcCoreProceduresEf.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctor repo;
        public DoctoresController(RepositoryDoctor repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<string> especialidades = await this.repo.GetEspecialidadAsync();
            return View(especialidades);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Datosupdate datos)
        {
            List<string> especialidades = await this.repo.GetEspecialidadAsync();
            if (datos.updateOp.Equals("0"))
            {
                await this.repo.UpdateDoctorRaw(datos.especialidad, datos.incremento);
                List<Doctor> doctores = await this.repo.GetAllDctoresEspecialidad(datos.especialidad);
                ViewData["DOCTORES"] = doctores;
                ViewData["Metodo"] = "procedure";

            }
            else if (datos.updateOp == "1")
            {
                await this.repo.UpdateDoctorLinq(datos.especialidad, datos.incremento);
                List<Doctor> doctores = await this.repo.GetAllDctoresEspecialidad(datos.especialidad);
                ViewData["DOCTORES"] = doctores;
                ViewData["Metodo"] = "Linq";

            }
            else
            {
                List<Doctor> doctores = await this.repo.GetAllDctoresEspecialidad(datos.especialidad);
                ViewData["DOCTORES"] = doctores;
            }

            return View(especialidades);
        }

    }
}
