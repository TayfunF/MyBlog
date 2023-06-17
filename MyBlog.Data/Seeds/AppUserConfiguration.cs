using Microsoft.AspNetCore.Identity;
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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Indexes for "normalized" username and email, to allow efficient lookups
            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            // Maps to the AspNetUsers table
            builder.ToTable("AspNetUsers");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each User can have many UserClaims
            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            // Each User can have many UserLogins
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            // Each User can have many UserTokens
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            // Each User can have many entries in the UserRole join table
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();



            //Super admin kullanıcısını oluşturma nesnem
            AppUser superadminUser = new AppUser
            {
                Id = Guid.Parse("2C34DA79-F839-4AA8-95DE-1D31A3B39C28"),
                UserName = "superadmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                PhoneNumber = "+905556667788",
                PhoneNumberConfirmed = true,
                FirstName = "Tayfun",
                LastName = "Fırtına",
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("F71F4B9A-AA60-461D-B398-DE31001BF214")
            };

            superadminUser.PasswordHash = MyCustomCreatePasswordHasher(superadminUser, "123456");

            //Admin kullanıcısı oluşturma nesnem
            AppUser adminUser = new AppUser
            {
                Id = Guid.Parse("0B48803D-A991-48E2-A19E-D6CA562F1D96"),
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                EmailConfirmed = false,
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PhoneNumber = "+905556678899",
                PhoneNumberConfirmed = false,
                FirstName = "Admin",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString(),
                ImageId = Guid.Parse("D16A6EC7-8C50-4AB0-89A5-02B9A551F0FA")
            };

            adminUser.PasswordHash = MyCustomCreatePasswordHasher(adminUser, "123456");

            builder.HasData(superadminUser, adminUser);
        }

        //Şifre Oluşturmak için passwordHasher methodum
        private string MyCustomCreatePasswordHasher(AppUser user, string password)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
