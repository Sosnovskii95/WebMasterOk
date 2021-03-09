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

            Category izmer = new Category { Id = 1, TitleCategory = "Измерительный инструмент", DescriptionCategory = "Измерительный инструмент" };
            Category elektro = new Category { Id = 2, TitleCategory = "Электроинструмент", DescriptionCategory = "Электроинструмент" };
            Category sant = new Category { Id = 3, TitleCategory = "Сантехника", DescriptionCategory = "Сантехника" };

            SubCategory subCategoryIzmer1 = new SubCategory { Id = 1, TitleSubCategory = "Линейки", CategoryId = izmer.Id, DescriptionSubCategory = "1" };
            SubCategory subCategoryIzmer2 = new SubCategory { Id = 2, TitleSubCategory = "Уровни", CategoryId = izmer.Id, DescriptionSubCategory = "2" };
            SubCategory subCategoryIzmer3 = new SubCategory { Id = 3, TitleSubCategory = "Нивелиры лазерные", CategoryId = izmer.Id, DescriptionSubCategory = "3" };

            Product productTest1 = new Product { Id = 1, TitleProduct = "Линейка измерительная 300", Price = 1230, SubCategoryId = subCategoryIzmer1.Id, DescriptionProduct = "1" };
            Product productTest2 = new Product { Id = 2, TitleProduct = "Линейка измерительная 500", Price = 1200, SubCategoryId = subCategoryIzmer1.Id, DescriptionProduct = "1" };
            Product productTest3 = new Product { Id = 3, TitleProduct = "Линейка измерительная 1000", Price = 1200, SubCategoryId = subCategoryIzmer1.Id, DescriptionProduct = "1" };
            Product productTest4 = new Product { Id = 4, TitleProduct = "Тестовый продукт категории 2", Price = 1200, SubCategoryId = subCategoryIzmer2.Id, DescriptionProduct = "2" };
            Product productTest5 = new Product { Id = 5, TitleProduct = "Тестовый продукт категории 2", Price = 1200, SubCategoryId = subCategoryIzmer2.Id, DescriptionProduct = "2" };

            PathImage pathImagesIzmer1 = new PathImage { Id = 1, NameImage = "izmer.jpg", ProductId = productTest1.Id, CategoryId = izmer.Id, SubCategoryId = subCategoryIzmer1.Id, Slider = false, TypeImage = "image/jpeg" };
            PathImage pathImagesTest2 = new PathImage { Id = 2, NameImage = "esteamerby.png", ProductId = productTest2.Id, CategoryId = elektro.Id, SubCategoryId = subCategoryIzmer2.Id, Slider = false, TypeImage = "image/png" };
            PathImage pathImagesTest3 = new PathImage { Id = 3, NameImage = "esteamerby.png", ProductId = productTest3.Id, CategoryId = izmer.Id, Slider = false, TypeImage = "image/png" };
            PathImage pathImagesTest4 = new PathImage { Id = 4, NameImage = "esteamerby.png", ProductId = productTest4.Id, CategoryId = elektro.Id, SubCategoryId = subCategoryIzmer1.Id, Slider = false, TypeImage = "image/png" };
            PathImage pathImagesTest5 = new PathImage { Id = 5, NameImage = "esteamerby.png", ProductId = productTest5.Id, CategoryId = izmer.Id, Slider = false, TypeImage = "image/png" };

            PathImage sliderTest1 = new PathImage { Id = 6, ProductId = null, NameImage = "1.jpg", Slider = true, TypeImage = "image/jpeg" };
            PathImage sliderTest2 = new PathImage { Id = 7, ProductId = null, NameImage = "2.jpg", Slider = true, TypeImage = "image/jpeg" };

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
            modelBuilder.Entity<Category>().HasData(new Category[] { izmer, elektro });
            modelBuilder.Entity<SubCategory>().HasData(new SubCategory[] { subCategoryIzmer1, subCategoryIzmer2, subCategoryIzmer3 });
            modelBuilder.Entity<PathImage>().HasData(new PathImage[] { pathImagesIzmer1, pathImagesTest2, pathImagesTest3, pathImagesTest4, pathImagesTest5, sliderTest1, sliderTest2 });
            modelBuilder.Entity<Product>().HasData(new Product[] { productTest1, productTest2, productTest3, productTest4, productTest5 });
            modelBuilder.Entity<Client>().HasData(new Client[] { client });
            modelBuilder.Entity<PayMethod>().HasData(new PayMethod[] { payMethodTest1, payMethodTest2, payMethodTest3 });
            modelBuilder.Entity<DeliveryMethod>().HasData(new DeliveryMethod[] { deliveryMethodTest1, deliveryMethodTest2, deliveryMethodTest3 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
