using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class PaidServices
    {
        [Key]
        public int ServiceID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
