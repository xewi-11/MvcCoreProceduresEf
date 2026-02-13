using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEf.Data;
using MvcCoreProceduresEf.Models;

namespace MvcCoreProceduresEf.Repositories
{

    #region VISTA EMPLEADOS

    //    create view V_EMPLEADOS_DEPARTAMENTOS
    //as
    //   select Cast(isnull(ROW_NUMBER() over (order by EMP.APELLIDO),0)as int) as ID ,
    //   EMP.APELLIDO,EMP.OFICIO,EMP.SALARIO,
    //   DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
    //   from EMP
    //   inner join DEPT on EMP.DEPT_NO=DEPT.DEPT_NO
    //go

    #endregion
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<VistaEmpleado>> GetEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistaEmpleados
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<VistaEmpleado> GetDetailsAsync(int id)
        {
            var consulta = from datos in this.context.VistaEmpleados
                           where datos.ID == id
                           select datos;
            return await consulta.FirstAsync();
        }
    }
}
