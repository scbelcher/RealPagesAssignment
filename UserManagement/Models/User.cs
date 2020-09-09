using System;
using System.Collections.Generic;

namespace UserManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public int StatusId { get; set; } //The status of the user (active/suspended/terminated)
        public DateTime CreateDate { get; set; }

        //Must have at least one entry for each User
        public List<Employment> Employments { get; set; }     
    }
}
