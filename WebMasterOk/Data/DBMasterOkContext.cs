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

        public DbSet<PayMethod> PayMethods { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        public DbSet<PathImage> PathImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin";
            string adminPassword = "123456";

            string userEmail = "user";
            string userPassword = "123456";

            Role adminRole = new Role { Id = 1, TitleRole = "Администратор", DescriptionRole = adminRoleName };
            Role userRole = new Role { Id = 2, TitleRole = "Пользователь", DescriptionRole = userRoleName };

            Position positionAdmin = new Position { Id = 1, TitlePosition = "admin" };
            Position positionUser = new Position { Id = 2, TitlePosition = "user" };

            Staff staffAdmin = new Staff { Id = 1, PositionId = positionAdmin.Id, Age = 21, FullNameStaff = "Администратор" };
            Staff staffUser = new Staff { Id = 2, PositionId = positionUser.Id, Age = 21, FullNameStaff = "Менеджер" };

            User adminUser = new User { Id = 1, LoginUser = adminEmail, PasswordUser = adminPassword, RoleId = adminRole.Id, StaffId = staffAdmin.Id };
            User userUser = new User { Id = 2, LoginUser = userEmail, PasswordUser = userPassword, RoleId = userRole.Id, StaffId = staffUser.Id };

            Category categoryTest1 = new Category { Id = 1, TitleCategory = "Тестовая категория 1", DescriptionCategory = "1" };
            Category categoryTest2 = new Category { Id = 2, TitleCategory = "Тестовая категория 2", DescriptionCategory = "2" };

            SubCategory subCategoryTest1 = new SubCategory { Id = 1, TitleSubCategory = "Тестовая подкатегория 1", CategoryId = categoryTest1.Id, DescriptionSubCategory = "1" };
            SubCategory subCategoryTest2 = new SubCategory { Id = 2, TitleSubCategory = "Тестовая подкатегория 2", CategoryId = categoryTest2.Id, DescriptionSubCategory = "2" };

            Product productTest1 = new Product { Id = 1, TitleProduct = "Тестовый продукт категории 1", Price = 1230, SubCategoryId = subCategoryTest1.Id, DescriptionProduct = "1" };
            Product productTest2 = new Product { Id = 2, TitleProduct = "Тестовый продукт категории 1", Price = 1200, SubCategoryId = subCategoryTest1.Id, DescriptionProduct = "1" };
            Product productTest3 = new Product { Id = 3, TitleProduct = "Тестовый продукт категории 2", Price = 1200, SubCategoryId = subCategoryTest2.Id, DescriptionProduct = "2" };
            Product productTest4 = new Product { Id = 4, TitleProduct = "Тестовый продукт категории 2", Price = 1200, SubCategoryId = subCategoryTest2.Id, DescriptionProduct = "2" };
            Product productTest5 = new Product { Id = 5, TitleProduct = "Тестовый продукт категории 2", Price = 1200, SubCategoryId = subCategoryTest2.Id, DescriptionProduct = "2" };

            PathImage pathImagesTest1 = new PathImage { Id = 1, NameImage = "esteamerby.png", ProductId = productTest1.Id, CategoryId = categoryTest1.Id, SubCategoryId = subCategoryTest1.Id, Slider = false };
            PathImage pathImagesTest2 = new PathImage { Id = 2, NameImage = "esteamerby.png", ProductId = productTest2.Id, CategoryId = categoryTest2.Id, SubCategoryId = subCategoryTest2.Id, Slider = false };
            PathImage pathImagesTest3 = new PathImage { Id = 3, NameImage = "esteamerby.png", ProductId = productTest3.Id, CategoryId = categoryTest1.Id, Slider = false };
            PathImage pathImagesTest4 = new PathImage { Id = 4, NameImage = "esteamerby.png", ProductId = productTest4.Id, CategoryId = categoryTest2.Id, SubCategoryId = subCategoryTest1.Id, Slider = false };
            PathImage pathImagesTest5 = new PathImage { Id = 5, NameImage = "esteamerby.png", ProductId = productTest5.Id, CategoryId = categoryTest1.Id, Slider = false };

            PathImage sliderTest1 = new PathImage { Id = 6, ProductId = null, NameImage = "1.jpg", Slider = true };
            PathImage sliderTest2 = new PathImage { Id = 7, ProductId = null, NameImage = "2.jpg", Slider = true };

            Client client = new Client { Id = 1, EmailClient = "123@gmail.com", Address = "Гомель", FamClient = "Иванов", FirstNameClient = "Иван", LastNameClient = "Иванович", LoginClient = "ivanov", NumberTelephone = "375 44 333 33 33", PasswordClient = "123" };

            PayMethod payMethodTest1 = new PayMethod { Id = 1, TitlePayMethod = "Наличными" };
            PayMethod payMethodTest2 = new PayMethod { Id = 2, TitlePayMethod = "Кредит или рассрочка" };
            PayMethod payMethodTest3 = new PayMethod { Id = 3, TitlePayMethod = "Терминал" };

            DeliveryMethod deliveryMethodTest1 = new DeliveryMethod { Id = 1, TitleDeliveryMethod = "Самовывоз - бесплатно" };
            DeliveryMethod deliveryMethodTest2 = new DeliveryMethod { Id = 2, TitleDeliveryMethod = "Курьером: по Гомелю" };
            DeliveryMethod deliveryMethodTest3 = new DeliveryMethod { Id = 3, TitleDeliveryMethod = "Курьером по Гомельской области" };

            modelBuilder.Entity<Position>().HasData(new Position[] { positionAdmin, positionUser });
            modelBuilder.Entity<Staff>().HasData(new Staff[] { staffAdmin, staffUser });
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });
            modelBuilder.Entity<Category>().HasData(new Category[] { categoryTest1, categoryTest2 });
            modelBuilder.Entity<SubCategory>().HasData(new SubCategory[] { subCategoryTest1, subCategoryTest2 });
            modelBuilder.Entity<PathImage>().HasData(new PathImage[] { pathImagesTest1, pathImagesTest2, pathImagesTest3, pathImagesTest4, pathImagesTest5, sliderTest1, sliderTest2 });
            modelBuilder.Entity<Product>().HasData(new Product[] { productTest1, productTest2, productTest3, productTest4, productTest5 });
            modelBuilder.Entity<Client>().HasData(new Client[] { client });
            modelBuilder.Entity<PayMethod>().HasData(new PayMethod[] { payMethodTest1, payMethodTest2, payMethodTest3 });
            modelBuilder.Entity<DeliveryMethod>().HasData(new DeliveryMethod[] { deliveryMethodTest1, deliveryMethodTest2, deliveryMethodTest3 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
