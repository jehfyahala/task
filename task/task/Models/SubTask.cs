using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace task.Models
{
    public class SubTask
    {
        [Key]
        public int SubTaskId { get; set; }

        [StringLength(30, ErrorMessage = "No debe de tener mas de 30 caracteres...")]
        [Display(Name = "Descripcion de la subTarea")]
        [MinLength(3, ErrorMessage = "Debe de tener mas de tres caracteres...")]
        public string SubTaskDescription { get; set; }
        [Display(Name = "Tarea")]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }
        //public State State { get; set; }
    }
}
