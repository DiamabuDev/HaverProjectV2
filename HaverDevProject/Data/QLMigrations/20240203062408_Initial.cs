﻿using System;
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
                name: "processApplicable",
                columns: table => new
                {
                    proAppId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    proAppName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_processApplicable_proAppId", x => x.proAppId);
                });

            migrationBuilder.CreateTable(
                name: "statusUpdate",
                columns: table => new
                {
                    statusUpdateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    statusUpdateName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_status_statusUpdateId", x => x.statusUpdateId);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplierId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    supplierCode = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    supplierName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    supplierEmail = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier_supplierId", x => x.supplierId);
                });

            migrationBuilder.CreateTable(
                name: "ncr",
                columns: table => new
                {
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrNumber = table.Column<string>(type: "TEXT", unicode: false, maxLength: 10, nullable: false),
                    ncrLastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    statusUpdateId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncr_ncrId", x => x.ncrId);
                    table.ForeignKey(
                        name: "fk_ncr_statusUpdate",
                        column: x => x.statusUpdateId,
                        principalTable: "statusUpdate",
                        principalColumn: "statusUpdateId");
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
                name: "ncrEng",
                columns: table => new
                {
                    ncrEngId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrEngCustomerNotification = table.Column<bool>(type: "INTEGER", nullable: true),
                    ncrEngDispositionDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    ncrEngLastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ncrEngCreationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ncrEngUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    engDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrEng_ncrEngId", x => x.ncrEngId);
                    table.ForeignKey(
                        name: "fk_ncrEng_engDispositionType",
                        column: x => x.engDispositionTypeId,
                        principalTable: "engDispositionType",
                        principalColumn: "engDispositionTypeId");
                    table.ForeignKey(
                        name: "fk_ncrEng_ncr",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId");
                });

            migrationBuilder.CreateTable(
                name: "ncrPurchasing",
                columns: table => new
                {
                    ncrPurchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrPurchasingDescription = table.Column<string>(type: "TEXT", unicode: false, maxLength: 300, nullable: true),
                    ncrPurchCreationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ncrPurchasingLastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ncrPurchasingUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    opDispositionTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrPurchasing_ncrPurchId", x => x.ncrPurchId);
                    table.ForeignKey(
                        name: "fk_ncrPurchasing_ncr",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId");
                    table.ForeignKey(
                        name: "fk_ncrPurchasing_opDispositionType",
                        column: x => x.opDispositionTypeId,
                        principalTable: "opDispositionType",
                        principalColumn: "opDispositionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "ncrQA",
                columns: table => new
                {
                    ncrQAId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrQAItemMarNonConforming = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrQASalesOrder = table.Column<string>(type: "TEXT", unicode: false, maxLength: 45, nullable: false),
                    ncrQACreationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ncrQALastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ncrQAUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    proAppId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrQA_ncrQAId", x => x.ncrQAId);
                    table.ForeignKey(
                        name: "fk_ncrQA_ncr",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId");
                    table.ForeignKey(
                        name: "fk_ncrQA_processApplicable",
                        column: x => x.proAppId,
                        principalTable: "processApplicable",
                        principalColumn: "proAppId");
                });

            migrationBuilder.CreateTable(
                name: "ncrReInspect",
                columns: table => new
                {
                    ncrReInspectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ncrReInspectAcceptable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ncrReInspectNewNcrNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    ncrReInspectCreationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ncrReInspectLastUpdated = table.Column<DateTime>(type: "datetime", nullable: false),
                    ncrReInspectUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ncrReInspect_ncrReInspectId", x => x.ncrReInspectId);
                    table.ForeignKey(
                        name: "fk_ncrReInspect_ncr",
                        column: x => x.ncrId,
                        principalTable: "ncr",
                        principalColumn: "ncrId");
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
                        name: "fk_drawing_ncrEng",
                        column: x => x.ncrEngId,
                        principalTable: "ncrEng",
                        principalColumn: "ncrEngId");
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
                        name: "fk_car_ncrPurchasing",
                        column: x => x.ncrPurchId,
                        principalTable: "ncrPurchasing",
                        principalColumn: "ncrPurchId");
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
                        name: "fk_followUp_followUpType",
                        column: x => x.followUpTypeId,
                        principalTable: "followUpType",
                        principalColumn: "followUpTypeId");
                    table.ForeignKey(
                        name: "fk_followUp_ncrPurchasing",
                        column: x => x.ncrPurchId,
                        principalTable: "ncrPurchasing",
                        principalColumn: "ncrPurchId");
                });

            migrationBuilder.CreateTable(
                name: "orderDetail",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    orderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    orderQuanReceived = table.Column<int>(type: "INTEGER", nullable: false),
                    orderQuanDefective = table.Column<int>(type: "INTEGER", nullable: false),
                    itemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ncrQAId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orderDetail_orderId", x => x.orderId);
                    table.ForeignKey(
                        name: "fk_orderDetail_item",
                        column: x => x.itemId,
                        principalTable: "item",
                        principalColumn: "itemId");
                    table.ForeignKey(
                        name: "fk_orderDetail_ncrQA",
                        column: x => x.ncrQAId,
                        principalTable: "ncrQA",
                        principalColumn: "ncrQAId");
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
                    itemDefectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefectPhoto_itemDefectPhotoId", x => x.itemDefectPhotoId);
                    table.ForeignKey(
                        name: "fk_itemDefectPhoto_itemDefect",
                        column: x => x.itemDefectId,
                        principalTable: "itemDefect",
                        principalColumn: "itemDefectId");
                });

            migrationBuilder.CreateTable(
                name: "itemDefectVideo",
                columns: table => new
                {
                    itemDefectVideoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    itemDefectVideoLink = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: false),
                    itemDefectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemDefectVideo", x => x.itemDefectVideoId);
                    table.ForeignKey(
                        name: "fk_itemDefectVideo_itemDefect",
                        column: x => x.itemDefectId,
                        principalTable: "itemDefect",
                        principalColumn: "itemDefectId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_car_ncrPurchId",
                table: "car",
                column: "ncrPurchId");

            migrationBuilder.CreateIndex(
                name: "IX_drawing_ncrEngId",
                table: "drawing",
                column: "ncrEngId");

            migrationBuilder.CreateIndex(
                name: "IX_followUp_followUpTypeId",
                table: "followUp",
                column: "followUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_followUp_ncrPurchId",
                table: "followUp",
                column: "ncrPurchId");

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
                name: "IX_itemDefectPhoto_itemDefectId",
                table: "itemDefectPhoto",
                column: "itemDefectId");

            migrationBuilder.CreateIndex(
                name: "IX_itemDefectVideo_itemDefectId",
                table: "itemDefectVideo",
                column: "itemDefectId");

            migrationBuilder.CreateIndex(
                name: "IX_ncr_statusUpdateId",
                table: "ncr",
                column: "statusUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_engDispositionTypeId",
                table: "ncrEng",
                column: "engDispositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrEng_ncrId",
                table: "ncrEng",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrPurchasing_ncrId",
                table: "ncrPurchasing",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrPurchasing_opDispositionTypeId",
                table: "ncrPurchasing",
                column: "opDispositionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_ncrId",
                table: "ncrQA",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrQA_proAppId",
                table: "ncrQA",
                column: "proAppId");

            migrationBuilder.CreateIndex(
                name: "IX_ncrReInspect_ncrId",
                table: "ncrReInspect",
                column: "ncrId");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetail_itemId",
                table: "orderDetail",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetail_ncrQAId",
                table: "orderDetail",
                column: "ncrQAId");

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
                name: "itemDefectPhoto");

            migrationBuilder.DropTable(
                name: "itemDefectVideo");

            migrationBuilder.DropTable(
                name: "ncrReInspect");

            migrationBuilder.DropTable(
                name: "orderDetail");

            migrationBuilder.DropTable(
                name: "ncrEng");

            migrationBuilder.DropTable(
                name: "followUpType");

            migrationBuilder.DropTable(
                name: "ncrPurchasing");

            migrationBuilder.DropTable(
                name: "itemDefect");

            migrationBuilder.DropTable(
                name: "ncrQA");

            migrationBuilder.DropTable(
                name: "engDispositionType");

            migrationBuilder.DropTable(
                name: "opDispositionType");

            migrationBuilder.DropTable(
                name: "defect");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "ncr");

            migrationBuilder.DropTable(
                name: "processApplicable");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropTable(
                name: "statusUpdate");
        }
    }
}
