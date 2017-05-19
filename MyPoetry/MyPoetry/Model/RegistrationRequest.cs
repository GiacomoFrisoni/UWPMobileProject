using System;

namespace MyPoetry.Model
{
    class RegistrationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public byte[] Photo { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
