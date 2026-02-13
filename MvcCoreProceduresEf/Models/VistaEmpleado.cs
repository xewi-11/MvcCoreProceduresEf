using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreProceduresEf.Models
{
    [Table("V_EMPLEADOS_DEPARTAMENTOS")]
    public class VistaEmpleado
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("APELLIDO")]
        public string APELLIDO { get; set; }
        [Column("OFICIO")]
        public string OFICIO { get; set; }
        [Column("SALARIO")]
        public int SALARIO { get; set; }

        [Column("DEPARTAMENTO")]
        public string DNOMBRE { get; set; }
        [Column("LOCALIDAD")]
        public string LOCALIDAD { get; set; }


    }
}
