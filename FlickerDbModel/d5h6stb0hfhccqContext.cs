using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlickerDbModel
{
    public partial class d5h6stb0hfhccqContext : DbContext
    {
        public d5h6stb0hfhccqContext(DbContextOptions<d5h6stb0hfhccqContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Faces> Faces { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "3.0.0-preview.19074.3");

            modelBuilder.Entity<Faces>(entity =>
            {
                entity.ToTable("faces");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Anger).HasColumnName("anger");

                entity.Property(e => e.Disgust).HasColumnName("disgust");

                entity.Property(e => e.Fear).HasColumnName("fear");

                entity.Property(e => e.Happiness).HasColumnName("happiness");

                entity.Property(e => e.Neutral).HasColumnName("neutral");

                entity.Property(e => e.PhotoId).HasColumnName("photo_id");

                entity.Property(e => e.Sadness).HasColumnName("sadness");

                entity.Property(e => e.Surprise).HasColumnName("surprise");

                entity.HasOne(d => d.Photo)
                    .WithOne(p => p.Faces)
                    .HasForeignKey<Faces>(d => d.PhotoId)
                    .HasConstraintName("photo_face_fkey");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FlickrId)
                    .HasColumnName("flickr_id")
                    .HasMaxLength(20);

                entity.Property(e => e.IsFace).HasColumnName("is_face");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(100);
            });
        }
    }
}
