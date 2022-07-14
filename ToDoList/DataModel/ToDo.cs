using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.DataModel
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }

        public string description { get; set; }

        public DateTime duedate { get; set; }

        public DateTime createdate { get; set; } = DateTime.Now;
        public Todolist todolist { get; set; }

        public ToDoStatuses todostatus { get; set; }


        [Required]
        public int todolistid {get; set;}

        [DisplayName("StatusName")]
        [Required]
        public int todostatusid{ get; set; }

        [NotMapped]
        public List<Todolist> ToDoListCollection { get; set; }

        [NotMapped]
        public List<ToDoStatuses> ToDoStatusCollection { get; set; }
  
    }
}
