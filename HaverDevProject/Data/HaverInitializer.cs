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
                context.Database.EnsureCreated();
                //context.Database.Migrate();


                if (!context.Suppliers.Any())
                {
                    context.Suppliers.AddRange(
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
                            NcrLastUpdated = DateTime.Parse("2023-12-18")
                            
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-138",
                            NcrLastUpdated = DateTime.Parse("2023-12-19")
                           
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-139",
                            NcrLastUpdated = DateTime.Parse("2023-12-22")
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-140",
                            NcrLastUpdated = DateTime.Parse("2024-01-18")
                        },
                        new Ncr
                        {
                            NcrNumber = "2023-141",
                            NcrLastUpdated = DateTime.Parse("2024-01-14")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-001",
                            NcrLastUpdated = DateTime.Parse("2024-01-10")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-002",
                            NcrLastUpdated = DateTime.Parse("2024-01-11")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-003",
                            NcrLastUpdated = DateTime.Parse("2024-01-15")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-004",
                            NcrLastUpdated = DateTime.Parse("2024-01-19")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-005",
                            NcrLastUpdated = DateTime.Parse("2024-01-22")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-006",
                            NcrLastUpdated = DateTime.Parse("2024-01-23")
                        },
                        new Ncr
                        {
                            NcrNumber = "2024-007",
                            NcrLastUpdated = DateTime.Parse("2024-01-23")
                        });
                    context.SaveChanges();
                }

                if (!context.NcrQas.Any())
                {
                    context.NcrQas.AddRange(
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10401227,
                            NcrQacreationDate = DateTime.Parse("2023-12-07"),
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10401228,
                            NcrQacreationDate = DateTime.Parse("2023-12-09"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10401229,
                            NcrQacreationDate = DateTime.Parse("2023-12-11"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10401230,
                            NcrQacreationDate = DateTime.Parse("2023-12-13"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10401231,
                            NcrQacreationDate = DateTime.Parse("2023-12-17"),
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410676,
                            NcrQacreationDate = DateTime.Parse("2024-01-03"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410677,
                            NcrQacreationDate = DateTime.Parse("2024-01-04"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410678,
                            NcrQacreationDate = DateTime.Parse("2024-01-06"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410679,
                            NcrQacreationDate = DateTime.Parse("2024-01-07"),
                            NcrQauserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410680,
                            NcrQacreationDate = DateTime.Parse("2024-01-11"),
                            NcrQauserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410681,
                            NcrQacreationDate = DateTime.Parse("2024-01-14"),
                            NcrQauserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        },
                        new NcrQa
                        {
                            NcrQaItemMarNonConforming = true,
                            NcrQaOrderNumber = 10410682,
                            NcrQacreationDate = DateTime.Parse("2024-01-14"),
                            NcrQauserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
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
                            NcrEngDispositionDescription = "Example Description of Disposition 1",
                            NcrEngUserId = 1, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Use As Is").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 2",
                            NcrEngUserId = 2, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Repair").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 3",
                            NcrEngUserId = 3, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 4",
                            NcrEngUserId = 4, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = false,
                            NcrEngDispositionDescription = "Example Description of Disposition 5",
                            NcrEngUserId = 1, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Use As Is").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 6",
                            NcrEngUserId = 2, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 7",
                            NcrEngUserId = 3, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 8",
                            NcrEngUserId = 4, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = false,
                            NcrEngDispositionDescription = "Example Description of Disposition 9",
                            NcrEngUserId = 1, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Repair").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 10",
                            NcrEngUserId = 2, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 11",
                            NcrEngUserId = 3, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Scrap").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        },
                        new NcrEng
                        {
                            NcrEngCustomerNotification = true,
                            NcrEngDispositionDescription = "Example Description of Disposition 12",
                            NcrEngUserId = 4, //nullable
                            EngDispositionTypeId = context.EngDispositionTypes.FirstOrDefault(f => f.EngDispositionTypeName == "Rework").EngDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        });
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
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 1").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 3,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2023-12-10"),
                            DrawingUserId = 2, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 2").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 5,
                            DrawingUpdatedRevNumber = 6,
                            DrawingRevDate = DateTime.Parse("2023-12-16"),
                            DrawingUserId = 3, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 3").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 7,
                            DrawingUpdatedRevNumber = 8,
                            DrawingRevDate = DateTime.Parse("2023-12-16"),
                            DrawingUserId = 4, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 4").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 1,
                            DrawingUpdatedRevNumber = 3,
                            DrawingRevDate = DateTime.Parse("2023-12-19"),
                            DrawingUserId = 1, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 5").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 2,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-04"),
                            DrawingUserId = 2, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 6").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 3,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-06"),
                            DrawingUserId = 3, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 7").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 4,
                            DrawingUpdatedRevNumber = 7,
                            DrawingRevDate = DateTime.Parse("2024-01-07"),
                            DrawingUserId = 4, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 8").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 1,
                            DrawingUpdatedRevNumber = 3,
                            DrawingRevDate = DateTime.Parse("2024-01-08"),
                            DrawingUserId = 1, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 9").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 2,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-12"),
                            DrawingUserId = 2, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 10").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 3,
                            DrawingUpdatedRevNumber = 4,
                            DrawingRevDate = DateTime.Parse("2024-01-14"),
                            DrawingUserId = 3, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 11").NcrEngId //THIS MUST CHANGE
                        },
                        new Drawing
                        {
                            DrawingOriginalRevNumber = 4,
                            DrawingUpdatedRevNumber = 7,
                            DrawingRevDate = DateTime.Parse("2024-01-18"),
                            DrawingUserId = 4, //need to make nullable
                            NcrEngId = context.NcrEngs.FirstOrDefault(f => f.NcrEngDispositionDescription == "Example Description of Disposition 12").NcrEngId //THIS MUST CHANGE
                        });
                    context.SaveChanges();
                }

                if (!context.FollowUpTypes.Any())
                {
                    context.FollowUpTypes.AddRange(
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

                if (!context.NcrPurchasings.Any())
                {
                    context.NcrPurchasings.AddRange(
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Replacement required",
                            NcrPurchasingUserId = 1, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 2, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Scrap in House").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 3, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Scrap in House").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Rework per engineering disposition",
                            NcrPurchasingUserId = 4, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Return to Supplier for either 'rework' or 'replace'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Replacement required",
                            NcrPurchasingUserId = 5, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 6, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Scrap in House").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 7, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Return to Supplier for either 'rework' or 'replace'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Rework per engineering disposition",
                            NcrPurchasingUserId = 8, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Defer for HBC Engineering Review").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Replacement required",
                            NcrPurchasingUserId = 9, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Rework 'In-House'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 10, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Scrap in House").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Use as is",
                            NcrPurchasingUserId = 11, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Defer for HBC Engineering Review").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        },
                        new NcrPurchasing
                        {
                            NcrPurchasingDescription = "Rework per engineering disposition",
                            NcrPurchasingUserId = 12, //need to make nullable
                            OpDispositionTypeId = context.OpDispositionTypes.FirstOrDefault(f => f.OpDispositionTypeName == "Return to Supplier for either 'rework' or 'replace'").OpDispositionTypeId,
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        });
                    context.SaveChanges();
                }

                if (!context.FollowUps.Any())
                {
                    context.FollowUps.AddRange(
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2023-12-10"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Resolution").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 1).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2023-12-12"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 2).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2023-12-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 3).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2023-12-18"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Update").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 4).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2023-12-21"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Resolution").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 5).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-06"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 6).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-08"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 7).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-09"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Update").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 8).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-10"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Resolution").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 9).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-14"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Clarification").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 10).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-16"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Feedback").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 11).NcrPurchId //THIS MUST CHANGE
                        },
                        new FollowUp
                        {
                            FollowUpExpectedDate = DateTime.Parse("2024-01-20"),
                            FollowUpTypeId = context.FollowUpTypes.FirstOrDefault(f => f.FollowUpTypeName == "Update").FollowUpTypeId,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 12).NcrPurchId //THIS MUST CHANGE
                        });

                    context.SaveChanges();
                }

                if (!context.Cars.Any())
                {
                    context.Cars.AddRange(
                        new Car
                        {
                            CarNumber = 3456,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 1).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 1243,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 2).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 7542,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 3).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 9253,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 4).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 2618,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 5).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 8723,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 6).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 9231,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 7).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 7772,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 8).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 3290,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 9).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 6666,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 10).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 4411,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 11).NcrPurchId //THIS MUST CHANGE
                        },
                        new Car
                        {
                            CarNumber = 1592,
                            NcrPurchId = context.NcrPurchasings.FirstOrDefault(f => f.NcrPurchasingUserId == 12).NcrPurchId //THIS MUST CHANGE
                        });

                    context.SaveChanges();
                }

                if (!context.NcrReInspects.Any())
                {
                    context.NcrReInspects.AddRange(
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-137").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-138").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-139").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-140").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2023-141").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-001").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-002").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-003").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 1, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-004").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 2, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-005").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 3, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-006").NcrId
                        },
                        new NcrReInspect
                        {
                            NcrReInspectAcceptable = true,
                            NcrReInspectNewNcrNumber = null,
                            NcrReInspectUserId = 4, //need to make nullable
                            NcrId = context.Ncrs.FirstOrDefault(f => f.NcrNumber == "2024-007").NcrId
                        });
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
