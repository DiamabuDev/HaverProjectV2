using HaverDevProject.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.ViewModels
{
    public class SupplierApiDTO
    {        
        public int SupplierId { get; set; }        
        public string SupplierCode { get; set; }                
        public string SupplierName { get; set; }                
        public bool SupplierStatus { get; set; } 
    }
}
