namespace Facial.Recognize.Core.Data
{
    using Facial.Recognize.Core.Models;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;

    public class TrainningFaceContext : DbContext
    {
        public TrainningFaceContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Face> Faces { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
