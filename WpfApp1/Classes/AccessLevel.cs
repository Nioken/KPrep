using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class AccessLevel
    {
        [Key]
        public int AccessLevelID { get; set; }
        [Required]
        public int Level { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
