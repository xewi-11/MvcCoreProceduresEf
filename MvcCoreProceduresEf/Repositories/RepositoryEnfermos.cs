using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEf.Data;
using MvcCoreProceduresEf.Models;
using System.Data;
using System.Data.Common;

namespace MvcCoreProceduresEf.Repositories
{

    #region STORED PROCEDURES

    //    create procedure SP_ALL_ENFERMOS
    //as
    //    select* from ENFERMO
    //go

    //create procedure SP_FIND_ENFERMO
    //(@inscripcion nvarchar(50))
    //as
    //    select* from ENFERMO where INSCRIPCION = @inscripcion
    //go

    //create procedure SP_DELETE_ENFERMO
    //(@inscripcion nvarchar(50))
    //as
    //    delete from ENFERMO where INSCRIPCION = @inscripcion
    //go

    #endregion
    public class RepositoryEnfermos
    {
        private EnfermosContext context;
        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context = context;
        }



        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            //NECESITAMOS UN DBCOMMAND,vamosa utilizxar un using para todo
            //EL COMMAND, EN SU CREACION NECESITA UNA CADENA DE CONEXION
            // EL OBEJTO CONNECTION NOS OFRECE EF
            //LAS CONEXIONES SE CREAN A PARTIR DE CONTEXT

            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ENFERMOS";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                //abrimos la onexion a partir del command
                await com.Connection.OpenAsync();
                //ejecutamos el command y obtenemos un datareader
                DbDataReader reader = await com.ExecuteReaderAsync();
                //DEBEMOS MAPEAR LOS METODOS MANUALMENTE
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo();
                    enfermo.Inscripcion = reader["INSCRIPCION"].ToString();
                    enfermo.Apellido = reader["APELLIDO"].ToString();
                    enfermo.Direccion = reader["DIRECCION"].ToString();
                    enfermo.FechaNacimiento = DateTime.Parse(reader["FECHA_NAC"].ToString());
                    enfermo.Genero = reader["S"].ToString();
                    enfermo.Nss = reader["NSS"].ToString();
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }
    }
}

