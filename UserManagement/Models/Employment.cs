using System;
using System.Collections.Generic;

namespace UserManagement.Models
{
    public class Employment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EmployerId { get; set; }
        public bool IsSuperUser { get; set; }
        public DateTime AssignmentDate { get; set; }

        //The products this user can access
        public List<AssignedProduct> AccessibleProducts { get; set; }
    }
}
