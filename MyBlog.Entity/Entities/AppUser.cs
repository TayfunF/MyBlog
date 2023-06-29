using Microsoft.AspNetCore.Identity;
using MyBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("F9660BDF-1BD1-47EE-7957-08DB62DD9E9E");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
