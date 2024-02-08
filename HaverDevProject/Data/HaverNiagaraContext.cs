using System;
using System.Collections.Generic;
using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Data;

public partial class HaverNiagaraContext : DbContext
{
    //To give access to IHttpContextAccessor for Audit Data with IAuditable
    private readonly IHttpContextAccessor _httpContextAccessor;

    //Property to hold the UserName value
    public string UserName
    {
        get; private set;
    }

    public HaverNiagaraContext(DbContextOptions<HaverNiagaraContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        if (_httpContextAccessor.HttpContext != null)
        {
            //We have a HttpContext, but there might not be anyone Authenticated
            UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
            UserName ??= "Unknown";
        }
        else
        {
            //No HttpContext so seeding data
            UserName = "Seed Data";
        }
    }

    public HaverNiagaraContext()
    {
    }

    public HaverNiagaraContext(DbContextOptions<HaverNiagaraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Defect> Defects { get; set; }

    public virtual DbSet<Drawing> Drawings { get; set; }

    public virtual DbSet<EngDispositionType> EngDispositionTypes { get; set; }

    public virtual DbSet<FollowUp> FollowUps { get; set; }

    public virtual DbSet<FollowUpType> FollowUpTypes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemDefect> ItemDefects { get; set; }

    public virtual DbSet<ItemDefectPhoto> ItemDefectPhotos { get; set; }

    public virtual DbSet<ItemDefectVideo> ItemDefectVideos { get; set; }

    public virtual DbSet<Ncr> Ncrs { get; set; }

    public virtual DbSet<NcrEng> NcrEngs { get; set; }

    public virtual DbSet<NcrPurchasing> NcrPurchasings { get; set; }

    public virtual DbSet<NcrQa> NcrQas { get; set; }

    public virtual DbSet<NcrReInspect> NcrReInspects { get; set; }

    public virtual DbSet<OpDispositionType> OpDispositionTypes { get; set; }

    //public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    //public virtual DbSet<ProcessApplicable> ProcessApplicables { get; set; }

    //public virtual DbSet<StatusUpdate> StatusUpdates { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<NcrProcurement> NcrProcurements { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HaverNiagara;Trusted_Connection=SSPI;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("pk_car_carId");
        });

        modelBuilder.Entity<Defect>(entity =>
        {
            entity.HasKey(e => e.DefectId).HasName("pk_defect_defectId");
        });

        modelBuilder.Entity<Drawing>(entity =>
        {
            entity.HasKey(e => e.DrawingId).HasName("pk_drawing_drawingId");
        });

        modelBuilder.Entity<EngDispositionType>(entity =>
        {
            entity.HasKey(e => e.EngDispositionTypeId).HasName("pk_engDispoistionType_engDispositionTypeId");
        });

        modelBuilder.Entity<FollowUp>(entity =>
        {
            entity.HasKey(e => e.FollowUpId).HasName("pk_followUp_followUpId");

            entity.HasOne(d => d.FollowUpType).WithMany(p => p.FollowUps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_followUp_followUpType");
        });

        modelBuilder.Entity<FollowUpType>(entity =>
        {
            entity.HasKey(e => e.FollowUpTypeId).HasName("pk_followUpType_followUpTypeId");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("pk_item_itemId");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_supplier");
        });

        modelBuilder.Entity<ItemDefect>(entity =>
        {
            entity.HasKey(e => e.ItemDefectId).HasName("pk_itemDefect_itemDefectId");

            entity.HasOne(d => d.Defect).WithMany(p => p.ItemDefects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemDefect_defect");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemDefects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemDefect_item");

        });

        modelBuilder.Entity<ItemDefectPhoto>(entity =>
        {
            entity.HasKey(e => e.ItemDefectPhotoId).HasName("pk_itemDefectPhoto_itemDefectPhotoId");

            entity.HasOne(d => d.NcrQa).WithMany(p => p.ItemDefectPhotos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemDefectPhoto_itemDefect");
        });

        modelBuilder.Entity<ItemDefectVideo>(entity =>
        {
            entity.HasKey(e => e.ItemDefectVideoId).HasName("pk_itemDefectVideo");

            entity.HasOne(d => d.NcrQa).WithMany(p => p.ItemDefectVideos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemDefectVideo_itemDefect");
        });

        modelBuilder.Entity<Ncr>(entity =>
        {
            entity.HasKey(e => e.NcrId).HasName("pk_ncr_ncrId");

        });

        modelBuilder.Entity<NcrEng>(entity =>
        {
            entity.HasKey(e => e.NcrEngId).HasName("pk_ncrEng_ncrEngId");

            entity.HasOne(d => d.EngDispositionType).WithMany(p => p.NcrEngs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ncrEng_engDispositionType");
        });

        modelBuilder.Entity<NcrPurchasing>(entity =>
        {
            entity.HasKey(e => e.NcrPurchId).HasName("pk_ncrPurchasing_ncrPurchId");

            entity.HasOne(d => d.OpDispositionType).WithMany(p => p.NcrPurchasings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ncrPurchasing_opDispositionType");
        });

        modelBuilder.Entity<NcrQa>(entity =>
        {
            entity.HasKey(e => e.NcrQaId).HasName("pk_ncrQA_ncrQAId");
        });

        modelBuilder.Entity<NcrReInspect>(entity =>
        {
            entity.HasKey(e => e.NcrReInspectId).HasName("pk_ncrReInspect_ncrReInspectId");
        });

        modelBuilder.Entity<OpDispositionType>(entity =>
        {
            entity.HasKey(e => e.OpDispositionTypeId).HasName("pk_opDispositionType_opDispositionTypeId");
        });

        modelBuilder.Entity<NcrProcurement>(entity =>
        {
            entity.HasKey(e => e.NcrProcurementId).HasName("pk_ncrProcurement_ncrProcurementId");
        });
        //modelBuilder.Entity<OrderDetail>(entity =>
        //{
        //    entity.HasKey(e => e.OrderId).HasName("pk_orderDetail_orderId");

        //    entity.HasOne(d => d.Item).WithMany(p => p.OrderDetails)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("fk_orderDetail_item");

        //    entity.HasOne(d => d.NcrQa).WithMany(p => p.OrderDetails)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("fk_orderDetail_ncrQA");
        //});

        //modelBuilder.Entity<ProcessApplicable>(entity =>
        //{
        //    entity.HasKey(e => e.ProAppId).HasName("pk_processApplicable_proAppId");
        //});

        //modelBuilder.Entity<StatusUpdate>(entity =>
        //{
        //    entity.HasKey(e => e.StatusUpdateId).HasName("pk_status_statusUpdateId");
        //});

        modelBuilder.Entity<Item>()
            .HasIndex(i => i.ItemNumber)
            .IsUnique();

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("pk_supplier_supplierId");

            entity.HasIndex(e => e.SupplierCode).IsUnique(); //check restriction for unique code.
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable trackable)
            {
                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        trackable.UpdatedOn = now;
                        trackable.UpdatedBy = UserName;
                        break;

                    case EntityState.Added:
                        trackable.CreatedOn = now;
                        trackable.CreatedBy = UserName;
                        trackable.UpdatedOn = now;
                        trackable.UpdatedBy = UserName;
                        break;
                }
            }
        }
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
