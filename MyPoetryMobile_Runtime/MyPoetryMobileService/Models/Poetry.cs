namespace MyPoetryMobileService.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Poetry")]
    public partial class Poetry
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(40)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime RevisionDate { get; set; }

        public int CharactersNumber { get; set; }

        public int WordsNumber { get; set; }

        public int VersesNumber { get; set; }

        public bool BookmarkYN { get; set; }

        [Required]
        [StringLength(255)]
        public string Id { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public virtual User User { get; set; }
    }
}
