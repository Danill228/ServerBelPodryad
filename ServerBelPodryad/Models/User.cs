using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Models
{
    public class User
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ThirdName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Site { get; set; }
        public Organization organization { get; set; }

        public User()
        {
        }

     
    }
}
