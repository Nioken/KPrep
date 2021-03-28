using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DateTime WorkDate { get; set; }
        [Required]
        public DateTime WorkTime { get; set; }
        [Required]
        public virtual ICollection<AnalysisMaterial> ticketID { get; set; }

    }
}
