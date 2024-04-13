using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.Utilities;
using HaverDevProject.CustomControllers;
using HaverDevProject.Services;
using Microsoft.AspNetCore.Authorization;
using Spire.Xls;
using OfficeOpenXml;
using System.Drawing;
using System.IO;
using HaverDevProject.Configurations;

namespace HaverDevProject.CustomControllers
{
    public class ReportController : ElephantController
    {

        private readonly HaverNiagaraContext _context;
        public ReportController(HaverNiagaraContext context)
        {
            _context = context;
        }

        #region DownloadPDF
        public async Task<IActionResult> DownloadPDF(int id)
        {
            var ncr = await _context.Ncrs
                .Include(n => n.NcrQa)
                .Include(n => n.NcrQa).ThenInclude(n => n.Item)
                .Include(n => n.NcrQa).ThenInclude(n => n.Defect)
                .Include(n => n.NcrQa).ThenInclude(n => n.Supplier)
                .Include(n => n.NcrQa).ThenInclude(n => n.ItemDefectPhotos)
                .Include(n => n.NcrEng)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDispositionType)
                .Include(n => n.NcrEng).ThenInclude(n => n.Drawing)
                .Include(n => n.NcrEng).ThenInclude(n => n.EngDefectPhotos)
                .Include(n => n.NcrOperation)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDispositionType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.FollowUpType)
                .Include(n => n.NcrOperation).ThenInclude(n => n.OpDefectPhotos)
                .Include(n => n.NcrProcurement)
                .Include(n => n.NcrProcurement).ThenInclude(n => n.ProcDefectPhotos)
                .Include(n => n.NcrReInspect)
                .Include(n => n.NcrReInspect).ThenInclude(n => n.NcrReInspectPhotos)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NcrId == id);

            try
            {

                var excelFilePath = "ncr-template.xlsx";
                var excelPictureFilePath = "picture-template.xlsx";

                Workbook workbook = new Workbook();
                Workbook workbookPicture = new Workbook();
                workbook.LoadFromFile(excelFilePath);
                workbookPicture.LoadFromFile(excelPictureFilePath);


                //Fill the data 
                var worksheet = workbook.Worksheets["NCR"];
                bool checkpictures = false;
                int firstpage = 1;
                int morepages = 1;
                worksheet.Range["Z4"].Value = firstpage.ToString();
                worksheet.Range["AC4"].Value = morepages.ToString();



                //Quality Representative
                if (ncr.NcrQa != null)
                {

                    worksheet.Range["AC5"].Value = ncr.NcrNumber;
                    worksheet.Range["AC6"].Value = ncr.NcrQa.NcrQaOrderNumber;
                    worksheet.Range["AC7"].Value = ncr.NcrQa.NcrQaSalesOrder;
                    worksheet.Range["M7"].Value = ncr.NcrQa.Supplier.SupplierName;
                    worksheet.Range["AF8"].Value = ncr.NcrQa.NcrQaQuanReceived.ToString();
                    worksheet.Range["AF9"].Value = ncr.NcrQa.NcrQaQuanDefective.ToString();
                    worksheet.Range["B9"].Value = ncr.NcrQa.Item.ItemName;
                    worksheet.Range["B11"].Value = ncr.NcrQa.NcrQaDescriptionOfDefect;
                    //worksheet.Range["U15"].Value = ncr.NcrQa.CreatedBy;
                    worksheet.Range["AE15"].Value = ncr.NcrQa.CreatedOn.ToString();
                    if (ncr.NcrQa.NcrQaProcessApplicable.Equals(true))
                    {
                        worksheet.Range["C6"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["C7"].Value = "X";
                    }
                    if (ncr.NcrQa.NcrQaItemMarNonConforming.Equals(true))
                    {
                        worksheet.Range["C14"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["H14"].Value = "X";
                    }
                    if (ncr.NcrQa.NcrQaEngDispositionRequired.Equals(true))
                    {
                        worksheet.Range["T14"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["Y14"].Value = "X";
                    }
                    if (ncr.NcrQa.ItemDefectPhotos.Count > 0 || ncr.NcrQa.NcrQaDefectVideo != null)
                    {

                        firstpage += 1;
                        morepages += 1;


                        checkpictures = true;
                        //var wb = workbookPicture.Worksheets["Pictures"];
                        var wb = workbookPicture.Worksheets.Add("Quality");
                        workbookPicture.Worksheets["Quality"].CopyFrom(workbookPicture.Worksheets["Pictures"]);
                        wb.Range["A6"].Value = "Quality";
                        int picCount = _context.ItemDefectPhotos.Count();
                        var Pics = _context.ItemDefectPhotos.Take(Math.Min(4, picCount)).ToList();
                        List<ExcelPicture> pictures = new List<ExcelPicture>();
                        for (int i = 0; i < Pics.Count; i++)
                        {
                            byte[] imageBytes = Pics[i].ItemDefectPhotoContent;
                            MemoryStream stream = new MemoryStream(imageBytes);
                            ExcelPicture picture = wb.Pictures.Add(i + 1, 5, stream);
                            picture.Height = 300;
                            picture.Width = 300;
                            pictures.Add(picture);
                        }

                        if (pictures.Count > 0)
                        {
                            pictures[0].LeftColumn = 3;
                            pictures[0].TopRow = 7;
                        }
                        if (pictures.Count > 1)
                        {
                            pictures[1].LeftColumn = 18;
                            pictures[1].TopRow = 7;
                        }
                        if (pictures.Count > 2)
                        {
                            pictures[2].LeftColumn = 3;
                            pictures[2].TopRow = 22;
                        }
                        if (pictures.Count > 3)
                        {
                            pictures[3].LeftColumn = 18;
                            pictures[3].TopRow = 22;
                        }
                        wb.Range["B37"].Value = ncr.NcrQa.NcrQaDefectVideo;
                        wb.Range["Z4"].Value = firstpage.ToString();
                        wb.Range["AE5"].Value = ncr.NcrNumber;

                        worksheet.Range["AC4"].Value = morepages.ToString();
                        //wb.Range["AC4"].Value = worksheet.Range["AC4"].Value;
                    }
                }

                //Engineering
                if (ncr.NcrEng != null)
                {
                    worksheet.Range["B21"].Value = ncr.NcrEng.NcrEngDispositionDescription;
                    //worksheet.Range["T26"].Value = ncr.NcrEng.CreatedBy;
                    worksheet.Range["AD26"].Value = ncr.NcrEng.CreatedOn.ToString();
                    if (ncr.NcrEng.NcrEngCustomerNotification.Equals(true))
                    {
                        worksheet.Range["Q18"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["U18"].Value = "X";
                    }
                    if (ncr.NcrEng.DrawingRequireUpdating.Equals(true))
                    {
                        worksheet.Range["O23"].Value = "X";
                        worksheet.Range["J24"].Value = ncr.NcrEng.DrawingOriginalRevNumber.ToString();
                        worksheet.Range["T24"].Value = ncr.NcrEng.DrawingUpdatedRevNumber.ToString();
                        worksheet.Range["T25"].Value = ncr.NcrEng.DrawingRevDate.ToString();
                    }
                    else
                    {
                        worksheet.Range["T23"].Value = "X";
                    }
                    switch (ncr.NcrEng.EngDispositionType.EngDispositionTypeId)
                    {
                        case 1:
                            worksheet.Range["E17"].Value = "X";
                            break;
                        case 2:
                            worksheet.Range["J17"].Value = "X";
                            break;
                        case 3:
                            worksheet.Range["N17"].Value = "X";
                            break;
                        case 4:
                            worksheet.Range["S17"].Value = "X";
                            break;
                    }
                    if (ncr.NcrEng.EngDefectPhotos.Count > 0 || ncr.NcrEng.NcrEngDefectVideo != null)
                    {

                        firstpage += 1;
                        morepages += 1;

                        checkpictures = true;
                        var wbE = workbookPicture.Worksheets.Add("Engineering");
                        workbookPicture.Worksheets["Engineering"].CopyFrom(workbookPicture.Worksheets["Pictures"]);
                        //var wbE = workbookPicture.Worksheets["Pictures"];
                        //workbookPicture.Worksheets.AddCopyBefore(wbE);
                        wbE.Range["A6"].Value = "Engineering";
                        int picCount = _context.EngDefectPhotos.Count();
                        var Pics = _context.EngDefectPhotos.Take(Math.Min(4, picCount)).ToList();
                        List<ExcelPicture> pictures = new List<ExcelPicture>();
                        for (int i = 0; i < Pics.Count; i++)
                        {
                            byte[] imageBytes = Pics[i].EngDefectPhotoContent;
                            MemoryStream stream = new MemoryStream(imageBytes);
                            ExcelPicture picture = wbE.Pictures.Add(i + 1, 5, stream);
                            picture.Height = 300;
                            picture.Width = 300;
                            pictures.Add(picture);
                        }

                        if (pictures.Count > 0)
                        {
                            pictures[0].LeftColumn = 3;
                            pictures[0].TopRow = 7;
                        }
                        if (pictures.Count > 1)
                        {
                            pictures[1].LeftColumn = 18;
                            pictures[1].TopRow = 7;
                        }
                        if (pictures.Count > 2)
                        {
                            pictures[2].LeftColumn = 3;
                            pictures[2].TopRow = 22;
                        }
                        if (pictures.Count > 3)
                        {
                            pictures[3].LeftColumn = 18;
                            pictures[3].TopRow = 22;
                        }
                        wbE.Range["B37"].Value = ncr.NcrEng.NcrEngDefectVideo;
                        wbE.Range["Z4"].Value = firstpage.ToString();
                        wbE.Range["AE5"].Value = ncr.NcrNumber;

                        worksheet.Range["AC4"].Value = morepages.ToString();
                        //wbE.Range["AC4"].Value = worksheet.Range["AC4"].Value;
                    }
                }

                //Operations
                if (ncr.NcrOperation != null)
                {
                    worksheet.Range["B29"].Value = ncr.NcrOperation.NcrPurchasingDescription;
                    //worksheet.Range["V33"].Value = ncr.NcrOperation.CreatedBy;
                    worksheet.Range["AD33"].Value = ncr.NcrOperation.CreatedOn.ToString();
                    switch (ncr.NcrOperation.OpDispositionType.OpDispositionTypeId)
                    {
                        case 1:
                            worksheet.Range["B28"].Value = "X";
                            break;
                        case 2:
                            worksheet.Range["K28"].Value = "X";
                            break;
                        case 3:
                            worksheet.Range["O27"].Value = "X";
                            break;
                        case 4:
                            worksheet.Range["S28"].Value = "X";
                            break;
                    }
                    if (ncr.NcrOperation.Car.Equals(true))
                    {
                        worksheet.Range["L31"].Value = "X";
                        worksheet.Range["Z31"].Value = $"CAR" + ncr.NcrOperation.CarNumber;

                    }
                    else
                    {
                        worksheet.Range["N31"].Value = "X";
                    }
                    if (ncr.NcrOperation.FollowUp.Equals(true))
                    {
                        worksheet.Range["L32"].Value = "X";
                        worksheet.Range["AD32"].Value = ncr.NcrOperation.ExpectedDate.ToString();
                    }
                    else
                    {
                        worksheet.Range["N32"].Value = "X";
                    }
                    if (ncr.NcrOperation.OpDefectPhotos.Count > 0 || ncr.NcrOperation.NcrOperationVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;

                        checkpictures = true;
                        var wbO = workbookPicture.Worksheets.Add("Operations");
                        workbookPicture.Worksheets["Operations"].CopyFrom(workbookPicture.Worksheets["Pictures"]);
                        //var wbO = workbookPicture.Worksheets["Pictures"];
                        //workbookPicture.Worksheets.AddCopyBefore(wbO);
                        wbO.Range["A6"].Value = "Operations";
                        int picCount = _context.OpDefectPhotos.Count();
                        var Pics = _context.OpDefectPhotos.Take(Math.Min(4, picCount)).ToList();
                        List<ExcelPicture> pictures = new List<ExcelPicture>();
                        for (int i = 0; i < Pics.Count; i++)
                        {
                            byte[] imageBytes = Pics[i].OpDefectPhotoContent;
                            MemoryStream stream = new MemoryStream(imageBytes);
                            ExcelPicture picture = wbO.Pictures.Add(i + 1, 5, stream);
                            picture.Height = 300;
                            picture.Width = 300;
                            pictures.Add(picture);
                        }

                        if (pictures.Count > 0)
                        {
                            pictures[0].LeftColumn = 3;
                            pictures[0].TopRow = 7;
                        }
                        if (pictures.Count > 1)
                        {
                            pictures[1].LeftColumn = 18;
                            pictures[1].TopRow = 7;
                        }
                        if (pictures.Count > 2)
                        {
                            pictures[2].LeftColumn = 3;
                            pictures[2].TopRow = 22;
                        }
                        if (pictures.Count > 3)
                        {
                            pictures[3].LeftColumn = 18;
                            pictures[3].TopRow = 22;
                        }
                        wbO.Range["B37"].Value = ncr.NcrOperation.NcrOperationVideo;
                        wbO.Range["Z4"].Value = firstpage.ToString();
                        wbO.Range["AE5"].Value = ncr.NcrNumber;

                        worksheet.Range["AC4"].Value = morepages.ToString();
                        //wbO.Range["AC4"].Value = worksheet.Range["AC4"].Value;
                    }
                }

                //Procurement
                if (ncr.NcrProcurement != null)
                {
                    //worksheet.Range["T40"].Value = ncr.NcrProcurement.CreatedBy;
                    worksheet.Range["AD40"].Value = ncr.NcrProcurement.CreatedOn.ToString();
                    if (ncr.NcrProcurement.NcrProcSupplierReturnReq.Equals(true))
                    {
                        worksheet.Range["L34"].Value = "X";
                        worksheet.Range["T36"].Value = ncr.NcrProcurement.SupplierReturnAccount;
                        worksheet.Range["T35"].Value = ncr.NcrProcurement.SupplierReturnName;
                        worksheet.Range["H35"].Value = ncr.NcrProcurement.SupplierReturnMANum;
                        worksheet.Range["H36"].Value = ncr.NcrProcurement.NcrProcExpectedDate.ToString();
                    }
                    if (ncr.NcrProcurement.NcrProcSupplierBilled.Equals(true))
                    {
                        worksheet.Range["L37"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["N37"].Value = "X";
                    }
                    if (ncr.NcrProcurement.NcrProcCreditExpected.Equals(true))
                    {
                        worksheet.Range["L38"].Value = "X";
                        worksheet.Range["AB38"].Value = $"$" + ncr.NcrProcurement.NcrProcRejectedValue.ToString();

                    }
                    else
                    {
                        worksheet.Range["N38"].Value = "X";
                    }
                    if (ncr.NcrProcurement.NcrProcSAPReturnCompleted.Equals(true))
                    {
                        worksheet.Range["L39"].Value = "X";
                    }
                    else
                    {
                        worksheet.Range["N39"].Value = "X";
                    }
                    if (ncr.NcrProcurement.ProcDefectPhotos.Count > 0 || ncr.NcrProcurement.NcrProcDefectVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;

                        checkpictures = true;
                        var wbP = workbookPicture.Worksheets.Add("Procurement");
                        workbookPicture.Worksheets["Procurement"].CopyFrom(workbookPicture.Worksheets["Pictures"]);
                        //var wbP = workbookPicture.Worksheets["Pictures"];
                        //workbookPicture.Worksheets.AddCopyBefore(wbP);
                        wbP.Range["A6"].Value = "Procurement";
                        int picCount = _context.ProcDefectPhotos.Count();
                        var Pics = _context.ProcDefectPhotos.Take(Math.Min(4, picCount)).ToList();
                        List<ExcelPicture> pictures = new List<ExcelPicture>();
                        for (int i = 0; i < Pics.Count; i++)
                        {
                            byte[] imageBytes = Pics[i].ProcDefectPhotoContent;
                            MemoryStream stream = new MemoryStream(imageBytes);
                            ExcelPicture picture = wbP.Pictures.Add(i + 1, 5, stream);
                            picture.Height = 300;
                            picture.Width = 300;
                            pictures.Add(picture);
                        }

                        if (pictures.Count > 0)
                        {
                            pictures[0].LeftColumn = 3;
                            pictures[0].TopRow = 7;
                        }
                        if (pictures.Count > 1)
                        {
                            pictures[1].LeftColumn = 18;
                            pictures[1].TopRow = 7;
                        }
                        if (pictures.Count > 2)
                        {
                            pictures[2].LeftColumn = 3;
                            pictures[2].TopRow = 22;
                        }
                        if (pictures.Count > 3)
                        {
                            pictures[3].LeftColumn = 18;
                            pictures[3].TopRow = 22;
                        }
                        wbP.Range["B37"].Value = ncr.NcrProcurement.NcrProcDefectVideo;
                        wbP.Range["Z4"].Value = firstpage.ToString();
                        wbP.Range["AE5"].Value = ncr.NcrNumber;

                        worksheet.Range["AC4"].Value = morepages.ToString();
                        //wbP.Range["AC4"].Value = worksheet.Range["AC4"].Value;
                    }
                }

                //Reinspection
                if (ncr.NcrReInspect != null)
                {
                    worksheet.Range["F42"].Value = ncr.NcrReInspect.NcrReInspectNotes;
                    //worksheet.Range["V43"].Value = ncr.NcrReInspect.CreatedBy;
                    worksheet.Range["AE43"].Value = ncr.NcrReInspect.CreatedOn.ToString();
                    if (ncr.NcrReInspect.NcrReInspectAcceptable.Equals(true))
                    {
                        worksheet.Range["L41"].Value = "X";


                    }
                    else
                    {
                        worksheet.Range["N41"].Value = "X";
                        worksheet.Range["J43"].Value = ncr.NcrReInspect.NcrReInspectNewNcrNumber;
                    }
                    if (ncr.NcrPhase == NcrPhase.Closed)
                    {
                        worksheet.Range["G44"].Value = "X";
                    }

                    if (ncr.NcrReInspect.NcrReInspectPhotos.Count > 0 || ncr.NcrReInspect.NcrReInspectDefectVideo != null)
                    {
                        firstpage += 1;
                        morepages += 1;

                        checkpictures = true;
                        var wbR = workbookPicture.Worksheets.Add("Reinspection");
                        workbookPicture.Worksheets["Reinspection"].CopyFrom(workbookPicture.Worksheets["Pictures"]);
                        //var wbR = workbookPicture.Worksheets["Pictures"];
                        //workbookPicture.Worksheets.AddCopyBefore(wbR);
                        wbR.Range["A6"].Value = "Reinspection";
                        int picCount = _context.NcrReInspectPhotos.Count();
                        var Pics = _context.NcrReInspectPhotos.Take(Math.Min(4, picCount)).ToList();
                        List<ExcelPicture> pictures = new List<ExcelPicture>();
                        for (int i = 0; i < Pics.Count; i++)
                        {
                            byte[] imageBytes = Pics[i].NcrReInspectPhotoContent;
                            MemoryStream stream = new MemoryStream(imageBytes);
                            ExcelPicture picture = wbR.Pictures.Add(i + 1, 5, stream);
                            picture.Height = 300;
                            picture.Width = 300;
                            pictures.Add(picture);
                        }

                        if (pictures.Count > 0)
                        {
                            pictures[0].LeftColumn = 3;
                            pictures[0].TopRow = 7;
                        }
                        if (pictures.Count > 1)
                        {
                            pictures[1].LeftColumn = 18;
                            pictures[1].TopRow = 7;
                        }
                        if (pictures.Count > 2)
                        {
                            pictures[2].LeftColumn = 3;
                            pictures[2].TopRow = 22;
                        }
                        if (pictures.Count > 3)
                        {
                            pictures[3].LeftColumn = 18;
                            pictures[3].TopRow = 22;
                        }
                        wbR.Range["B37"].Value = ncr.NcrReInspect.NcrReInspectDefectVideo;
                        wbR.Range["Z4"].Value = firstpage.ToString();
                        wbR.Range["AE5"].Value = ncr.NcrNumber;

                        worksheet.Range["AC4"].Value = morepages.ToString();
                        //wbR.Range["AC4"].Value = worksheet.Range["AC4"].Value;
                    }
                }




                Workbook exportWorkbook = new Workbook();

                exportWorkbook.Worksheets.AddCopy(workbook.Worksheets);

                //foreach (Worksheet sheet in workbook.Worksheets)
                //{
                //    exportWorkbook.Worksheets.AddCopy(sheet);
                //}

                if (checkpictures == true)
                {
                    List<string> sheetsToIgnore = new List<string> { "Pictures" };
                    foreach (Worksheet sheet in workbookPicture.Worksheets)
                    {
                        if (!sheetsToIgnore.Contains(sheet.Name))
                        {
                            if (ncr.NcrReInspect != null)
                            {

                                //worksheet.Range["U40"].Value = ncr.NcrReInspect.CreatedBy;
                                sheet.Range["AE40"].Value = ncr.NcrReInspect.CreatedOn.ToString();
                                if (ncr.NcrReInspect.NcrReInspectAcceptable.Equals(true))
                                {
                                    sheet.Range["L39"].Value = "X";


                                }
                                else
                                {
                                    sheet.Range["Q39"].Value = "X";
                                    sheet.Range["J40"].Value = ncr.NcrReInspect.NcrReInspectNewNcrNumber;
                                }
                            }
                            sheet.Range["AC4"].Value += morepages.ToString();
                            exportWorkbook.Worksheets.AddCopy(sheet);
                        }
                    }
                }

                //exportWorkbook.ConverterSetting.SheetFitToPage = true;
                string filename = ncr.NcrNumber;
                //string rootPath = Server.MapPath("~");
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string pdfFilePath = Path.Combine(rootPath, "Downloads", $"{filename}.pdf");
                string downloadsFolder = Path.Combine(rootPath, "Downloads");
                if (!Directory.Exists(downloadsFolder))
                {
                    Directory.CreateDirectory(downloadsFolder);
                }
                exportWorkbook.SaveToFile(pdfFilePath, FileFormat.PDF);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = $"{filename}.pdf",
                    Inline = false,
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                return PhysicalFile(pdfFilePath, "application/pdf");


            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF: {ex.Message}");
            }


        }
        #endregion
    }
}
