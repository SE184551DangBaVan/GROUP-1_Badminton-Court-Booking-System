using System;
using System.Collections.Generic;
using BadmintonBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace BadmintonBooking.Models;

public partial class DemobadmintonContext : BadmintonBookingIdentityContext
{
    
    public DemobadmintonContext(DbContextOptions<DemobadmintonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=demobadminton;User ID=sa;Password=12345;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BId).HasName("PK__Booking__4B26EFE64BF64B9B");

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
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.HasKey(e => e.CoId).HasName("PK__Court__F38FB8F59E478C7F");

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
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__Payment__A3420A77AA68D8C7");

            entity.ToTable("Payment");

            entity.Property(e => e.PId)
                .ValueGeneratedNever()
                .HasColumnName("P_ID");
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

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.TsId).HasName("PK__TimeSlot__D128865A3809844E");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.TsId).HasColumnName("TS_ID");
            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.CoId).HasColumnName("CO_ID");
            entity.Property(e => e.TsCheckedIn).HasColumnName("TS_Checked_in");
            entity.Property(e => e.TsDate).HasColumnName("TS_Date");
            entity.Property(e => e.TsEnd).HasColumnName("TS_End");
            entity.Property(e => e.TsStart).HasColumnName("TS_Start");
            entity.Property(e => e.UserId).HasMaxLength(450);

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
