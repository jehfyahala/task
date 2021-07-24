using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace task.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [StringLength(70, ErrorMessage = "No debe de tener mas de 70 caracteres...")]
        [Display(Name = "Descripcion de Estado")]
        [MinLength(3, ErrorMessage = "Debe de tener mas de tres caracteres...")]
        public string StateDescription { get; set; }

        //nueva mod hay varios elementos en el plural
        //de Un estado a muchas tareas
        public IEnumerable<Job> Jobs { get; set; }
    }
}
