﻿using HaverDevProject.Models;

namespace HaverDevProject.ViewModels
{
    public class SupplierDetailsViewModel
    {
        public Supplier Supplier { get; set; }
        public IEnumerable<Ncr> RelatedNCRs { get; set; }
    }
}
