using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inern_Project.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
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
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<ShippingCosts> ShippingCosts { get; set; }
        public DbSet<OrderCoupons> OrderCoupon { get; set; }
        public DbSet<BannerSlide> BannerSlides { get; set; }
        public DbSet<ProductRates> ProductRates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //var hasher= new PasswordHasher<ApplicationUser>();
            //string passwordhashed = hasher.HashPassword(null, "Admin123");
            //builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            //{
            //    Id = Guid.Parse("57F2F45D-B062-40EB-BEAF-0CF31E196D64"),
            //    UserName = "admin@gmail.com",
            //    NormalizedUserName = "ADMIN",
            //    PersonName = "admin",
            //    Email = "admin@gmail.com",
            //    NormalizedEmail = "ADMIN@GMAIL.COM",
            //    EmailConfirmed = true,
            //    PasswordHash = passwordhashed,  
            //    SecurityStamp = "4AA279DC-CFC5-4B21-96C7-52AABB89F127",
            //});
           
            #region OLD
            builder.Entity<ProductRates>().HasOne(u=>u.User).WithMany(p=>p.ProductRates).HasForeignKey(u=>u.UserID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BannerSlide>().HasOne(c => c.BackgroundColor).WithMany(c => c.BannerSlides).HasForeignKey(r => r.BackgroundColorID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProductRates>().HasOne(c => c.Product).WithMany(p => p.ProductRates).HasForeignKey(r => r.ProductID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payments>().Property(p => p.PaymentMethod).HasConversion<string>();
            builder.Entity<Payments>().Property(p => p.Status).HasConversion<string>();
            builder.Entity<Payments>().HasOne(u => u.User).WithMany(p => p.Payments).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>().Property(p => p.OrderStatus).HasConversion<string>();
            builder.Entity<Order>().HasOne(c => c.User).WithMany(r => r.Orders).HasForeignKey(o => o.UserID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(c => c.ShippingCosts).WithMany(r => r.orders).HasForeignKey(o => o.ShippingCostID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(c => c.OrderCoupon).WithMany(r => r.orders).HasForeignKey(o => o.OrderCouponID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(o => o.Address).WithMany(a => a.orders).HasForeignKey(o => o.AddressID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ShippingCosts>().HasOne(r => r.Area).WithOne(r => r.ShippingCosts).HasForeignKey<ShippingCosts>(o => o.AraeID);


            builder.Entity<OrderCoupons>().Property(p => p.DiscountType).HasConversion<string>();

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

            builder.Entity<Address>().HasOne(a=>a.City).WithMany(c=>c.Addresses).HasForeignKey(a=>a.CityID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Address>().HasOne(a=>a.Area).WithMany(c=>c.Addresses).HasForeignKey(a=>a.AreaID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Address>().HasOne(a=>a.User).WithMany(c=>c.Addresses).HasForeignKey(a=>a.UserID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Area>().HasOne(a=>a.City)
                .WithMany(r=>r.Areas)
                .HasForeignKey(c=>c.CityID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Size>().HasData(
new Size
{
SizeID = Guid.Parse("4F0406B9-FE94-487F-9049-0B28DA2C9274"),
SizeName = "Small"
},
new Size

{
SizeID = Guid.Parse("730323EA-7E2E-4EC1-98F2-78FA1AA7C5E9"),
SizeName = "Medium"
},
new Size
{
SizeID = Guid.Parse("67AF942F-AEEB-4A4D-964B-0EFFC559368E"),
SizeName = "Large"
},
new Size
{
SizeID = Guid.Parse("D43285B3-B034-42CC-8795-E5FFFB245654"),
SizeName = "X Large"
},
new Size
{
SizeID = Guid.Parse("66A0C1C9-9DB3-421A-87D0-F8C965687703"),
SizeName = "XX Large"
}
);
            builder.Entity<Product>().HasOne(p => p.Size).WithMany(p => p.Products).HasForeignKey(p => p.SizeID).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>().HasOne(p=>p.Color).WithMany(p=>p.Products).HasForeignKey(p=>p.ColorID).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Colors>().HasIndex(p => p.ColorName).IsUnique();
            builder.Entity<Colors>().HasData(
        new Colors
        {
            ColorID = Guid.Parse("179D4D45-54A7-48DA-82F4-AD4673854BD2"),
            ColorName = "Black",
            ColorHexCode = "#000000"
        },
        new Colors

        {
            ColorID = Guid.Parse("97CA3FD7-7860-4961-8DDE-D90CEC05E3CF"),
            ColorName = "White",
            ColorHexCode = "#FFFFFF"
        },
        new Colors
        {
            ColorID = Guid.Parse("91099764-4FF3-4A91-8F84-3BF913CA3B6C"),
            ColorName = "Red",
            ColorHexCode = "#FF0000"
        },
        new Colors
        {
            ColorID = Guid.Parse("438B0E6E-AF1F-4BF1-9E18-EF5A2DDD94F3"),
            ColorName = "Blue",
            ColorHexCode = "#0000FF"
        },
        new Colors
        {
            ColorID = Guid.Parse("220096A6-C67A-4640-83F2-AE526A2A542D"),
            ColorName = "Green",
            ColorHexCode = "#008000"
        },
        new Colors
        {
            ColorID = Guid.Parse("0FB24ADB-A140-4C50-9EB6-4D948408F30D"),
            ColorName = "Yellow",
            ColorHexCode = "#FFFF00"
        },
        new Colors
        {
            ColorID = Guid.Parse("A3A07D13-C759-4920-A68A-47EF88E7594D"),
            ColorName = "Orange",
            ColorHexCode = "#FFA500"
        },
        new Colors
        {
            ColorID = Guid.Parse("0A65996C-B895-42BC-AF60-FAC2F914376C"),
            ColorName = "Purple",
            ColorHexCode = "#800080"
        },
        new Colors
        {
            ColorID = Guid.Parse("72A9CBB7-360C-4C75-97F7-680BAF249A22"),
            ColorName = "Pink",
            ColorHexCode = "#FFC0CB"
        },
        new Colors
        {
            ColorID = Guid.Parse("1B0856EC-68C8-49D5-9872-2A3F12C373AC"),
            ColorName = "Gray",
            ColorHexCode = "#808080"
        }
    );



            builder.Entity<ApplicationRole>().HasData(
              new ApplicationRole
              {
                  Id = Guid.Parse("BFFBE3E7-4ADF-4BB5-83DB-6880F743D5DA"),
                  Name = "Admin",
                  NormalizedName = "ADMIN",
                  ConcurrencyStamp = Guid.Parse("1C23EDF8-0BC4-41E4-BF72-F89F0594EAD7").ToString()
                  ,
                  RoleDescription = "Admin role with full permissions"

              }, new ApplicationRole
              {
                  Id = Guid.Parse("BCF2E140-CF5C-4352-A238-E04C7F220E64"),
                  Name = "User",
                  NormalizedName = "USER",
                  RoleDescription = "User role with limited permissions",
                  ConcurrencyStamp = Guid.Parse("3D02C4F7-16A9-47F9-BE71-3E27A0B9C46B").ToString()
              });

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
