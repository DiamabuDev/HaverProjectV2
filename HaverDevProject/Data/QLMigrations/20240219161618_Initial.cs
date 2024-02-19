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
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrNumber = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: false),
                    ncrLastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ncrStatus = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncr_ncrId", x => x.ncrId);
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
                    ncrEngId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrEngCustomerNotification = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrEngDispositionDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    ncrEngUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    engDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrEng_ncrEngId", x => x.ncrEngId);
                    table.ForeignKey(
                        name: "FK_ncrEng_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrEng_engDispositionType",
                        column: x => x.engDispositionTypeId,
                        principalTable: "engDispositionType",
                        principalColumn: "engDispositionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "ncrProcurement",
                columns: table => new
                {
                    ncrProcurementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrProcSupplierReturnReq = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrProcExpectedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ncrProcDisposedAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrProcSAPReturnCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrProcCreditExpected = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrProcSupplierBilled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrProcurement_ncrProcurementId", x => x.ncrProcurementId);
                    table.ForeignKey(
                        name: "FK_ncrProcurement_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId",
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
                        principalColumn: "ncrId",
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
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                        name: "FK_NcrOperations_ncr_NcrId",
                        column: x => x.NcrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrOperation_opDispositionType",
                        column: x => x.OpDispositionTypeId,
                        principalTable: "opDispositionType",
                        principalColumn: "OpDispositionTypeId");
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
                        principalColumn: "ncrEngId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "itemDefect",
                columns: table => new
                {
                    itemDefectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemId = table.Column<int>(type: "INTEGER", nullable: false),
                    defectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefect_itemDefectId", x => x.itemDefectId);
                    table.ForeignKey(
                        name: "fk_itemDefect_defect",
                        column: x => x.defectId,
                        principalTable: "defect",
                        principalColumn: "defectId");
                    table.ForeignKey(
                        name: "fk_itemDefect_item",
                        column: x => x.itemId,
                        principalTable: "item",
                        principalColumn: "itemId");
                });

            migrationBuilder.CreateTable(
                name: "ncrQA",
                columns: table => new
                {
                    ncrQAId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrQaItemMarNonConforming = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrQAProcessApplicable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrQACreationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ncrQaOrderNumber = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    ncrQaSalesOrder = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    ncrQaQuanReceived = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrQaQuanDefective = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrQaDescriptionOfDefect = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ncrQAUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrQaEngDispositionRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false),
                    itemDefectId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefectId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrQA_ncrQAId", x => x.ncrQAId);
                    table.ForeignKey(
                        name: "FK_ncrQA_itemDefect_DefectId",
                        column: x => x.DefectId,
                        principalTable: "itemDefect",
                        principalColumn: "itemDefectId");
                    table.ForeignKey(
                        name: "FK_ncrQA_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "ncrQAId");
                });

            migrationBuilder.CreateTable(
                name: "itemDefectVideo",
                columns: table => new
                {
                    itemDefectVideoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemDefectVideoLink = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    ncrQaId = table.Column<int>(type: "INTEGER", nullable: false),
                    NcrOperationNcrOpId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefectVideo", x => x.itemDefectVideoId);
                    table.ForeignKey(
                        name: "FK_itemDefectVideo_NcrOperations_NcrOperationNcrOpId",
                        column: x => x.NcrOperationNcrOpId,
                        principalTable: "NcrOperations",
                        principalColumn: "NcrOpId");
                    table.ForeignKey(
                        name: "fk_itemDefectVideo_itemDefect",
                        column: x => x.ncrQaId,
                        principalTable: "ncrQA",
                        principalColumn: "ncrQAId");
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
                name: "IX_itemDefect_itemId",
                table: "itemDefect",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectPhoto_NcrOperationNcrOpId",
                table: "itemDefectPhoto",
                column: "NcrOperationNcrOpId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectPhoto_ncrQaId",
                table: "itemDefectPhoto",
                column: "ncrQaId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectVideo_NcrOperationNcrOpId",
                table: "itemDefectVideo",
                column: "NcrOperationNcrOpId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectVideo_ncrQaId",
                table: "itemDefectVideo",
                column: "ncrQaId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_engDispositionTypeId",
                table: "ncrEng",
                column: "engDispositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_ncrId",
                table: "ncrEng",
                column: "ncrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NcrOperations_FollowUpTypeId",
                table: "NcrOperations",
                column: "FollowUpTypeId");

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
                name: "IX_ncrProcurement_ncrId",
                table: "ncrProcurement",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_DefectId",
                table: "ncrQA",
                column: "DefectId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_ncrId",
                table: "ncrQA",
                column: "ncrId",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drawing");

            migrationBuilder.DropTable(
                name: "itemDefectPhoto");

            migrationBuilder.DropTable(
                name: "itemDefectVideo");

            migrationBuilder.DropTable(
                name: "ncrProcurement");

            migrationBuilder.DropTable(
                name: "ncrReInspect");

            migrationBuilder.DropTable(
                name: "ncrEng");

            migrationBuilder.DropTable(
                name: "NcrOperations");

            migrationBuilder.DropTable(
                name: "ncrQA");

            migrationBuilder.DropTable(
                name: "engDispositionType");

            migrationBuilder.DropTable(
                name: "followUpType");

            migrationBuilder.DropTable(
                name: "opDispositionType");

            migrationBuilder.DropTable(
                name: "itemDefect");

            migrationBuilder.DropTable(
                name: "ncr");

            migrationBuilder.DropTable(
                name: "defect");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "supplier");
        }
    }
}
