using System;

namespace UserManagement.Models.DTO
{
    public class AssignedProductDTO
    {
        public int ProductId { get; set; }
        public int EmployerId { get; set; }
        public int LicenseTypeId { get; set; }
        public int MinimumRoleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
