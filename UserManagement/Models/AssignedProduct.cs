using System;

namespace UserManagement.Models
{
    public class AssignedProduct
    {
        public int Id { get; set; }
        public ProductLicense Product { get; set; }
        public int UserId { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
