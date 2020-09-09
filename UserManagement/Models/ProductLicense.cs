using System;

namespace UserManagement.Models
{
    public class ProductLicense
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int EmployerId { get; set; }
        public int LicenseTypeId { get; set; }
        public int MinimumRoleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
