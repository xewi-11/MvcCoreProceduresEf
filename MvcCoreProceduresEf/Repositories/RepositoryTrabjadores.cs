using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEf.Data;
using MvcCoreProceduresEf.Models;
using System.Data;

namespace MvcCoreProceduresEf.Repositories
{
    #region VISTAS Y PROCEDURES ALMACENADOS

    //    create view V_TRABAJADORES
    //as
    //   select EMP_NO as IDTRABAJADOR ,APELLIDO,OFICIO as OFICIO, SALARIO from EMP
    //   union
    //   select DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO from DOCTOR
    //   union
    //   select EMPLEADO_NO,APELLIDO,FUNCIOn,SALARIO from PLANTILLA
    //go

    //create procedure ALL_TRABAJADORES_OFICIOS
    //(@oficio nvarchar(50),
    //@personas int out,
    //@media int out,
    //@suma int out)
    //as 

    //   select* from V_TRABAJADORES where OFICIO=@oficio
    //   select @personas= Count(IDTRABAJADOR)
    //    ,@media=AVG(SALARIO)
    //    ,@suma=SUM(SALARIO) from V_TRABAJADORES
    //     where OFICIO = @oficio
    //go



    #endregion
    public class RepositoryTrabjadores
    {

        private HospitalContext context;


        public RepositoryTrabjadores(HospitalContext context)
        {
            this.context = context;

        }
        public async Task<TrabajadoresModel> GetTrabajadoresAsync()
        {
            //PRIMERO CON LINQ
            var consulta = from datos in context.Trabajadores
                           select datos;

            TrabajadoresModel model = new TrabajadoresModel();

            model.Trabajadores = await consulta.ToListAsync();
            model.personas = await consulta.CountAsync();
            model.mediaSalarial = (int)await consulta.AverageAsync(z => z.SALARIO);
            model.sumaSalarial = await consulta.SumAsync(z => z.SALARIO);

            return model;
        }
        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in context.Trabajadores
                            select datos.OFICIO).Distinct();
            return await consulta.ToListAsync();

        }
        public async Task<TrabajadoresModel> GetTrabajadoresModelPorOficioASync(string oficio)
        {
            //PARA LLAMAR AL PROCEDIMIENTO YA QUE TENEMOS MODEL VAMOS A LLAMARLO CON EF ES DECIR CON FROMSQLRAW
            //LA UNICA DIFEENCIA CUANDO TENEMOS PARAMETROS DE SALIDA
            //ES INDICAR LA PALABRA OUT EN LA DECLARACION DE LAS VARIABLES

            string sql = "ALL_TRABAJADORES_OFICIOS @oficio, @personas out," +
                " @media out, @suma out";

            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamPersonas = new SqlParameter("@personas", -1);
            pamPersonas.Direction = ParameterDirection.Output;
            SqlParameter pamMedia = new SqlParameter("@media", -1);
            pamMedia.Direction = ParameterDirection.Output;
            SqlParameter pamSuma = new SqlParameter("@suma", -1);
            pamSuma.Direction = ParameterDirection.Output;

            //EJECUTAMOS LA CONSULTA CON EL MODEL FromSqlRaw

            var consulta = this.context.Trabajadores.FromSqlRaw(sql, pamOficio, pamPersonas, pamMedia, pamSuma);

            TrabajadoresModel model = new TrabajadoresModel();
            model.Trabajadores = await consulta.ToListAsync();
            model.personas = int.Parse(pamPersonas.Value.ToString());
            model.mediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.sumaSalarial = int.Parse(pamSuma.Value.ToString());


            return model;
        }

    }
}
