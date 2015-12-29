using System.Data.Entity;

namespace NancySample.Models
{
    public interface IAppDbContext
    {
        //Db Sets (tables)
        IDbSet<User> Users { get; set; }
        
        //So we can save changes
        int SaveChanges();
    }
}