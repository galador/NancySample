using System.Data.Entity;
using System.Diagnostics;
using MySql.Data.Entity;

namespace NancySample.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AppDbContext : DbContext, IAppDbContext
    {
        static AppDbContext()
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        //Constructor
        public AppDbContext() : base("name=database")
        {
            Database.Log = (l => Debug.WriteLine(l));
        }

        //DbSets
        public virtual IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) { }
    }
}