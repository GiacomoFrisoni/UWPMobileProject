using Microsoft.Azure.Mobile.Server;
using System;

namespace MyPoetryMobileService.DataObjects
{
    public class UserDto : EntityData
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
        public bool IsActivated { get; set; }
        public string ActivationCode { get; set; }
        public string BackgroundPref { get; set; }
        public string LanguagePref { get; set; }
    }
}