using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using HaverDevProject.Models;
using NuGet.DependencyResolver;
using Microsoft.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.Data
{
    public static class HaverInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            HaverNiagaraContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<HaverNiagaraContext>();

            try
            {
                context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //context.Database.Migrate();


                if (!context.Suppliers.Any())
                {
                    context.Suppliers.AddRange(
                        new Supplier
                        {
                            SupplierCode = "000000",
                            SupplierName = "NO SUPPLIER PROVIDED"
                        },
                        new Supplier
                        {
                            SupplierCode = "793254",
                            SupplierName = "INTEGRITY WOVEN WIRE",
                            SupplierEmail = "integritywire@example.com",
                            SupplierStatus = false
                        },
                        new Supplier
                        {
                            SupplierCode = "792356",
                            SupplierName = "FLO COMPONENTS LTD.",
                            SupplierEmail = "flocompltd@example.com",
                            SupplierStatus = false
                        },
                        new Supplier
                        {
                            SupplierCode = "700009",
                            SupplierName = "AJAX TOCCO",
                            SupplierEmail = "ajaxtocco@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700013",
                            SupplierName = "HINGSTON METAL FABRICATORS",
                            SupplierEmail = "hingstonmetal@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700027",
                            SupplierName = "HOTZ ENVIRONMENTAL SERVICES",
                            SupplierEmail = "hotzservices@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700044",
                            SupplierName = "BLACK CREEK METAL",
                            SupplierEmail = "blackcreekmetal@example.com",
                            SupplierStatus = false
                        },
                        new Supplier
                        {
                            SupplierCode = "700045",
                            SupplierName = "POLYMER EXTRUSIONS INC",
                            SupplierEmail = "lsm@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700087",
                            SupplierName = "DON CASSELMAN & SON LTD",
                            SupplierEmail = "fastenal@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "880006",
                            SupplierName = "W S TYLER - PARTICLE & FINE",
                            SupplierEmail = "wstyler@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "790891",
                            SupplierName = "LAWRENCE SINTERED METALS",
                            SupplierEmail = "lsm@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700493",
                            SupplierName = "FASTENAL COMPANY",
                            SupplierEmail = "fastenal@example.com"
                        },

                        new Supplier
                        {
                            SupplierCode = "880065",
                            SupplierName = "HBC ENGINEERING",
                            SupplierEmail = "hbc@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700502",
                            SupplierName = "ST CATHARINES PATTERN LTD",
                            SupplierEmail = "stcathpattern@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700505",
                            SupplierName = "NIAGARA PRECISION LTD",
                            SupplierEmail = "niagaraprecision@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "700508",
                            SupplierName = "BORDER CITY CASTINGS",
                            SupplierEmail = "bordercity@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "888888",
                            SupplierName = "HBC PROCUREMENT",
                            SupplierEmail = "hbcprocurement@example.com",
                        },
                        new Supplier
                        {
                            SupplierCode = "792679",
                            SupplierName = "IFM EFFECTOR CANADA INC.",
                            SupplierEmail = "ifm@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "792565",
                            SupplierName = "PLAS-TECH DESIGN FABRICATION DISTRI",
                            SupplierEmail = "plastech@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "792493",
                            SupplierName = "SGF-SUDDEUTSCHE GELENKSCHEIBENFABRI",
                            SupplierEmail = "sgfsudd@example.com"
                        },
                        new Supplier
                        {
                            SupplierCode = "792011",
                            SupplierName = "VANDER WEYDEN CONSTRUCTION",
                            SupplierEmail = "vander@example.com"
                        });

                    context.SaveChanges();
                }

                if (!context.Items.Any())
                {
                    context.Items.AddRange(
                        new Item
                        {
                            ItemNumber = 10342455,
                            ItemName = "Bearing Housing",
                            ItemDescription = "Protective enclosure for bearings.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "INTEGRITY WOVEN WIRE").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 10482834,
                            ItemName = "Backing Shield",
                            ItemDescription = "Shield for internal components.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "FLO COMPONENTS LTD.").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11536261,
                            ItemName = "Side Arm",
                            ItemDescription = "Structural component for reinforcement.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "AJAX TOCCO").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11854290,
                            ItemName = "Panel",
                            ItemDescription = "Surface to house internal components.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "HINGSTON METAL FABRICATORS").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 10344215,
                            ItemName = "Jig Washer",
                            ItemDescription = "Used to separate heavy and light minerals",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "HOTZ ENVIRONMENTAL SERVICES").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 10482863,
                            ItemName = "Screen",
                            ItemDescription = "Flat stationary for grading",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "BLACK CREEK METAL").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11536287,
                            ItemName = "Conveyor",
                            ItemDescription = "Belt and roller transportation system",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "POLYMER EXTRUSIONS INC").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11854266,
                            ItemName = "Rotary Kiln",
                            ItemDescription = "Cyclindrical vessel to process feedstock",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "DON CASSELMAN & SON LTD").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 10344216,
                            ItemName = "Liner",
                            ItemDescription = "Sleeve designed to withstand corrosion",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "W S TYLER - PARTICLE & FINE").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 10482864,
                            ItemName = "Spherical Roller Bearing",
                            ItemDescription = "Low friction angular misalignment",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "LAWRENCE SINTERED METALS").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11536288,
                            ItemName = "Cyconical Bore",
                            ItemDescription = "Cylinder component for reinforcement.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "FASTENAL COMPANY").SupplierId
                        },
                        new Item
                        {
                            ItemNumber = 11854267,
                            ItemName = "Spherical Roller Washer",
                            ItemDescription = "Spherical rolling element and a sliding coated Haver-Bush.",
                            SupplierId = context.Suppliers.FirstOrDefault(f => f.SupplierName == "HBC ENGINEERING").SupplierId
                        });

                    context.SaveChanges();
                }

                if (!context.Defects.Any())
                {
                    context.Defects.AddRange(
                        new Defect
                        {
                            DefectName = "Design Error(Drawing)",
                            DefectDesription = "Incorrect hardware was delivered due to a design error in the drawing."
                        },
                        new Defect
                        {
                            DefectName = "Poor Paint finish",
                            DefectDesription = "The delivered item has an unsatisfactory paint finish."
                        }, new Defect
                        {
                            DefectName = "Poor quality surface finish",
                            DefectDesription = "The surface finish of the delivered item does not meet quality standards."
                        },
                        new Defect
                        {
                            DefectName = "Poor Weld quality",
                            DefectDesription = "The welding quality of the delivered item is subpar."
                        },
                        new Defect
                        {
                            DefectName = "Missing Items",
                            DefectDesription = "Some components or items are missing from the delivery."
                        },
                        new Defect
                        {
                            DefectName = "Broken / Twisted Wires",
                            DefectDesription = "The wires in the delivered item are broken or twisted."
                        },
                        new Defect
                        {
                            DefectName = "Out of Crimp",
                            DefectDesription = "The crimping of connectors or terminals is not within acceptable limits."
                        },
                        new Defect
                        {
                            DefectName = "Incorrect Center Hole Punching",
                            DefectDesription = "The center hole punching in the delivered item is inaccurate."
                        },
                        //Holes not tapped
                        //Incorrect hole(size, locations, missing)
                        //Incorrect thread size
                        //Not Painted
                        //Incorrect fit
                        //Incorrect weld size
                        //Incorrect Hook / Hook Orientation
                        //Incorrect Center Hole Punching
                        //Missing Center Hole Punching
                        //Incorrect labelling
                        //Drawing not updated
                        //Incorrect material
                        //Finishing error(M.W.STC)
                        //Incorrect component(FMP package)
                        new Defect
                        {
                            DefectName = "Incorrect hardware",
                            DefectDesription = "Incorrect hardware was delivered."
                        },
                        new Defect
                        {
                            DefectName = "Delivery quality",
                            DefectDesription = "Quality of hardware was poor."
                        },
                        new Defect
                        {
                            DefectName = "Incorrect specification",
                            DefectDesription = "Hardware did not match the specifications."
                        },
                        new Defect
                        {
                            DefectName = "Incorrect dimensions",
                            DefectDesription = "Hardware has improper dimensions."
                        });

                    context.SaveChanges();
                }

                if (!context.ItemDefects.Any())
                {
                    context.ItemDefects.AddRange(
                        new ItemDefect
                        {                            
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Delivery quality").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect dimensions").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Backing Shield").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect hardware").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect specification").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect dimensions").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Delivery quality").DefectId
                        },
                        new ItemDefect
                        {
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Panel").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect dimensions").DefectId
                        });

                    context.SaveChanges();
                }                               

                if (!context.Ncrs.Any())
                {
                    context.Ncrs.AddRange(
                        new Ncr
                        {
                            NcrNumber = "2023-137",
                            NcrLastUpdated = DateTime.Parse("2023-12-18"),   
                            NcrStatus = false,
                            NcrPhase = NcrPhase.Closed
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-138",
                            NcrLastUpdated = DateTime.Parse("2023-12-19"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.ReInspection
                            
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-139",
                            NcrLastUpdated = DateTime.Parse("2023-12-22"),
                            NcrStatus = false,
                            NcrPhase = NcrPhase.Closed
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-140",
                            NcrLastUpdated = DateTime.Parse("2024-01-18"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.ReInspection
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-141",
                            NcrLastUpdated = DateTime.Parse("2024-01-14"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.ReInspection
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-001",
                            NcrLastUpdated = DateTime.Parse("2024-01-10"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Procurement
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-002",
                            NcrLastUpdated = DateTime.Parse("2024-01-11"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Procurement
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-003",
                            NcrLastUpdated = DateTime.Parse("2024-01-15"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Operations
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-004",
                            NcrLastUpdated = DateTime.Parse("2024-01-19"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Operations
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-005",
                            NcrLastUpdated = DateTime.Parse("2024-01-22"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Engineer
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-006",
                            NcrLastUpdated = DateTime.Parse("2024-01-23"),
                            NcrStatus = false,
                            NcrPhase = NcrPhase.Closed
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-007",
                            NcrLastUpdated = DateTime.Parse("2024-01-23"),
                            NcrStatus = true,
                            NcrPhase = NcrPhase.Engineer
                        });
                    context.SaveChanges();
                }

                if (!context.NcrQas.Any())
                {
                    context.NcrQas.AddRange(
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401227",
                            NcrQacreationDate = DateTime.Parse("2023-12-07"), 
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId,
                            NcrQaProcessApplicable = false,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Delivery quality").DefectId,
                            NcrQaOrderNumber = "4500695162",
                            NcrQaQuanReceived = 10,
                            NcrQaQuanDefective = 8,
                            NcrQaDescriptionOfDefect = "Example description 1",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401228",
                            NcrQacreationDate = DateTime.Parse("2023-12-09"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect dimensions").DefectId,
                            NcrQaOrderNumber = "4500695429",
                            NcrQaQuanReceived = 5,
                            NcrQaQuanDefective = 5,
                            NcrQaDescriptionOfDefect = "Example description 2",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401229",
                            NcrQacreationDate = DateTime.Parse("2023-12-11"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect dimensions").DefectId,
                            NcrQaProcessApplicable = false,
                            NcrQaOrderNumber = "4500684525",
                            NcrQaQuanReceived = 12,
                            NcrQaQuanDefective = 3,
                            NcrQaDescriptionOfDefect = "Example description 3",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401230",
                            NcrQacreationDate = DateTime.Parse("2023-12-13"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Backing Shield").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect hardware").DefectId,
                            NcrQaOrderNumber = "4500683983",
                            NcrQaQuanReceived = 28,
                            NcrQaQuanDefective = 14,
                            NcrQaDescriptionOfDefect = "Example description 4",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401231",
                            NcrQacreationDate = DateTime.Parse("2023-12-17"),
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect specification").DefectId,
                            NcrQaOrderNumber = "4500694121",
                            NcrQaQuanReceived = 2,
                            NcrQaQuanDefective = 2,
                            NcrQaDescriptionOfDefect = "Example description 5",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401232",
                            NcrQacreationDate = DateTime.Parse("2024-01-03"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Backing Shield").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect hardware").DefectId,
                            NcrQaProcessApplicable = false,
                            NcrQaOrderNumber = "4500681790",
                            NcrQaQuanReceived = 1,
                            NcrQaQuanDefective = 1,
                            NcrQaDescriptionOfDefect = "Example description 6",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401233",
                            NcrQacreationDate = DateTime.Parse("2024-01-04"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Delivery quality").DefectId,
                            NcrQaOrderNumber = "4500671162",
                            NcrQaQuanReceived = 9,
                            NcrQaQuanDefective = 8,
                            NcrQaDescriptionOfDefect = "Example description 7",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401234",
                            NcrQacreationDate = DateTime.Parse("2024-01-06"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect specification").DefectId,
                            NcrQaOrderNumber = "4500685546",
                            NcrQaQuanReceived = 4,
                            NcrQaQuanDefective = 1,
                            NcrQaDescriptionOfDefect = "Example description 8",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401235",
                            NcrQacreationDate = DateTime.Parse("2024-01-07"),
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect specification").DefectId,
                            NcrQaOrderNumber = "4500683210",
                            NcrQaQuanReceived = 15,
                            NcrQaQuanDefective = 10,
                            NcrQaDescriptionOfDefect = "Example description 9",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401236",
                            NcrQacreationDate = DateTime.Parse("2024-01-11"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Bearing Housing").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Delivery quality").DefectId,
                            NcrQaProcessApplicable = false,
                            NcrQaOrderNumber = "4500700595",
                            NcrQaQuanReceived = 17,
                            NcrQaQuanDefective = 6,
                            NcrQaDescriptionOfDefect = "Example description 10",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401681",
                            NcrQacreationDate = DateTime.Parse("2024-01-14"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Side Arm").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect specification").DefectId,
                            NcrQaOrderNumber = "4500695645",
                            NcrQaQuanReceived = 12,
                            NcrQaQuanDefective = 2,
                            NcrQaDescriptionOfDefect = "Example description 11",
                            NcrQaEngDispositionRequired = true
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaSalesOrder = "10401682",
                            NcrQacreationDate = DateTime.Parse("2024-01-14"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId,
                            ItemId = context.Items.FirstOrDefault(f => f.ItemName == "Backing Shield").ItemId,
                            DefectId = context.Defects.FirstOrDefault(f => f.DefectName == "Incorrect hardware").DefectId,
                            NcrQaOrderNumber = "4500691574",
                            NcrQaQuanReceived = 24,
                            NcrQaQuanDefective = 6,
                            NcrQaDescriptionOfDefect = "Example description 12",
                            NcrQaEngDispositionRequired = true
                        });
                    context.SaveChanges();
                }                

                if (!context.EngDispositionTypes.Any())
                {
                    context.EngDispositionTypes.AddRange(
                        new EngDispositionType
                        {
                            EngDispositionTypeName = "Use As Is"
                        },
                        new EngDispositionType
                        {
                            EngDispositionTypeName = "Repair"
                        }, new EngDispositionType
                        {
                            EngDispositionTypeName = "Rework"
                        },
                        new EngDispositionType
                        {
                            EngDispositionTypeName = "Scrap"
                        });

                    context.SaveChanges();
                }

                if (!context.NcrEngs.Any())
                {
                    context.NcrEngs.AddRange(
                        new NcrEng
                        {
                            NcrEngCustomerNotification = false,
                            NcrEngDispositionDescription = "N/A",
                            NcrEngUserId = 1, //nullable
                            NcrEngCreationDate = DateTime.Parse("2023-12-18"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Use As Is").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 2",
                            NcrEngUserId = 2, //nullable
                            NcrEngCreationDate = DateTime.Parse("2023-12-19"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Repair").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 3",
                            NcrEngUserId = 3, //nullable
                            NcrEngCreationDate = DateTime.Parse("2023-12-23"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "N/A",
                            NcrEngUserId = 4, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-18"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = false,
                            NcrEngDispositionDescription = "N/A",
                            NcrEngUserId = 1, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-14"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Use As Is").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "N/A",
                            NcrEngUserId = 2, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-11"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 7",
                            NcrEngUserId = 3, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-11"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 8",
                            NcrEngUserId = 4, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-15"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = false,
                            NcrEngDispositionDescription = "Example Description of Disposition 9",
                            NcrEngUserId = 1, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-22"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Repair").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        },
                        //new NcrEng
                        //{
                        //    NcrEngCustomerNotification = true,
                        //    NcrEngDispositionDescription = "N/A",
                        //    NcrEngUserId = 2, //nullable
                        //    EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        //},
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "N/A",
                            NcrEngUserId = 3, //nullable
                            NcrEngCreationDate = DateTime.Parse("2024-01-23"),
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        }
                        //new NcrEng
                        //{
                        //    NcrEngCustomerNotification = true,
                        //    NcrEngDispositionDescription = "Example Description of Disposition 12",
                        //    NcrEngUserId = 4, //nullable
                        //    EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        //}
                        );
                    context.SaveChanges();
                }

                if (!context.Drawings.Any())
                {
                    context.Drawings.AddRange(
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 1,
                            DrawingUpdatedRevNumber = 2,
                            DrawingRevDate = DateTime.Parse("2023-12-08"),
                            DrawingUserId = 1, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2023-137").NcrEngId //revisar
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 3,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2023-12-10"),
                            DrawingUserId = 2, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2023-138").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 5,
                            DrawingUpdatedRevNumber = 6,
                            DrawingRevDate = DateTime.Parse("2023-12-16"),
                            DrawingUserId = 3, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2023-139").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 7,
                            DrawingUpdatedRevNumber = 8,
                            DrawingRevDate = DateTime.Parse("2023-12-16"),
                            DrawingUserId = 4, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2023-140").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 1,
                            DrawingUpdatedRevNumber = 3,
                            DrawingRevDate = DateTime.Parse("2023-12-19"),
                            DrawingUserId = 1, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2023-141").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 2,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-04"),
                            DrawingUserId = 2, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2024-001").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 3,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-06"),
                            DrawingUserId = 3, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2024-002").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 4,
                            DrawingUpdatedRevNumber = 7,
                            DrawingRevDate = DateTime.Parse("2024-01-07"),
                            DrawingUserId = 4, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2024-003").NcrEngId//THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 1,
                            DrawingUpdatedRevNumber = 3,
                            DrawingRevDate = DateTime.Parse("2024-01-08"),
                            DrawingUserId = 1, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.Ncr.NcrNumber == "2024-004").NcrEngId //THIS MUST CHANGE
                        }
                        //new Drawing
                        //{
                        //    DrawingOriginalRevNumber = 2,
                        //    DrawingUpdatedRevNumber = 4,
                        //    DrawingRevDate = DateTime.Parse("2024-01-12"),
                        //    DrawingUserId = 2, //need to make nullable
                        //    NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 10").NcrEngId //THIS MUST CHANGE
                        //},
                        //new Drawing
                        //{
                        //    DrawingOriginalRevNumber = 3,
                        //    DrawingUpdatedRevNumber = 4,
                        //    DrawingRevDate = DateTime.Parse("2024-01-14"),
                        //    DrawingUserId = 3, //need to make nullable
                        //    NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 11").NcrEngId //THIS MUST CHANGE
                        //},
                        //new Drawing
                        //{
                        //    DrawingOriginalRevNumber = 4,
                        //    DrawingUpdatedRevNumber = 7,
                        //    DrawingRevDate = DateTime.Parse("2024-01-18"),
                        //    DrawingUserId = 4, //need to make nullable
                        //    NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 12").NcrEngId //THIS MUST CHANGE
                        //}
                        );
                    context.SaveChanges();
                }

                if (!context.FollowUpTypes.Any())
                {
                    context.FollowUpTypes.AddRange(
                        new FollowUpType
                        {
                            FollowUpTypeName = "N/A"
                        },
                        new FollowUpType
                        {
                            FollowUpTypeName = "Resolution"
                        },
                        new FollowUpType
                        {
                            FollowUpTypeName = "Clarification"
                        },
                        new FollowUpType
                        {
                            FollowUpTypeName = "Feedback"
                        },
                        new FollowUpType
                        {
                            FollowUpTypeName = "Update"
                        });

                    context.SaveChanges();
                }

                if (!context.OpDispositionTypes.Any())
                {
                    context.OpDispositionTypes.AddRange(
                        new OpDispositionType
                        {
                            OpDispositionTypeName = "Rework 'In-House'"
                        },
                        new OpDispositionType
                        {
                            OpDispositionTypeName = "Scrap in House"
                        },
                        new OpDispositionType
                        {
                            OpDispositionTypeName = "Return to Supplier for either 'rework' or 'replace'"
                        },
                        new OpDispositionType
                        {
                            OpDispositionTypeName = "Defer for HBC Engineering Review"
                        });

                    context.SaveChanges();
                }

                if (!context.NcrOperations.Any())
                {
                    context.NcrOperations.AddRange(
                        new NcrOperation
                        {
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId,
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                            NcrPurchasingDescription = "Replacement required",
                            Car = true,
                            CarNumber = "3456",
                            FollowUp = true,
                            ExpectedDate = DateTime.Parse("2024-05-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Update").FollowUpTypeId,
                            UpdateOp = DateTime.Parse("2024-01-18"),
                            NcrPurchasingUserId = 1
                            
                        },
                        new NcrOperation
                        {
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId,
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Scrap in House").OpDispositionTypeId,
                            NcrPurchasingDescription = "Replacement required",
                            Car = true,
                            CarNumber = "3456",
                            FollowUp = true,
                            ExpectedDate = DateTime.Parse("2024-05-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Resolution").FollowUpTypeId,
                            UpdateOp = DateTime.Parse("2024-01-19"),
                            NcrPurchasingUserId = 1
                            
                        },
                        new NcrOperation
                        {
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId,
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Defer for HBC Engineering Review").OpDispositionTypeId,
                            NcrPurchasingDescription = "Replacement required",
                            Car = true,
                            CarNumber = "3456",
                            FollowUp = true,
                            ExpectedDate = DateTime.Parse("2024-05-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                            UpdateOp = DateTime.Parse("2024-01-20"),
                            NcrPurchasingUserId = 1
                            
                        },
                        new NcrOperation
                        {
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId,
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                            NcrPurchasingDescription = "Rework per engineering disposition",
                            Car = true,
                            CarNumber = "3456",
                            FollowUp = true,
                            ExpectedDate = DateTime.Parse("2024-05-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                            UpdateOp = DateTime.Parse("2024-01-21"),
                            NcrPurchasingUserId = 1
                        },
                        new NcrOperation
                        {
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId,
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Defer for HBC Engineering Review").OpDispositionTypeId,
                            NcrPurchasingDescription = "Replacement required",
                            Car = true,
                            CarNumber = "3456",
                            FollowUp = true,
                            ExpectedDate = DateTime.Parse("2024-05-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                            UpdateOp = DateTime.Parse("2024-01-20"),
                            NcrPurchasingUserId = 1

                        }
                        //new NcrOperation
                        //{
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId,
                        //    OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Defer for HBC Engineering Review").OpDispositionTypeId,
                        //    NcrPurchasingDescription = "Replacement required",
                        //    Car = true,
                        //    CarNumber = "3456",
                        //    FollowUp = true,
                        //    ExpectedDate = DateTime.Parse("2024-05-18"),
                        //    FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                        //    UpdateOp = DateTime.Parse("2024-01-20"),
                        //    NcrPurchasingUserId = 1

                        //},
                        //new NcrOperation
                        //{
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId,
                        //    OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                        //    NcrPurchasingDescription = "Rework per engineering disposition",
                        //    Car = true,
                        //    CarNumber = "3456",
                        //    FollowUp = true,
                        //    ExpectedDate = DateTime.Parse("2024-05-18"),
                        //    FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                        //    UpdateOp = DateTime.Parse("2024-01-21"),
                        //    NcrPurchasingUserId = 1
                        //},
                        //new NcrOperation
                        //{
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId,
                        //    OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                        //    NcrPurchasingDescription = "Rework per engineering disposition",
                        //    Car = true,
                        //    CarNumber = "3456",
                        //    FollowUp = true,
                        //    ExpectedDate = DateTime.Parse("2024-05-18"),
                        //    FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                        //    UpdateOp = DateTime.Parse("2024-01-21"),
                        //    NcrPurchasingUserId = 1
                        //}
                        );
                    context.SaveChanges();
                }

                if (!context.NcrReInspects.Any())
                {
                    context.NcrReInspects.AddRange(
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectUserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId,
                            NcrReInspectCreationDate = DateTime.Parse("2023-12-07")
                        },
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 2, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        //},
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectUserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId,
                            NcrReInspectCreationDate = DateTime.Parse("2023-12-11")
                        },
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 4, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 1, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId,
                        //    NcrReInspectCreationDate = DateTime.Parse("2023-12-17")
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 2, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 3, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 4, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 1, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        //},
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectUserId = 2, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        //},
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectUserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId,
                            NcrReInspectCreationDate = DateTime.Parse("2024-01-14")
                        }//,
                        //new NcrReInspect
                        //{
                        //    NcrReInspectAcceptable = true,
                        //    NcrReInspectNewNcrNumber = null,
                        //    NcrReInspectUserId = 4, //need to make nullable
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        //}
                        );
                    context.SaveChanges();
                }
                if (!context.NcrProcurements.Any())
                {
                    context.NcrProcurements.AddRange(
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2023-12-07"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 1, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2023-12-18"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2023-12-19"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 2, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2023-12-19"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2023-12-23"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 3, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2023-12-23"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2024-01-18"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 4, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2024-01-18"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2024-01-10"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 1, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2024-01-14"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-01-13"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 2, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        //},
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-01-17"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 3, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        //},
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-01-20"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 4, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        //},
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-01-22"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 1, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        //},
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-01-25"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 2, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        //},
                        new NcrProcurement
                        {
                            NcrProcSupplierReturnReq = true,
                            NcrProcExpectedDate = DateTime.Parse("2024-01-30"),
                            NcrProcDisposedAllowed = false,
                            NcrProcSAPReturnCompleted = true,
                            NcrProcCreditExpected = true,
                            NcrProcSupplierBilled = true,
                            NcrProcUserId = 3, //need to make nullable
                            NcrProcCreated = DateTime.Parse("2024-01-23"),
                            SupplierReturnMANum = "11345",
                            SupplierReturnName = "FEDEX",
                            SupplierReturnAccount = "12345",
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        }
                        //new NcrProcurement
                        //{
                        //    NcrProcSupplierReturnReq = true,
                        //    NcrProcExpectedDate = DateTime.Parse("2024-02-03"),
                        //    NcrProcDisposedAllowed = false,
                        //    NcrProcSAPReturnCompleted = true,
                        //    NcrProcCreditExpected = true,
                        //    NcrProcSupplierBilled = true,
                        //    NcrProcUserId = 4, //need to make nullable
                        //    NcrProcCreated = DateTime.Parse("2024-02-20"),
                        //    SupplierReturnMANum = "11345",
                        //    SupplierReturnName = "FEDEX",
                        //    SupplierReturnAccount = "12345",
                        //    NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        //}
                        );
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
