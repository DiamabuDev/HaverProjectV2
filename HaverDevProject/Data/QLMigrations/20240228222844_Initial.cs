using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaverDevProject.Data.QLMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "defect",
                columns: table => new
                {
                    defectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    defectName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    defectDesription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defect_defectId", x => x.defectId);
                });

            migrationBuilder.CreateTable(
                name: "engDispositionType",
                columns: table => new
                {
                    engDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    engDispositionTypeName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_engDispoistionType_engDispositionTypeId", x => x.engDispositionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "followUpType",
                columns: table => new
                {
                    FollowUpTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FollowUpTypeName = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_followUpType_followUpTypeId", x => x.FollowUpTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ncr",
                columns: table => new
                {
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrNumber = table.Column<string>(type: "TEXT", nullable: true),
                    NcrLastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrPhase = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncr_ncrId", x => x.NcrId);
                });

            migrationBuilder.CreateTable(
                name: "opDispositionType",
                columns: table => new
                {
                    OpDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OpDispositionTypeName = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opDispositionType_opDispositionTypeId", x => x.OpDispositionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplierId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    supplierCode = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    supplierName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    supplierContactName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 90, nullable: true),
                    supplierEmail = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: true),
                    supplierStatus = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_supplierId", x => x.supplierId);
                });

            migrationBuilder.CreateTable(
                name: "ncrEng",
                columns: table => new
                {
                    NcrEngId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrEngCustomerNotification = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrEngDispositionDescription = table.Column<string>(type: "TEXT", nullable: true),
                    NcrEngStatusFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrEngUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    EngDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false),
                    DrawingId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrEng_ncrEngId", x => x.NcrEngId);
                    table.ForeignKey(
                        name: "FK_ncrEng_ncr_NcrId",
                        column: x => x.NcrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrEng_engDispositionType",
                        column: x => x.EngDispositionTypeId,
                        principalTable: "engDispositionType",
                        principalColumn: "engDispositionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "NcrProcurements",
                columns: table => new
                {
                    NcrProcurementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrProcSupplierReturnReq = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcExpectedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrProcDisposedAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcSAPReturnCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcCreditExpected = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcSupplierBilled = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcFlagStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrProcUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrProcUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierReturnId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrProcurement_ncrProcurementId", x => x.NcrProcurementId);
                    table.ForeignKey(
                        name: "FK_NcrProcurements_ncr_NcrId",
                        column: x => x.NcrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ncrReInspect",
                columns: table => new
                {
                    ncrReInspectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrReInspectAcceptable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrReInspectNewNcrNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    ncrReInspectUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrReInspect_ncrReInspectId", x => x.ncrReInspectId);
                    table.ForeignKey(
                        name: "FK_ncrReInspect_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    itemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    itemName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    itemDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    supplierId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_itemId", x => x.itemId);
                    table.ForeignKey(
                        name: "fk_item_supplier",
                        column: x => x.supplierId,
                        principalTable: "supplier",
                        principalColumn: "supplierId");
                });

            migrationBuilder.CreateTable(
                name: "drawing",
                columns: table => new
                {
                    drawingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DrawingRequireUpdating = table.Column<bool>(type: "INTEGER", nullable: false),
                    drawingOriginalRevNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    drawingUpdatedRevNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    drawingRevDate = table.Column<DateTime>(type: "date", nullable: false),
                    drawingUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrEngId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drawing_drawingId", x => x.drawingId);
                    table.ForeignKey(
                        name: "FK_drawing_ncrEng_ncrEngId",
                        column: x => x.ncrEngId,
                        principalTable: "ncrEng",
                        principalColumn: "NcrEngId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NcrOperations",
                columns: table => new
                {
                    NcrOpId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false),
                    OpDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrPurchasingDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Car = table.Column<bool>(type: "INTEGER", nullable: false),
                    CarNumber = table.Column<string>(type: "TEXT", nullable: true),
                    FollowUp = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FollowUpTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateOp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrPurchasingUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrEngId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrOperation_NcrOpId", x => x.NcrOpId);
                    table.ForeignKey(
                        name: "FK_NcrOperations_followUpType_FollowUpTypeId",
                        column: x => x.FollowUpTypeId,
                        principalTable: "followUpType",
                        principalColumn: "FollowUpTypeId");
                    table.ForeignKey(
                        name: "FK_NcrOperations_ncrEng_NcrEngId",
                        column: x => x.NcrEngId,
                        principalTable: "ncrEng",
                        principalColumn: "NcrEngId");
                    table.ForeignKey(
                        name: "FK_NcrOperations_ncr_NcrId",
                        column: x => x.NcrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrOperation_opDispositionType",
                        column: x => x.OpDispositionTypeId,
                        principalTable: "opDispositionType",
                        principalColumn: "OpDispositionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "supplierReturn",
                columns: table => new
                {
                    supplierReturnId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    supplierReturnMANum = table.Column<string>(type: "TEXT", nullable: false),
                    supplierReturnName = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    supplierReturnAccount = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    ncrProcurementId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplierReturn_supplierReturnId", x => x.supplierReturnId);
                    table.ForeignKey(
                        name: "FK_supplierReturn_NcrProcurements_ncrProcurementId",
                        column: x => x.ncrProcurementId,
                        principalTable: "NcrProcurements",
                        principalColumn: "NcrProcurementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "itemDefect",
                columns: table => new
                {
                    itemId = table.Column<int>(type: "INTEGER", nullable: false),
                    defectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemDefect", x => new { x.itemId, x.defectId });
                    table.ForeignKey(
                        name: "FK_itemDefect_defect_defectId",
                        column: x => x.defectId,
                        principalTable: "defect",
                        principalColumn: "defectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itemDefect_item_itemId",
                        column: x => x.itemId,
                        principalTable: "item",
                        principalColumn: "itemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ncrQA",
                columns: table => new
                {
                    NcrQaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrQaItemMarNonConforming = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrQaProcessApplicable = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrQacreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrQaOrderNumber = table.Column<string>(type: "TEXT", nullable: true),
                    NcrQaSalesOrder = table.Column<string>(type: "TEXT", nullable: true),
                    NcrQaQuanReceived = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrQaQuanDefective = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrQaDescriptionOfDefect = table.Column<string>(type: "TEXT", nullable: true),
                    NcrQauserId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrQaEngDispositionRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefectId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrQaDefectVideo = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrQA_ncrQAId", x => x.NcrQaId);
                    table.ForeignKey(
                        name: "FK_ncrQA_defect_DefectId",
                        column: x => x.DefectId,
                        principalTable: "defect",
                        principalColumn: "defectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ncrQA_ncr_NcrId",
                        column: x => x.NcrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrQa_item",
                        column: x => x.ItemId,
                        principalTable: "item",
                        principalColumn: "itemId");
                });

            migrationBuilder.CreateTable(
                name: "itemDefectPhoto",
                columns: table => new
                {
                    itemDefectPhotoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemDefectPhotoContent = table.Column<byte[]>(type: "BLOB", nullable: false),
                    itemDefectPhotoMimeType = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    itemDefectPhotoDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    ncrQaId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrOperationNcrOpId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefectPhoto_itemDefectPhotoId", x => x.itemDefectPhotoId);
                    table.ForeignKey(
                        name: "FK_itemDefectPhoto_NcrOperations_NcrOperationNcrOpId",
                        column: x => x.NcrOperationNcrOpId,
                        principalTable: "NcrOperations",
                        principalColumn: "NcrOpId");
                    table.ForeignKey(
                        name: "fk_itemDefectPhoto_itemDefect",
                        column: x => x.ncrQaId,
                        principalTable: "ncrQA",
                        principalColumn: "NcrQaId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_drawing_ncrEngId",
                table: "drawing",
                column: "ncrEngId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_itemNumber",
                table: "item",
                column: "itemNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_item_supplierId",
                table: "item",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefect_defectId",
                table: "itemDefect",
                column: "defectId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectPhoto_NcrOperationNcrOpId",
                table: "itemDefectPhoto",
                column: "NcrOperationNcrOpId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectPhoto_ncrQaId",
                table: "itemDefectPhoto",
                column: "ncrQaId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_EngDispositionTypeId",
                table: "ncrEng",
                column: "EngDispositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_NcrId",
                table: "ncrEng",
                column: "NcrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NcrOperations_FollowUpTypeId",
                table: "NcrOperations",
                column: "FollowUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NcrOperations_NcrEngId",
                table: "NcrOperations",
                column: "NcrEngId");

            migrationBuilder.CreateIndex(
                name: "IX_NcrOperations_NcrId",
                table: "NcrOperations",
                column: "NcrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NcrOperations_OpDispositionTypeId",
                table: "NcrOperations",
                column: "OpDispositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NcrProcurements_NcrId",
                table: "NcrProcurements",
                column: "NcrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_DefectId",
                table: "ncrQA",
                column: "DefectId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_ItemId",
                table: "ncrQA",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_NcrId",
                table: "ncrQA",
                column: "NcrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ncrReInspect_ncrId",
                table: "ncrReInspect",
                column: "ncrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplier_supplierCode",
                table: "supplier",
                column: "supplierCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplierReturn_ncrProcurementId",
                table: "supplierReturn",
                column: "ncrProcurementId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drawing");

            migrationBuilder.DropTable(
                name: "itemDefect");

            migrationBuilder.DropTable(
                name: "itemDefectPhoto");

            migrationBuilder.DropTable(
                name: "ncrReInspect");

            migrationBuilder.DropTable(
                name: "supplierReturn");

            migrationBuilder.DropTable(
                name: "NcrOperations");

            migrationBuilder.DropTable(
                name: "ncrQA");

            migrationBuilder.DropTable(
                name: "NcrProcurements");

            migrationBuilder.DropTable(
                name: "followUpType");

            migrationBuilder.DropTable(
                name: "ncrEng");

            migrationBuilder.DropTable(
                name: "opDispositionType");

            migrationBuilder.DropTable(
                name: "defect");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "ncr");

            migrationBuilder.DropTable(
                name: "engDispositionType");

            migrationBuilder.DropTable(
                name: "supplier");
        }
    }
}
