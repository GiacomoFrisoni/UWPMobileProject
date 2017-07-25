namespace MyPoetryMobileService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Poetry = new HashSet<Poetry>();
        }

        [Required]
        [StringLength(40)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string Surname { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [Column(TypeName = "date")]
        public DateTime RegistrationDate { get; set; }

        public int AccessesNumber { get; set; }

        public int UseTime { get; set; }

        public bool IsActivated { get; set; }

        [Required]
        [MaxLength(1024)]
        public byte[] Salt { get; set; }

        [Required]
        [MaxLength(1024)]
        public byte[] SaltedAndHashedPassword { get; set; }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        [StringLength(6)]
        public string ActivationCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Poetry> Poetry { get; set; }

        [StringLength(50)]
        public string BackgroundPref { get; set; }

        [StringLength(2)]
        public string LanguagePref { get; set; }
    }
}
