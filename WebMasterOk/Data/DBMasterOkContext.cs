using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Data
{
    public class DBMasterOkContext : DbContext
    {
        public DBMasterOkContext(DbContextOptions<DBMasterOkContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCheck> ProductChecks { get; set; }

        public DbSet<ProductSold> ProductSolds { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "123@gmail.com";
            string adminPassword = "123456";

            string userEmail = "1234@gmail.com";
            string userPassword = "123456";

            Role adminRole = new Role { Id = 1, TitleRole = adminRoleName, DescriptionRole = adminRoleName };
            Role userRole = new Role { Id = 2, TitleRole = userRoleName, DescriptionRole = userRoleName };

            Position positionAdmin = new Position { Id = 1, TitlePosition = "admin" };
            Position positionUser = new Position { Id = 2, TitlePosition = "user" };

            Staff staffAdmin = new Staff { Id = 1, PositionId = positionAdmin.Id };
            Staff staffUser = new Staff { Id = 2, PositionId = positionUser.Id };

            User adminUser = new User { Id = 1, LoginUser = adminEmail, PasswordUser = adminPassword, RoleId = adminRole.Id, StaffId = staffAdmin.Id };
            User userUser = new User { Id = 2, LoginUser = userEmail, PasswordUser = userPassword, RoleId = userRole.Id, StaffId = staffUser.Id };

            modelBuilder.Entity<Position>().HasData(new Position[] { positionAdmin, positionUser });
            modelBuilder.Entity<Staff>().HasData(new Staff[] { staffAdmin, staffUser });
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });

            base.OnModelCreating(modelBuilder);
        }
    }
}
