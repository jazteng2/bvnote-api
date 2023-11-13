using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bvnote_web_api.Data;

public partial class BvnV1Context : DbContext
{
    public BvnV1Context()
    {
    }

    public BvnV1Context(DbContextOptions<BvnV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Abbrev> Abbrevs { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Verse> Verses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=127.0.0.1; Database=bvn_v1; Uid=root; Port=3306;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abbrev>(entity =>
        {
            entity.HasKey(e => e.AbbrevId).HasName("PRIMARY");

            entity.ToTable("abbrev");

            entity.HasIndex(e => e.BookId, "BookID");

            entity.Property(e => e.AbbrevId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("AbbrevID");
            entity.Property(e => e.Abbreviation).HasMaxLength(40);
            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookID");

            entity.HasOne(d => d.Book).WithMany(p => p.Abbrevs)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("abbrev_ibfk_1");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");

            entity.ToTable("book");

            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookID");
            entity.Property(e => e.OldTestament).HasColumnType("bit(1)");
            entity.Property(e => e.Title).HasMaxLength(40);
        });

        modelBuilder.Entity<Verse>(entity =>
        {
            entity.HasKey(e => e.VerseId).HasName("PRIMARY");

            entity.ToTable("verse");

            entity.HasIndex(e => e.BookId, "BookID");

            entity.Property(e => e.VerseId)
                .HasMaxLength(36)
                .HasColumnName("VerseID");
            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BookID");
            entity.Property(e => e.Content).HasColumnType("text");

            entity.HasOne(d => d.Book).WithMany(p => p.Verses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verse_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
