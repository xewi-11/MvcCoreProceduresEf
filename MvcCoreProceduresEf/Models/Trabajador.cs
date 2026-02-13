using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreProceduresEf.Models
{
    [Table("V_TRABAJADORES")]
    public class Trabajador
    {
        [Key]
        [Column("IDTRABAJADOR")]
        public int IDTRABAJADOR { get; set; }
        [Column("APELLIDO")]
        public string APELLIDO { get; set; }
        [Column("OFICIO")]
        public string OFICIO { get; set; }
        [Column("SALARIO")]
        public int SALARIO { get; set; }
    }
}
