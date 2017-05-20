using System;

namespace MyPoetry.Model
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public byte[] Photo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AccessesNumber { get; set; }
        public int UseTime { get; set; }
    }
}
