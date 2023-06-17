using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Seeds
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(
                new AppUserRole
                {
                    UserId = Guid.Parse("2C34DA79-F839-4AA8-95DE-1D31A3B39C28"),
                    RoleId = Guid.Parse("DFB45ACE-1801-46B3-917B-EEAA7B1539B6")
                },
                new AppUserRole
                {
                    UserId = Guid.Parse("0B48803D-A991-48E2-A19E-D6CA562F1D96"),
                    RoleId = Guid.Parse("95FD6878-A514-4C01-8FB8-EB665E512D3F")
                });
        }
    }
}
