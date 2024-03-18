using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace HaverDevProject.Controllers.API
{
    public class ApiController : Controller
    {
        private readonly HaverNiagaraContext _context;

        public ApiController(HaverNiagaraContext context)
        {
            _context = context;
        }
        private SelectList SupplierSelectList(int? selectedId)
        {
            return new SelectList(_context.Suppliers
                .Where(s => s.SupplierStatus == true && s.SupplierName != "NO SUPPLIER PROVIDED")
                .OrderBy(s => s.SupplierName), "SupplierId", "Summary", selectedId);
        }

        private SelectList NcrsSelectList()
        {
            var ncrs = _context.NcrQas
                .Include(n => n.Supplier)
                .Include(n => n.Defect)
                .Include(n => n.Ncr)
                .Where(n => n.Ncr.NcrPhase != NcrPhase.Archive)
                .AsNoTracking();

            return new SelectList(ncrs
                .OrderBy(n => n.NcrId));
        }

        [HttpGet]
        [Route("api/ncr")]
        public JsonResult GetSuppliersTest(int? id)
        {
            return Json(SupplierSelectList(id));
        }

        //[HttpGet]
        //[Route("api/getncrs")]
        //public JsonResult GetNcrs(int? id)
        //{
        //    return Json(NcrsSelectList());
        //}

        // GET: api/Ncrs
        [HttpGet]
        [Route("api/Ncrs")]
        public async Task<ActionResult<IEnumerable<NcrQaDTO>>> GetNcrs()
        {
            var ncrsDTO = await _context.NcrQas
                .Include(n => n.Ncr)
                .Include(n => n.Supplier)
                .Include(n => n.Item)
                .Include(n => n.Defect)
                .Select(n => new NcrQaDTO
                {
                    NcrQaId = n.NcrQaId,
                    NcrNumber = n.Ncr.NcrNumber,
                    NcrStatus = n.Ncr.NcrStatus,
                    NcrPhase = n.Ncr.NcrPhase,
                    NcrQacreationDate = n.NcrQacreationDate,
                    NcrQaOrderNumber = n.NcrQaOrderNumber,
                    NcrQaSalesOrder = n.NcrQaSalesOrder,
                    NcrQaQuanReceived = n.NcrQaQuanReceived,
                    NcrQaQuanDefective = n.NcrQaQuanDefective,
                    SupplierId = n.SupplierId,
                    //Supplier = new Supplier
                    //{
                    //    SupplierId = n.Supplier.SupplierId,
                    //    SupplierName = n.Supplier.SupplierName,
                    //    SupplierStatus = n.Supplier.SupplierStatus
                    //}
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
