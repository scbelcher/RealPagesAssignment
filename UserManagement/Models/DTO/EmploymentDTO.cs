using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models.DTO
{
    public class EmploymentDTO
    {
        public int UserId { get; set; }
        public int EmployerId { get; set; }
        public bool IsSuperUser { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
