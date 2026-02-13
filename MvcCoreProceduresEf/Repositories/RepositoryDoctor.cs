using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEf.Data;
using MvcCoreProceduresEf.Models;
using System.Data;
using System.Data.Common;

namespace MvcCoreProceduresEf.Repositories
{
    #region STORED PROCEDURES
    // create procedure SP_ALL_ESPECIALIDADES
    // as 
    // select distinct(ESPECIALIDAD)from DOCTOR
    // go

    // create procedure SP_UPDATE_DOCTOR
    // (@especialidad nvarchar(50), @salario int)
    // as

    //     update DOCTOR set SALARIO = @salario where ESPECIALIDAD = @especialidad

    // go

    // create procedure SP_MOSTRAR_DOCTOR_ESPECIALIDAD
    // (@especialidad nvarchar(50))
    //as
    // select* from DOCTOR where ESPECIALIDAD=@especialidad
    //go
    #endregion
    public class RepositoryDoctor
    {
        private HospitalContext context;

        public RepositoryDoctor(HospitalContext context)
        {
            this.context = context;

        }
        public async Task<List<string>> GetEspecialidadAsync()
        {
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ESPECIALIDADES";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    string especialidad = reader["ESPECIALIDAD"].ToString();
                    especialidades.Add(especialidad);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }
        public async Task UpdateDoctorRaw(string especialidad, int salario)
        {
            string sql = "SP_UPDATE_DOCTOR @especialidad, @salario";

            SqlParameter pamEsp = new SqlParameter("@especialidad", especialidad);
            SqlParameter pamSal = new SqlParameter("@salario", salario);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamEsp, pamSal);
        }
        public async Task<List<Doctor>> GetAllDctoresEspecialidad(string especialidad)
        {
            string sql = "SP_MOSTRAR_DOCTOR_ESPECIALIDAD @especialidad";
            SqlParameter pamEsp = new SqlParameter("@especialidad", especialidad);
            List<Doctor> doctores = await this.context.Doctores.FromSqlRaw(sql, pamEsp).ToListAsync();
            return doctores;
        }
        public async Task UpdateDoctorLinq(string especialidad, int incremento)
        {
            var consulta = from datos in this.context.Doctores
                           where datos.ESPECIALIDAD == especialidad
                           select datos;
            List<Doctor> doctores = await consulta.ToListAsync();
            // List<Doctor> doctores = await this.GetAllDctoresEspecialidad(especialidad);
            if (doctores != null)
            {
                foreach (Doctor doc in doctores)
                {
                    doc.SALARIO += incremento;
                    await this.context.SaveChangesAsync();
                }
            }
        }
    }
}
