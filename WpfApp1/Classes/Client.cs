using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Gender { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
