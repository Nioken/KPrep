using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        [Required]
        public int TicketNumber { get; set; }
        [Required]
        public string WorkDate { get; set; }
        [Required]
        public string WorkTime { get; set; }
        public virtual Client Client { get; set; }
        public virtual PaidServices PaidServices { get; set; }
        public virtual ICollection<Result> Results { get; set; }

    }
}
