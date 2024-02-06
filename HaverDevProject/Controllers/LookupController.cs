using Microsoft.AspNetCore.Mvc;
using HaverDevProject.CustomControllers;
using HaverDevProject.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HaverDevProject.Controllers
{
    public class LookupController : CognizantController
    {
        private readonly HaverNiagaraContext _context;
        public LookupController(HaverNiagaraContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Tab = "Information-Tab")
        {
            //Note: select the tab you want to load by passing in
            ViewData["Tab"] = Tab;
            return View();
        }
        public PartialViewResult ProcessApplicable()
        {
            ViewData["proAppId"] = new
                SelectList(_context.ProcessApplicables
                .OrderBy(a => a.ProAppName), "ProAppId", "ProAppName");
            return PartialView("_ProcessApplicable");
        }
        public PartialViewResult EngDispositionType()
        {
            ViewData["engDispositionTypeId"] = new
                SelectList(_context.EngDispositionTypes
                .OrderBy(a => a.EngDispositionTypeName), "EngDispositionTypeId", "EngDispositionTypeName");
            return PartialView("_EngDispositionType");
        }
        public PartialViewResult FollowUpType()
        {
            ViewData["FollowUpTypeId"] = new
                SelectList(_context.FollowUpTypes
                .OrderBy(a => a.FollowUpTypeName), "FollowUpTypeId", "FollowUpTypeName");
            return PartialView("_FollowUpType");
        }

        public PartialViewResult StatusUpdate()
        {
            ViewData["StatusUpdateId"] = new
                SelectList(_context.StatusUpdates
                .OrderBy(a => a.StatusUpdateName), "StatusUpdateId", "StatusUpdateName");
            return PartialView("_StatusUpdate");
        }

        public PartialViewResult OpDispositionType()
        {
            ViewData["OpDispositionTypeId"] = new
                SelectList(_context.OpDispositionTypes
                .OrderBy(a => a.OpDispositionTypeName), "OpDispositionTypeId", "OpDispositionTypeName");
            return PartialView("_OpDispositionType");
        }
    }
}
