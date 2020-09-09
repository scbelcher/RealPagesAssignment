using Microsoft.EntityFrameworkCore;

namespace UserManagement.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLicense> ProductLicenses { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<LicenseType> LicenseTypes { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<AssignedProduct> AssignedProducts { get; set; }
    }
}
