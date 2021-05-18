using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Worker
    {
        [Key]
        public int WorkerID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public int Expirience { get; set; }
        [Required]
        public string Specialize { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        //public int AccessILevelD { get; set; }
        [JsonIgnore]
        public virtual ICollection<PaidServices> workerID { get; set; }
        [JsonIgnore]
        [Required]
        public virtual AccessLevel AccessLevel { get; set; }
    }
}
