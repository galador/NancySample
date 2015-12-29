using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace NancySample.Models
{
    /*
     * This is the class used by Nancy to map the identity
     * stored in the session cookie to a "real" user.
     */
    public class AuthenticationUser : IUserMapper
    {
        private readonly IAppDbContext _db;

        public AuthenticationUser(IAppDbContext context)
        {
            _db = context;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _db.Users.First(x => x.Uuid == identifier);
            return user;
        }
    }


    /*
     * This is the Entity Framework user class
     */
    [Table("users")]
    public class User : IUserIdentity
    {
        /*
         * IUserIdentify Fields
         * UserName mapped to actual DB column
         */
        //[NotMapped]
        //public string UserName => Username;
        [NotMapped]
        public IEnumerable<string> Claims => new List<string>();


        /*
         * Entity Framework fields
         */
        public long Id { get; set; }

        [MinLength(4)]
        [MaxLength(16)]
        [Required]
        public string UserName { get; set; }

        public Guid Uuid { get; set; }
        public string PassHash { get; set; }

        [Column("secret")]
        public string PassSalt { get; set; }

        [StringLength(80)]
        public string Email { get; set; }
    }
}