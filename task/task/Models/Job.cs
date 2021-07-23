using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace task.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [StringLength(30, ErrorMessage = "No debe de tener mas de 30 caracteres...")]
        [Display(Name = "Descripcion de la Tarea")]
        [MinLength(3, ErrorMessage = "Debe de tener mas de tres caracteres...")]
        public string JobDescription { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime JobStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Final")]
        public DateTime JobFinalDate { get; set; }
        //modificacion 1:51 am
        [Display(Name = "Descripcion del estado")]
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
        //nuevo codigo
        //public IEnumerable<State> States { get; set; }
        public IEnumerable<SubTask> SubTasks { get; set; }
    }
}
