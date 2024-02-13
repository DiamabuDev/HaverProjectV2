using HaverDevProject.Data;
using HaverDevProject.Models;
using HaverDevProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HaverDevProject.Controllers
{
    public class NCRQACreateDTOController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly HaverNiagaraContext _dbContext;

        public NCRQACreateDTOController(HaverNiagaraContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: NcrQa/Create
        public IActionResult Create()
        {
            NCRQACreateDTO ncrQaCreateDTOs = new NCRQACreateDTO();
            // Puedes agregar lógica aquí si es necesario antes de mostrar la vista de creación.
            ncrQaCreateDTOs.NcrNumber = "2024 - 000199";
            return View(ncrQaCreateDTOs);
        }

        // POST: NcrQa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NCRQACreateDTO ncrQaCreateDTO)
        {
            if (ModelState.IsValid)
            {
                
                // Mapping
                Ncr ncr = new Ncr
                {
                    NcrNumber = ncrQaCreateDTO.NcrNumber,
                    NcrLastUpdated = DateTime.Now,
                    NcrStatus = true                
                    
                };
                _dbContext.Ncrs.Add(ncr);
                _dbContext.SaveChanges();

                int ncrNumber = _dbContext.Ncrs
                    .Where(n => n.NcrNumber == ncrQaCreateDTO.NcrNumber)
                    .Select(n => n.NcrId)
                    .FirstOrDefault();

                NcrQa ncrQa = new NcrQa
                {                    
                    NcrQaItemMarNonConforming = ncrQaCreateDTO.NcrQaItemMarNonConforming,
                    NcrQaProcessApplicable = ncrQaCreateDTO.NcrQaProcessApplicable,
                    NcrQacreationDate = DateTime.Today,
                    NcrQaOrderNumber = ncrQaCreateDTO.NcrQaOrderNumber,
                    NcrQaSalesOrder = ncrQaCreateDTO.NcrQaSalesOrder,
                    NcrQaQuanReceived = ncrQaCreateDTO.NcrQaQuanReceived,
                    NcrQaQuanDefective = ncrQaCreateDTO.NcrQaQuanDefective,
                    NcrQaDescriptionOfDefect = ncrQaCreateDTO.NcrQaDescriptionOfDefect,
                    ItemId = ncrQaCreateDTO.ItemId,
                    NcrId = ncrNumber
                };
                
                _dbContext.NcrQas.Add(ncrQa);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index)); // Redirigir a la acción Index o a la acción que desees
            }
            else
            {
                //Debugging Approach: Not for production code.
                //This code will list validation errors at the top of the View.
                //Use it to diagnose when there seems to be a Validation Error
                //that is going unreported.  Remove this code when you are
                //finished debugging.
                var booBoos = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors });

                foreach (var booBoo in booBoos)
                {
                    string key = booBoo.Key;
                    foreach (var error in booBoo.Errors)
                    {
                        var errorMessage = error?.ErrorMessage;
                        ModelState.AddModelError("", "For " + key + ": " + errorMessage);
                    }
                }
            }

            return View(ncrQaCreateDTO);
        }
    }
}
