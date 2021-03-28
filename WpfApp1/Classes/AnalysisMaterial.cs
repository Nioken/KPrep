using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{ 
    public class AnalysisMaterial
    {
        [Key]
        public int MaterialID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public virtual ICollection<Result> materialID { get; set; }
    }
}
