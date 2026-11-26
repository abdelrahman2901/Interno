using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inerno.Infrastructure.ApplicationDbContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Colors> Color { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<ShippingCosts> ShippingCosts { get; set; }
        public DbSet<OrderCoupons> OrderCoupon { get; set; }
        public DbSet<BannerSlide> BannerSlides { get; set; }
        public DbSet<ProductRates> ProductRates { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUserRoles> ApplicationUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<ApplicationRole>().HasData(
              new ApplicationRole
              {
                  RoleID = Guid.Parse("BFFBE3E7-4ADF-4BB5-83DB-6880F743D5DA"),
                  RoleName = "Admin",
                  RoleDescription = "Admin role with full permissions",
                  RoleEnumID = RolesEnum.Admin,

              }, new ApplicationRole
              {
                  RoleID = Guid.Parse("BCF2E140-CF5C-4352-A238-E04C7F220E64"),
                  RoleName = "Customer",
                  RoleDescription = "User role with limited permissions",
                  RoleEnumID = RolesEnum.Customer,
              });

            builder.Entity<Product>()
                .HasIndex(r => new { r.ProductID, r.CreatedAt })
                .IsDescending(false, true);

            #region OLD
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser()
            {
                UserID = Guid.Parse("D1A9B5C3-4F2E-4A6B-8C1D-9F3E2A1B5C3D"),
                PersonName = "Admin User",
                Email = "Admin@gmail.com",
                PhoneNumber = "+201066761644",
                Gender = GenderEnum.Male,
                Password = "Admin12",
                IsGmailVerified = true,
                IsPhoneNumberVerified = true,
                IsBlocked = false,
                IsDeleted = false,
                RefreshToken = null,
                RefreshTokenExpirtation = null,
            });

            builder.Entity<ApplicationUserRoles>().HasData(new ApplicationUserRoles()
            {
                UserID = Guid.Parse("D1A9B5C3-4F2E-4A6B-8C1D-9F3E2A1B5C3D"),
                RoleID = Guid.Parse("BFFBE3E7-4ADF-4BB5-83DB-6880F743D5DA")
            });
            builder.Entity<ApplicationRole>().HasIndex(r => r.RoleEnumID).IsUnique();
            #region OLD


            builder.Entity<ProductRates>().ToTable(table =>
            {
                table.HasCheckConstraint("CK_ProductRates_Rating", "[Rating] BETWEEN 1 AND 5");
            });
            builder.Entity<ApplicationUser>().HasIndex(r => r.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(r => r.PhoneNumber).IsUnique();

            builder.Entity<ApplicationUserRoles>().HasKey(r => new { r.UserID, r.RoleID });
            builder.Entity<ApplicationUser>().Property(r => r.Gender).HasConversion<string>();
            builder.Entity<ApplicationUser>()
    .ToTable(table =>
        table.HasCheckConstraint(
            "CK_ApplicationUser_Gender",
            $"[Gender] IN ('{GenderEnum.Male.ToString()}', '{GenderEnum.Female.ToString()}')"
        ));

            builder.Entity<ApplicationUserRoles>()
                .HasOne(r => r.User)
                .WithMany(r => r.Roles)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade); ;

            builder.Entity<ApplicationUserRoles>()
               .HasOne(r => r.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(x => x.RoleID)
               .OnDelete(DeleteBehavior.Cascade);




            builder.Entity<ProductRates>().HasOne(u => u.User).WithMany(p => p.ProductRates).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BannerSlide>().HasOne(c => c.BackgroundColor).WithMany().HasForeignKey(r => r.BackgroundColorID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProductRates>().HasOne(c => c.Product).WithMany(p => p.ProductRates).HasForeignKey(r => r.ProductID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payments>().Property(p => p.PaymentMethod).HasConversion<string>();
            builder.Entity<Payments>().Property(p => p.Status).HasConversion<string>();
            builder.Entity<Payments>()
    .ToTable(table =>
        table.HasCheckConstraint(
            "CK_Payments_PaymentMethod",
            $"[PaymentMethod] IN ('{PaymentMethods.Cash.ToString()}', '{PaymentMethods.CreditCard.ToString()}')"
        ));
            builder.Entity<Payments>()
    .ToTable(table =>
        table.HasCheckConstraint(
            "CK_Payments_Status",
            $"[Status] IN ('{StatusEnum.Approved.ToString()}', '{StatusEnum.Rejected.ToString()}', '{StatusEnum.Pending.ToString()}')"
        ));
            builder.Entity<Payments>().HasOne(u => u.User).WithMany(p => p.Payments).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>().Property(p => p.OrderStatus).HasConversion<string>();
            builder.Entity<Order>()
   .ToTable(table =>
       table.HasCheckConstraint(
           "CK_Order_OrderStatus",
           $"[OrderStatus] IN ('{OrderStatus.Cancelled.ToString()}', '{OrderStatus.Delivered.ToString()}', '{OrderStatus.Shipped.ToString()}' , '{OrderStatus.Processing.ToString()}')"
       ));
            builder.Entity<Order>().HasOne(c => c.User).WithMany(r => r.Orders).HasForeignKey(o => o.UserID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(c => c.ShippingCosts).WithMany(r => r.orders).HasForeignKey(o => o.ShippingCostID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(c => c.OrderCoupon).WithMany(r => r.orders).HasForeignKey(o => o.OrderCouponID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(o => o.Address).WithMany(a => a.orders).HasForeignKey(o => o.AddressID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ShippingCosts>().HasOne(r => r.Area).WithOne(r => r.ShippingCosts).HasForeignKey<ShippingCosts>(o => o.AraeID);


            builder.Entity<OrderCoupons>().Property(p => p.DiscountType).HasConversion<string>();
            builder.Entity<OrderCoupons>()
    .ToTable(table =>
        table.HasCheckConstraint(
            "CK_OrderCoupon_DiscountType",
            $"[DiscountType] IN ('{DiscountTypeEnum.Percentage.ToString()}', '{DiscountTypeEnum.Fixed.ToString()}')"
        ));


            builder.Entity<OrderItems>().HasOne(ot => ot.Product).WithMany(o => o.OrderItems).HasForeignKey(ot => ot.ProductID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderItems>().HasOne(ot => ot.Order).WithMany(o => o.OrderItems).HasForeignKey(ot => ot.OrderID).OnDelete(DeleteBehavior.NoAction);



            builder.Entity<Cart>().HasIndex(u => u.UserID).IsUnique();

            builder.Entity<CartItems>().HasOne(u => u.Cart)
                .WithMany(r => r.CartItems)
                .HasForeignKey(r => r.CartID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CartItems>().HasOne(u => u.Product)
                .WithMany(r => r.CartItems)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WishList>().HasOne(u => u.Product)
                .WithMany(r => r.WishLists)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WishList>().HasOne(u => u.User)
                .WithMany(r => r.WishLists)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Address>().HasOne(a => a.City).WithMany(c => c.Addresses).HasForeignKey(a => a.CityID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Address>().HasOne(a => a.Area).WithMany(c => c.Addresses).HasForeignKey(a => a.AreaID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Address>().HasOne(a => a.User).WithMany(c => c.Addresses).HasForeignKey(a => a.UserID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Area>().HasOne(a => a.City)
                .WithMany(r => r.Areas)
                .HasForeignKey(c => c.CityID)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Product>().HasOne(p => p.Size).WithMany(p => p.Products).HasForeignKey(p => p.SizeID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>().HasOne(p => p.Color).WithMany(p => p.Products).HasForeignKey(p => p.ColorID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Colors>().HasIndex(p => p.ColorName).IsUnique();



            builder.Entity<Category>().HasOne(p => p.ParentCategory)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(p => p.ParentCategoryID)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Product)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }

}
