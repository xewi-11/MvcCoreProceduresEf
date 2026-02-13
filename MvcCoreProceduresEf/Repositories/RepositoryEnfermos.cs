using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEf.Data;
using MvcCoreProceduresEf.Models;
using System.Data;
using System.Data.Common;

namespace MvcCoreProceduresEf.Repositories
{

    #region STORED PROCEDURES

    //create procedure SP_ALL_ENFERMOS
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

    //create procedure SP_INSERT_ENFERMO
    //(@apellido nvarchar(50),@direccion nvarchar(50),@fechanac datetime, @sexo nvarchar(50),@nss nvarchar(50))
    //as
    //   declare @inscripcion nvarchar(50)
    //   select @inscripcion= (select CAST(MAX(INSCRIPCION) as INT)from ENFERMO )+1;

    //   insert into ENFERMO values(@inscripcion, @apellido, @direccion, @fechanac, @sexo, @nss)
    //go

    //create procedure SP_UPDATE_ENFERMO
    //(@inscripcion nvarchar(50),@apellido nvarchar(50),@direccion nvarchar(50),@fechanac datetime, @sexo nvarchar(50),@nss nvarchar(50))
    //as
    //   update ENFERMO set APELLIDO = @apellido, DIRECCION = @direccion, FECHA_NAC = @fechanac, S = @sexo, NSS = @nss
    //       where INSCRIPCION = @inscripcion
    //go

    #endregion
    public class RepositoryEnfermos
    {
        private HospitalContext context;
        public RepositoryEnfermos(HospitalContext context)
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

        public async Task<Enfermo> FindEnfermoAsync(string inscripcion)
        {
            //PARA LLAMAR A UN PROCEDIMIENTO QUE CONTIENE PARAMETROS
            //LA LLAMADA SE REALIZA MEDIANTE EL NOMBRE DEL PROCEDURE
            //Y CADA PARAMETRO A CONTINUACION EN LA DECLARACION
            //DEL SQL: SP_PROCEDURE @PAM1, @PAM2
            string sql = "SP_FIND_ENFERMO @inscripcion";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            //SI LOS DATOS QUE DEVUELVE EL PROCEDURE ESTAN MAPEADOS
            //CON UN MODEL, PODEMOS UTILIZAR EL METODO
            //FromSqlRaw PARA RECUPERAR DIRECTAMENTE EL MODEL/S.
            //NO PODEMOS CONSULTAR Y EXTRAER A LA VEX CON LINQ, SE DEBE
            //REALIZAR SIEMPRE EN DOS PASOS
            var consulta = this.context.Enfermos.FromSqlRaw(sql, pamIns);
            Enfermo enfermo = await consulta.ToAsyncEnumerable().FirstOrDefaultAsync();
            return enfermo;
        }

        public async Task InsertEnfermoASync(string apellido, string direccion, DateTime fechanac, string sexo, string nss)
        {


            string sql = "SP_INSERT_ENFERMO";
            SqlParameter pamApel = new SqlParameter("@apellido", apellido);
            SqlParameter pamDir = new SqlParameter("@direccion", direccion);
            SqlParameter pamFec = new SqlParameter("@fechanac", fechanac);
            SqlParameter pamSex = new SqlParameter("@sexo", sexo);
            SqlParameter pamNss = new SqlParameter("@nss", nss);

            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamApel);
                com.Parameters.Add(pamDir);
                com.Parameters.Add(pamFec);
                com.Parameters.Add(pamSex);
                com.Parameters.Add(pamNss);
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }
        public async Task DeleteEnfermoAsync(string inscripcion)
        {

            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamIns);
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();

            }


        }
        public async Task DeleteEnfermoRawAsync(string inscripcion)
        {

            string sql = "SP_DELETE_ENFERMO @inscripcion";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamIns);
        }
        public async Task InsertEnfermoRaw(string apellido, string direccion, DateTime fechanac, string sexo, string nss)
        {
            string sql = "SP_INSERT_ENFERMO @apellido, @direccion, @fechanac, @sexo, @nss";
            SqlParameter pamApel = new SqlParameter("@apellido", apellido);
            SqlParameter pamDir = new SqlParameter("@direccion", direccion);
            SqlParameter pamFec = new SqlParameter("@fechanac", fechanac);
            SqlParameter pamSex = new SqlParameter("@sexo", sexo);
            SqlParameter pamNss = new SqlParameter("@nss", nss);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApel, pamDir, pamFec, pamSex, pamNss);
        }
        public async Task UpdateEnfermoRaw(string inscripcion, string apellido, string direccion, DateTime fechanac, string sexo, string nss)
        {
            string sql = "SP_UPDATE_ENFERMO @inscripcion, @apellido, @direccion, @fechanac, @sexo, @nss";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            SqlParameter pamApel = new SqlParameter("@apellido", apellido);
            SqlParameter pamDir = new SqlParameter("@direccion", direccion);
            SqlParameter pamFec = new SqlParameter("@fechanac", fechanac);
            SqlParameter pamSex = new SqlParameter("@sexo", sexo);
            SqlParameter pamNss = new SqlParameter("@nss", nss);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamIns, pamApel, pamDir, pamFec, pamSex, pamNss);
        }
    }
}

