using System;

namespace UserManagement.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
