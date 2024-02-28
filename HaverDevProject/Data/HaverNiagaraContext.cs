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

    public virtual DbSet<Defect> Defects { get; set; }

    public virtual DbSet<Drawing> Drawings { get; set; }

    public virtual DbSet<EngDispositionType> EngDispositionTypes { get; set; }

    public virtual DbSet<FollowUpType> FollowUpTypes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemDefect> ItemDefects { get; set; }

    public virtual DbSet<ItemDefectPhoto> ItemDefectPhotos { get; set; }

    public virtual DbSet<Ncr> Ncrs { get; set; }

    public virtual DbSet<NcrEng> NcrEngs { get; set; }

    public virtual DbSet<NcrOperation> NcrOperations { get; set; }

    public virtual DbSet<NcrQa> NcrQas { get; set; }

    public virtual DbSet<NcrReInspect> NcrReInspects { get; set; }

    public virtual DbSet<OpDispositionType> OpDispositionTypes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<NcrProcurement> NcrProcurements { get; set; }

    public virtual DbSet<SupplierReturn> SupplierReturns { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HaverNiagara;Trusted_Connection=SSPI;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Defect>(entity =>
        {
            entity.HasKey(e => e.DefectId).HasName("pk_defect_defectId");
        });

        modelBuilder.Entity<Drawing>(entity =>
        {
            entity.HasKey(e => e.DrawingId).HasName("pk_drawing_drawingId");
        });

        modelBuilder.Entity<SupplierReturn>(entity =>
        {
            entity.HasKey(e => e.SupplierReturnId).HasName("pk_supplierReturn_supplierReturnId");
        });


        modelBuilder.Entity<EngDispositionType>(entity =>
        {
            entity.HasKey(e => e.EngDispositionTypeId).HasName("pk_engDispoistionType_engDispositionTypeId");
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

        modelBuilder.Entity<ItemDefect>()
            .HasKey(t => new { t.ItemId, t.DefectId });

        modelBuilder.Entity<ItemDefectPhoto>(entity =>
        {
            entity.HasKey(e => e.ItemDefectPhotoId).HasName("pk_itemDefectPhoto_itemDefectPhotoId");

            entity.HasOne(d => d.NcrQa).WithMany(p => p.ItemDefectPhotos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itemDefectPhoto_itemDefect");
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

        modelBuilder.Entity<NcrOperation>(entity =>
        {
            entity.HasKey(e => e.NcrOpId).HasName("pk_ncrOperation_NcrOpId");

            entity.HasOne(d => d.OpDispositionType).WithMany(p => p.NcrOperations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ncrOperation_opDispositionType");
        });

        modelBuilder.Entity<NcrQa>(entity =>
        {
            entity.HasKey(e => e.NcrQaId).HasName("pk_ncrQA_ncrQAId");

            entity.HasOne(d => d.Item).WithMany(p => p.NcrQas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ncrQa_item");
        });

        modelBuilder.Entity<NcrReInspect>(entity =>
        {
            entity.HasKey(e => e.NcrReInspectId).HasName("pk_ncrReInspect_ncrReInspectId");
        });

        modelBuilder.Entity<NcrProcurement>(entity =>
        {
            entity.HasKey(e => e.NcrProcurementId).HasName("pk_ncrProcurement_ncrProcurementId");
        });

        modelBuilder.Entity<OpDispositionType>(entity =>
        {
            entity.HasKey(e => e.OpDispositionTypeId).HasName("pk_opDispositionType_opDispositionTypeId");
        });
               

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
