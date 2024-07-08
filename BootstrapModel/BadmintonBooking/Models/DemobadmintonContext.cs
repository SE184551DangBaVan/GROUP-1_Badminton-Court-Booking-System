using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BadmintonBooking.Models;

public partial class DemobadmintonContext : DbContext
{
    public DemobadmintonContext()
    {
    }

    public DemobadmintonContext(DbContextOptions<DemobadmintonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("BadmintonBookingIdentityContextConnection"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BId).HasName("PK__Booking__4B26EFE60BB05A0F");

            entity.ToTable("Booking");

            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.BBookingType)
                .HasMaxLength(100)
                .HasColumnName("B_Booking_Type");
            entity.Property(e => e.BGuestName)
                .HasMaxLength(100)
                .HasColumnName("B_Guest_Name");
            entity.Property(e => e.BTotalHours).HasColumnName("B_Total_Hours");
            entity.Property(e => e.CoId).HasColumnName("CO_ID");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Co).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking.CO_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_AspNetUsers");
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.HasKey(e => e.CoId).HasName("PK__Court__F38FB8F5CC93CDB2");

            entity.ToTable("Court");

            entity.Property(e => e.CoId).HasColumnName("CO_ID");
            entity.Property(e => e.CoAddress)
                .HasMaxLength(255)
                .HasColumnName("CO_Address");
            entity.Property(e => e.CoInfo)
                .HasMaxLength(1000)
                .HasColumnName("CO_Info");
            entity.Property(e => e.CoName)
                .HasMaxLength(255)
                .HasColumnName("CO_Name");
            entity.Property(e => e.CoPath)
                .HasMaxLength(255)
                .HasColumnName("CO_Path");
            entity.Property(e => e.CoPrice).HasColumnName("CO_Price");
            entity.Property(e => e.CoStatus).HasColumnName("CO_Status");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.Courts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Court_AspNetUsers");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__Payment__A3420A77F1652EFD");

            entity.ToTable("Payment");

            entity.Property(e => e.PId).HasColumnName("P_ID");
            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.PAmount)
                .HasColumnType("decimal(11, 2)")
                .HasColumnName("P_Amount");
            entity.Property(e => e.PDateTime)
                .HasColumnType("datetime")
                .HasColumnName("P_Date_Time");

            entity.HasOne(d => d.BIdNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment.B_ID");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__FCCDF87C53BCEA20");

            entity.ToTable("Rating");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Rating1).HasColumnName("Rating");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Court).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Court_Rating");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.TsId).HasName("PK__TimeSlot__D128865A57281125");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.TsId).HasColumnName("TS_ID");
            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.CoId).HasColumnName("CO_ID");
            entity.Property(e => e.TsCheckedIn).HasColumnName("TS_Checked_in");
            entity.Property(e => e.TsDate).HasColumnName("TS_Date");
            entity.Property(e => e.TsEnd).HasColumnName("TS_End");
            entity.Property(e => e.TsStart).HasColumnName("TS_Start");

            entity.HasOne(d => d.BIdNavigation).WithMany(p => p.TimeSlots)
                .HasForeignKey(d => d.BId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Time Slot.B_ID");

            entity.HasOne(d => d.Co).WithMany(p => p.TimeSlots)
                .HasForeignKey(d => d.CoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Time Slot.CO_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
