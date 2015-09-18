using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThesisProject.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class UserPreference
    {
        public UserPreference()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string Gender { get; set; }
        public string School { get; set; }
        public string College { get; set; }
        public string WorkPlace { get; set; }
        public string JobTitle { get; set; }
        public List<String> SocialMedia { get; set; }
        public List<String> ShoppingDomain { get; set; }
    }
}
