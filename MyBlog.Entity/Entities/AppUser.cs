using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("D16A6EC7-8C50-4AB0-89A5-02B9A551F0FA");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
