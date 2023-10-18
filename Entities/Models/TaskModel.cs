using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("TB_Task")]
    public class TaskModel
    {
        [Column("Task_ID")]
        public int Id { get; set; }

        [Column("Task_TITULO")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("Task_INFORMACAO")]
        [MaxLength(255)]
        public string Information { get; set; }

        [Column("Task_ATIVO")]
        public bool Active { get; set; }

        [Column("Task_DATA_CADASTRO")]
        public DateTime RegistrationDate { get; set; }

        [Column("Task_DATA_ALTERACAO")]
        public DateTime ChangeDate { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}