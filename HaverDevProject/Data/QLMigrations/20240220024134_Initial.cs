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
                    followUpTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    followUpTypeName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_followUpType_followUpTypeId", x => x.followUpTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ncr",
                columns: table => new
                {
                    NcrId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NcrNumber = table.Column<string>(type: "TEXT", nullable: true),
                    NcrLastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NcrStatus = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncr_ncrId", x => x.NcrId);
                });

            migrationBuilder.CreateTable(
                name: "opDispositionType",
                columns: table => new
                {
                    opDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    opDispositionTypeName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opDispositionType_opDispositionTypeId", x => x.opDispositionTypeId);
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
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrEng_ncrEngId", x => x.ncrEngId);
                    table.ForeignKey(
                        name: "FK_ncrEng_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
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
                name: "ncrPurchasing",
                columns: table => new
                {
                    ncrPurchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrPurchasingDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    ncrPurchasingUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    opDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrPurchasing_ncrPurchId", x => x.ncrPurchId);
                    table.ForeignKey(
                        name: "FK_ncrPurchasing_ncr_ncrId",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "NcrId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ncrPurchasing_opDispositionType",
                        column: x => x.opDispositionTypeId,
                        principalTable: "opDispositionType",
                        principalColumn: "opDispositionTypeId");
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
                name: "car",
                columns: table => new
                {
                    carId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    carNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrPurchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car_carId", x => x.carId);
                    table.ForeignKey(
                        name: "FK_car_ncrPurchasing_ncrPurchId",
                        column: x => x.ncrPurchId,
                        principalTable: "ncrPurchasing",
                        principalColumn: "ncrPurchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followUp",
                columns: table => new
                {
                    followUpId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    followUpExpectedDate = table.Column<DateTime>(type: "date", nullable: false),
                    followUpTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrPurchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_followUp_followUpId", x => x.followUpId);
                    table.ForeignKey(
                        name: "FK_followUp_ncrPurchasing_ncrPurchId",
                        column: x => x.ncrPurchId,
                        principalTable: "ncrPurchasing",
                        principalColumn: "ncrPurchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_followUp_followUpType",
                        column: x => x.followUpTypeId,
                        principalTable: "followUpType",
                        principalColumn: "followUpTypeId");
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
                    reInspectPhotoId = table.Column<int>(type: "INTEGER", nullable: false),
                    reInspectPhotoContent = table.Column<byte[]>(type: "BLOB", nullable: false),
                    reInspectPhotoMimeType = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    reInspectPhotoDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    reInspectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefectPhoto_itemDefectPhotoId", x => x.itemDefectPhotoId);
                    table.ForeignKey(
                        name: "fk_itemDefectPhoto_itemDefect",
                        column: x => x.ncrQaId,
                        principalTable: "ncrQA",
                        principalColumn: "NcrQaId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_car_ncrPurchId",
                table: "car",
                column: "ncrPurchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_drawing_ncrEngId",
                table: "drawing",
                column: "ncrEngId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_followUp_followUpTypeId",
                table: "followUp",
                column: "followUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_followUp_ncrPurchId",
                table: "followUp",
                column: "ncrPurchId",
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
                name: "IX_itemDefectPhoto_ncrQaId",
                table: "itemDefectPhoto",
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
                name: "IX_ncrProcurement_ncrId",
                table: "ncrProcurement",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrPurchasing_ncrId",
                table: "ncrPurchasing",
                column: "ncrId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ncrPurchasing_opDispositionTypeId",
                table: "ncrPurchasing",
                column: "opDispositionTypeId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "car");

            migrationBuilder.DropTable(
                name: "drawing");

            migrationBuilder.DropTable(
                name: "followUp");

            migrationBuilder.DropTable(
                name: "itemDefect");

            migrationBuilder.DropTable(
                name: "itemDefectPhoto");

            migrationBuilder.DropTable(
                name: "ncrProcurement");

            migrationBuilder.DropTable(
                name: "ncrReInspect");

            migrationBuilder.DropTable(
                name: "ncrEng");

            migrationBuilder.DropTable(
                name: "ncrPurchasing");

            migrationBuilder.DropTable(
                name: "followUpType");

            migrationBuilder.DropTable(
                name: "ncrQA");

            migrationBuilder.DropTable(
                name: "engDispositionType");

            migrationBuilder.DropTable(
                name: "opDispositionType");

            migrationBuilder.DropTable(
                name: "defect");

            migrationBuilder.DropTable(
                name: "ncr");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "supplier");
        }
    }
}
