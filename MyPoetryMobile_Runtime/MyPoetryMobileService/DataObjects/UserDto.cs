using Microsoft.Azure.Mobile.Server;
using System;

namespace MyPoetryMobileService.DataObjects
{
    public class UserDto : EntityData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public byte[] Photo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int AccessesNumber { get; set; }
        public int UseTime { get; set; }
    }
}