using System;
using System.Collections.Generic;

namespace UserManagement.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public List<LicenseType> Licenses { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
