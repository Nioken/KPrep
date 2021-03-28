using System.Data.Entity;
using WpfApp1.Classes;

namespace WFAEntity.API
{
    class MyDBContext : DbContext
    {
        public MyDBContext() : base("LocalString")
        {

        }
        public DbSet<AccessLevel> AccessLevels { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<PaidServices> PaidServices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Result> Results { get; set; }

        public static WFAEntity.API.MyDBContext DBContext = new WFAEntity.API.MyDBContext();
    }
}
