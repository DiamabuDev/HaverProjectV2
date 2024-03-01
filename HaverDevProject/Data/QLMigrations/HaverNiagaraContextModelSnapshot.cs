﻿// <auto-generated />
using System;
using HaverDevProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HaverDevProject.Data.QLMigrations
{
    [DbContext(typeof(HaverNiagaraContext))]
    partial class HaverNiagaraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("HaverDevProject.Models.Defect", b =>
                {
                    b.Property<int>("DefectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("defectId");

                    b.Property<string>("DefectDesription")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("defectDesription");

                    b.Property<string>("DefectName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("defectName");

                    b.HasKey("DefectId")
                        .HasName("pk_defect_defectId");

                    b.ToTable("defect");
                });

            modelBuilder.Entity("HaverDevProject.Models.Drawing", b =>
                {
                    b.Property<int>("DrawingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("drawingId");

                    b.Property<int>("DrawingOriginalRevNumber")
                        .HasColumnType("INTEGER")
                        .HasColumnName("drawingOriginalRevNumber");

                    b.Property<bool>("DrawingRequireUpdating")
                        .HasColumnType("INTEGER")
                        .HasColumnName("DrawingRequireUpdating");

                    b.Property<DateTime>("DrawingRevDate")
                        .HasColumnType("date")
                        .HasColumnName("drawingRevDate");

                    b.Property<int>("DrawingUpdatedRevNumber")
                        .HasColumnType("INTEGER")
                        .HasColumnName("drawingUpdatedRevNumber");

                    b.Property<int>("DrawingUserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("drawingUserId");

                    b.Property<int>("NcrEngId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ncrEngId");

                    b.HasKey("DrawingId")
                        .HasName("pk_drawing_drawingId");

                    b.HasIndex("NcrEngId")
                        .IsUnique();

                    b.ToTable("drawing");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngDefectPhoto", b =>
                {
                    b.Property<int>("EngDefectPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("engDefectPhotoId");

                    b.Property<byte[]>("EngDefectPhotoContent")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("engDefectPhotoContent");

                    b.Property<string>("EngDefectPhotoDescription")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("engDefectPhotoDescription");

                    b.Property<string>("EngDefectPhotoMimeType")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("engDefectPhotoMimeType");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrEngId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ncrEngId");

                    b.HasKey("EngDefectPhotoId")
                        .HasName("pk_engDefectPhoto_engDefectPhotoId");

                    b.HasIndex("NcrEngId");

                    b.ToTable("engDefectPhoto");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngDispositionType", b =>
                {
                    b.Property<int>("EngDispositionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("engDispositionTypeId");

                    b.Property<string>("EngDispositionTypeName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("engDispositionTypeName");

                    b.HasKey("EngDispositionTypeId")
                        .HasName("pk_engDispoistionType_engDispositionTypeId");

                    b.ToTable("engDispositionType");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngFileContent", b =>
                {
                    b.Property<int>("EngFileContentID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.HasKey("EngFileContentID");

                    b.ToTable("EngFileContent");
                });

            modelBuilder.Entity("HaverDevProject.Models.FileContent", b =>
                {
                    b.Property<int>("FileContentID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.HasKey("FileContentID");

                    b.ToTable("FileContent");
                });

            modelBuilder.Entity("HaverDevProject.Models.FollowUpType", b =>
                {
                    b.Property<int>("FollowUpTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FollowUpTypeName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.HasKey("FollowUpTypeId")
                        .HasName("pk_followUpType_followUpTypeId");

                    b.ToTable("followUpType");
                });

            modelBuilder.Entity("HaverDevProject.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemId");

                    b.Property<string>("ItemDescription")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDescription");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("itemName");

                    b.Property<int>("ItemNumber")
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemNumber");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("supplierId");

                    b.HasKey("ItemId")
                        .HasName("pk_item_itemId");

                    b.HasIndex("ItemNumber")
                        .IsUnique();

                    b.HasIndex("SupplierId");

                    b.ToTable("item");
                });

            modelBuilder.Entity("HaverDevProject.Models.ItemDefect", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemId");

                    b.Property<int>("DefectId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("defectId");

                    b.HasKey("ItemId", "DefectId");

                    b.HasIndex("DefectId");

                    b.ToTable("itemDefect");
                });

            modelBuilder.Entity("HaverDevProject.Models.ItemDefectPhoto", b =>
                {
                    b.Property<int>("ItemDefectPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemDefectPhotoId");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ItemDefectPhotoContent")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("itemDefectPhotoContent");

                    b.Property<string>("ItemDefectPhotoDescription")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDefectPhotoDescription");

                    b.Property<string>("ItemDefectPhotoMimeType")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDefectPhotoMimeType");

                    b.Property<int?>("NcrOperationNcrOpId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrQaId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ncrQaId");

                    b.HasKey("ItemDefectPhotoId")
                        .HasName("pk_itemDefectPhoto_itemDefectPhotoId");

                    b.HasIndex("NcrOperationNcrOpId");

                    b.HasIndex("NcrQaId");

                    b.ToTable("itemDefectPhoto");
                });

            modelBuilder.Entity("HaverDevProject.Models.Ncr", b =>
                {
                    b.Property<int>("NcrId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("NcrLastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("NcrNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrPhase")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrStatus")
                        .HasColumnType("INTEGER");

                    b.HasKey("NcrId")
                        .HasName("pk_ncr_ncrId");

                    b.ToTable("ncr");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrEng", b =>
                {
                    b.Property<int>("NcrEngId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("DrawingId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DrawingOriginalRevNumber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DrawingRequireUpdating")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DrawingRevDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("DrawingUpdatedRevNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DrawingUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EngDispositionTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrEngCustomerNotification")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrEngDispositionDescription")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NcrEngStatusFlag")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrEngUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrPhase")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("NcrEngId")
                        .HasName("pk_ncrEng_ncrEngId");

                    b.HasIndex("EngDispositionTypeId");

                    b.HasIndex("NcrId")
                        .IsUnique();

                    b.ToTable("ncrEng");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrOperation", b =>
                {
                    b.Property<int>("NcrOpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Car")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpectedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FollowUp")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FollowUpTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NcrEngId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrPurchasingDescription")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrPurchasingUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OpDispositionTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("UpdateOp")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("NcrOpId")
                        .HasName("pk_ncrOperation_NcrOpId");

                    b.HasIndex("FollowUpTypeId");

                    b.HasIndex("NcrEngId");

                    b.HasIndex("NcrId")
                        .IsUnique();

                    b.HasIndex("OpDispositionTypeId");

                    b.ToTable("NcrOperations");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrProcurement", b =>
                {
                    b.Property<int>("NcrProcurementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrProcCreditExpected")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrProcDisposedAllowed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("NcrProcExpectedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NcrProcFlagStatus")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrProcSAPReturnCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrProcSupplierBilled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrProcSupplierReturnReq")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrProcUserId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("NcrProcurementId")
                        .HasName("pk_ncrProcurement_ncrProcurementId");

                    b.HasIndex("NcrId")
                        .IsUnique();

                    b.ToTable("NcrProcurements");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrQa", b =>
                {
                    b.Property<int>("NcrQaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("DefectId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrQaDefectVideo")
                        .HasColumnType("TEXT");

                    b.Property<string>("NcrQaDescriptionOfDefect")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NcrQaEngDispositionRequired")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrQaItemMarNonConforming")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrQaOrderNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NcrQaProcessApplicable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrQaQuanDefective")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NcrQaQuanReceived")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrQaSalesOrder")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NcrQaStatusFlag")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("NcrQacreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrQauserId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("NcrQaId")
                        .HasName("pk_ncrQA_ncrQAId");

                    b.HasIndex("DefectId");

                    b.HasIndex("ItemId");

                    b.HasIndex("NcrId")
                        .IsUnique();

                    b.ToTable("ncrQA");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrReInspect", b =>
                {
                    b.Property<int>("NcrReInspectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrQaStatusFlag")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NcrReInspectAcceptable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NcrReInspectDefectVideo")
                        .HasColumnType("TEXT");

                    b.Property<string>("NcrReInspectNewNcrNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("NcrReInspectUserId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("NcrReInspectId")
                        .HasName("pk_ncrReInspect_ncrReInspectId");

                    b.HasIndex("NcrId")
                        .IsUnique();

                    b.ToTable("NcrReInspects");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrReInspectPhoto", b =>
                {
                    b.Property<int>("NcrReInspectPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ncrReInspectPhotoId");

                    b.Property<int>("NcrReInspectId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ncrReInspectId");

                    b.Property<byte[]>("NcrReInspectPhotoContent")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("ncrReInspectPhotoContent");

                    b.Property<string>("NcrReInspectPhotoDescription")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("ncrReInspectPhotoDescription");

                    b.Property<string>("NcrReInspectPhotoMimeType")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("ncrReInspectPhotoMimeType");

                    b.HasKey("NcrReInspectPhotoId")
                        .HasName("pk_ncrReInspectPhoto_ncrReInspectPhotoId");

                    b.HasIndex("NcrReInspectId");

                    b.ToTable("ncrReInspectPhoto");
                });

            modelBuilder.Entity("HaverDevProject.Models.OpDispositionType", b =>
                {
                    b.Property<int>("OpDispositionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("OpDispositionTypeName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.HasKey("OpDispositionTypeId")
                        .HasName("pk_opDispositionType_opDispositionTypeId");

                    b.ToTable("opDispositionType");
                });

            modelBuilder.Entity("HaverDevProject.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("supplierId");

                    b.Property<string>("SupplierCode")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("supplierCode");

                    b.Property<string>("SupplierContactName")
                        .HasMaxLength(90)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("supplierContactName");

                    b.Property<string>("SupplierEmail")
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("supplierEmail");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .IsUnicode(false)
                        .HasColumnType("TEXT")
                        .HasColumnName("supplierName");

                    b.Property<bool>("SupplierStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("supplierStatus");

                    b.HasKey("SupplierId")
                        .HasName("pk_supplier_supplierId");

                    b.HasIndex("SupplierCode")
                        .IsUnique();

                    b.ToTable("supplier");
                });

            modelBuilder.Entity("HaverDevProject.Models.Drawing", b =>
                {
                    b.HasOne("HaverDevProject.Models.NcrEng", "NcrEng")
                        .WithOne("Drawing")
                        .HasForeignKey("HaverDevProject.Models.Drawing", "NcrEngId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NcrEng");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngDefectPhoto", b =>
                {
                    b.HasOne("HaverDevProject.Models.NcrEng", "NcrEng")
                        .WithMany("EngDefectPhotos")
                        .HasForeignKey("NcrEngId")
                        .IsRequired()
                        .HasConstraintName("fk_engDefectPhoto_itemDefect");

                    b.Navigation("NcrEng");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngFileContent", b =>
                {
                    b.HasOne("HaverDevProject.Models.EngDefectPhoto", "EngDefectPhoto")
                        .WithOne("EngFileContent")
                        .HasForeignKey("HaverDevProject.Models.EngFileContent", "EngFileContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EngDefectPhoto");
                });

            modelBuilder.Entity("HaverDevProject.Models.FileContent", b =>
                {
                    b.HasOne("HaverDevProject.Models.ItemDefectPhoto", "ItemDefectPhoto")
                        .WithOne("FileContent")
                        .HasForeignKey("HaverDevProject.Models.FileContent", "FileContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemDefectPhoto");
                });

            modelBuilder.Entity("HaverDevProject.Models.Item", b =>
                {
                    b.HasOne("HaverDevProject.Models.Supplier", "Supplier")
                        .WithMany("Items")
                        .HasForeignKey("SupplierId")
                        .IsRequired()
                        .HasConstraintName("fk_item_supplier");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("HaverDevProject.Models.ItemDefect", b =>
                {
                    b.HasOne("HaverDevProject.Models.Defect", "Defect")
                        .WithMany("ItemDefects")
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HaverDevProject.Models.Item", "Item")
                        .WithMany("ItemDefects")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("HaverDevProject.Models.ItemDefectPhoto", b =>
                {
                    b.HasOne("HaverDevProject.Models.NcrOperation", null)
                        .WithMany("ItemDefectPhotos")
                        .HasForeignKey("NcrOperationNcrOpId");

                    b.HasOne("HaverDevProject.Models.NcrQa", "NcrQa")
                        .WithMany("ItemDefectPhotos")
                        .HasForeignKey("NcrQaId")
                        .IsRequired()
                        .HasConstraintName("fk_itemDefectPhoto_itemDefect");

                    b.Navigation("NcrQa");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrEng", b =>
                {
                    b.HasOne("HaverDevProject.Models.EngDispositionType", "EngDispositionType")
                        .WithMany("NcrEngs")
                        .HasForeignKey("EngDispositionTypeId")
                        .IsRequired()
                        .HasConstraintName("fk_ncrEng_engDispositionType");

                    b.HasOne("HaverDevProject.Models.Ncr", "Ncr")
                        .WithOne("NcrEng")
                        .HasForeignKey("HaverDevProject.Models.NcrEng", "NcrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EngDispositionType");

                    b.Navigation("Ncr");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrOperation", b =>
                {
                    b.HasOne("HaverDevProject.Models.FollowUpType", "FollowUpType")
                        .WithMany("NcrOperations")
                        .HasForeignKey("FollowUpTypeId");

                    b.HasOne("HaverDevProject.Models.NcrEng", "NcrEng")
                        .WithMany()
                        .HasForeignKey("NcrEngId");

                    b.HasOne("HaverDevProject.Models.Ncr", "Ncr")
                        .WithOne("NcrOperation")
                        .HasForeignKey("HaverDevProject.Models.NcrOperation", "NcrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HaverDevProject.Models.OpDispositionType", "OpDispositionType")
                        .WithMany("NcrOperations")
                        .HasForeignKey("OpDispositionTypeId")
                        .IsRequired()
                        .HasConstraintName("fk_ncrOperation_opDispositionType");

                    b.Navigation("FollowUpType");

                    b.Navigation("Ncr");

                    b.Navigation("NcrEng");

                    b.Navigation("OpDispositionType");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrProcurement", b =>
                {
                    b.HasOne("HaverDevProject.Models.Ncr", "Ncr")
                        .WithOne("NcrProcurement")
                        .HasForeignKey("HaverDevProject.Models.NcrProcurement", "NcrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ncr");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrQa", b =>
                {
                    b.HasOne("HaverDevProject.Models.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HaverDevProject.Models.Item", "Item")
                        .WithMany("NcrQas")
                        .HasForeignKey("ItemId")
                        .IsRequired()
                        .HasConstraintName("fk_ncrQa_item");

                    b.HasOne("HaverDevProject.Models.Ncr", "Ncr")
                        .WithOne("NcrQa")
                        .HasForeignKey("HaverDevProject.Models.NcrQa", "NcrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");

                    b.Navigation("Item");

                    b.Navigation("Ncr");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrReInspect", b =>
                {
                    b.HasOne("HaverDevProject.Models.Ncr", "Ncr")
                        .WithOne("NcrReInspect")
                        .HasForeignKey("HaverDevProject.Models.NcrReInspect", "NcrId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ncr");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrReInspectPhoto", b =>
                {
                    b.HasOne("HaverDevProject.Models.NcrReInspect", "NcrReInspect")
                        .WithMany("NcrReInspectPhotos")
                        .HasForeignKey("NcrReInspectId")
                        .IsRequired();

                    b.Navigation("NcrReInspect");
                });

            modelBuilder.Entity("HaverDevProject.Models.Defect", b =>
                {
                    b.Navigation("ItemDefects");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngDefectPhoto", b =>
                {
                    b.Navigation("EngFileContent");
                });

            modelBuilder.Entity("HaverDevProject.Models.EngDispositionType", b =>
                {
                    b.Navigation("NcrEngs");
                });

            modelBuilder.Entity("HaverDevProject.Models.FollowUpType", b =>
                {
                    b.Navigation("NcrOperations");
                });

            modelBuilder.Entity("HaverDevProject.Models.Item", b =>
                {
                    b.Navigation("ItemDefects");

                    b.Navigation("NcrQas");
                });

            modelBuilder.Entity("HaverDevProject.Models.ItemDefectPhoto", b =>
                {
                    b.Navigation("FileContent");
                });

            modelBuilder.Entity("HaverDevProject.Models.Ncr", b =>
                {
                    b.Navigation("NcrEng");

                    b.Navigation("NcrOperation");

                    b.Navigation("NcrProcurement");

                    b.Navigation("NcrQa");

                    b.Navigation("NcrReInspect");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrEng", b =>
                {
                    b.Navigation("Drawing");

                    b.Navigation("EngDefectPhotos");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrOperation", b =>
                {
                    b.Navigation("ItemDefectPhotos");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrQa", b =>
                {
                    b.Navigation("ItemDefectPhotos");
                });

            modelBuilder.Entity("HaverDevProject.Models.NcrReInspect", b =>
                {
                    b.Navigation("NcrReInspectPhotos");
                });

            modelBuilder.Entity("HaverDevProject.Models.OpDispositionType", b =>
                {
                    b.Navigation("NcrOperations");
                });

            modelBuilder.Entity("HaverDevProject.Models.Supplier", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
