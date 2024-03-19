using HaverDevProject.Data;
using Microsoft.AspNetCore.Mvc;
using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using HaverDevProject.ViewModels;

namespace HaverDevProject.Controllers.APIs
{
    public class ApiController : Controller
    {
        private readonly HaverNiagaraContext _context;

        public ApiController(HaverNiagaraContext context)
        {
            _context = context;
        }

        //Testing----Eliminar despues de evaluar
        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "Summary", selectedId);
        }

        //Testing - Eliminar despues de evaluar
        [HttpGet]
        [Route("api/ncr")]
        public JsonResult GetSuppliersTest(int? id)
        {
            return Json(SupplierSelectList(id));
        }

        // GET: api/Ncrs
        [HttpGet]
        [Route("api/Ncrs")]
        public async Task<ActionResult<IEnumerable<NcrQaApiDTO>>> GetNcrs()
        {
            var ncrsDTO = await _context.NcrQas
                .Include(n => n.Ncr)
                .Include(n => n.Supplier)
                .Include(n => n.Item)
                .Include(n => n.Defect)
                .Select(n => new NcrQaApiDTO
                {
                    NcrNumber = n.Ncr.NcrNumber,
                    NcrStatus = n.Ncr.NcrStatus,
                    NcrPhase = n.Ncr.NcrPhase,
                    NcrQacreationDate = n.NcrQacreationDate,
                    NcrQaOrderNumber = n.NcrQaOrderNumber,
                    NcrQaSalesOrder = n.NcrQaSalesOrder,
                    NcrQaQuanReceived = n.NcrQaQuanReceived,
                    NcrQaQuanDefective = n.NcrQaQuanDefective,
                    SupplierId = n.SupplierId,
                    SupplierApiDTO = new SupplierApiDTO
                    {
                        SupplierId = n.Supplier.SupplierId,
                        SupplierName = n.Supplier.SupplierName,
                        SupplierCode = n.Supplier.SupplierCode,
                        SupplierStatus = n.Supplier.SupplierStatus                        
                    },
                    ItemId = n.Item.ItemId,
                    ItemApiDTO = new ItemApiDTO
                    {
                        ItemId = n.Item.ItemId,
                        ItemName = n.Item.ItemName,
                        ItemNumber = n.Item.ItemNumber
                    },
                    DefectId = n.Defect.DefectId,
                    DefectApiDTO = new DefectApiDTO
                    {
                        DefectId = n.Defect.DefectId,
                        DefectName = n.Defect.DefectName
                    }                 
                    
                })
                .ToListAsync();

            if (ncrsDTO.Count() > 0)
            {
                return ncrsDTO;
            }
            else
            {
                return NotFound(new { message = "Error: No Ncrs records in the system." });
            }
        }
    }

    
}

