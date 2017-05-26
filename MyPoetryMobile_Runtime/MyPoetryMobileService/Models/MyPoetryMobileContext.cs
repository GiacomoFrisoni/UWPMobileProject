namespace MyPoetryMobileService.Models
{
    using System.Data.Entity;

    public partial class MyPoetryMobileContext : DbContext
    {
        public MyPoetryMobileContext(string connectionString)
            : base(connectionString) { }

        public virtual DbSet<Poetry> Poetry { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poetry>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.ActivationCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Poetry)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
