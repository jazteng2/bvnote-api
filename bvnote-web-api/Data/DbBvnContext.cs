using Microsoft.EntityFrameworkCore;

namespace bvnote_web_api.Data;

public partial class DbBvnContext : DbContext
{
    public DbBvnContext()
    {
    }

    public DbBvnContext(DbContextOptions<DbBvnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Abbrev> Abbrevs { get; set; }
    public virtual DbSet<Verse> Verses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");
            entity.ToTable("book");
            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookId");
            entity.Property(e => e.Title).HasMaxLength(40);
            entity.Property(e => e.OldTestament).HasColumnType("bit(1)");
        });

        modelBuilder.Entity<Abbrev>(entity =>
        {
            entity.HasKey(e => e.AbbrevId).HasName("PRIMARY");
            entity.ToTable("abbrev");
            entity.HasIndex(e => e.BookId, "BookId");
            entity.Property(e => e.AbbrevId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("AbbrevId");
            entity.Property(e => e.Abbreviation).HasMaxLength(20);
            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookId");
            entity.HasOne(d => d.Book).WithMany(p => p.Abbrevs)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("abbrev_ibfk_1");
        });

        modelBuilder.Entity<Verse>(entity =>
        {
            entity.HasKey(e => e.VerseId).HasName("PRIMARY");
            entity.ToTable("versenkjv");
            entity.HasIndex(e => e.BookId, "BookId");
            entity.Property(e => e.VerseId)
                .HasMaxLength(38)
                .HasColumnName("VerseId");
            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookId");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.HasOne(d => d.Book).WithMany(p => p.Verses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("versenkjv_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
