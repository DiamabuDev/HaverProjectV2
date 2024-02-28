using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaverDevProject.Models
{
    [Table("supplierReturn")]
    public class SupplierReturn
    {
        [Key]
        [Column("supplierReturnId")]
        public int SupplierReturnId { get; set; }

        [Display(Name = "RMA Number")]
        [Required(ErrorMessage = "You must provide the RMA Number")]
        [Column("supplierReturnMANum")]
        public string SupplierReturnMANum { get; set; }

        [Display(Name = "Carrier Name")]
        [Required(ErrorMessage = "You must provide the Carrier Name")]
        [StringLength(45, ErrorMessage = "Only 45 characters for Carrier Name")]
        [Column("supplierReturnName")]
        public string SupplierReturnName { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "You must provide the Account Number")]
        [StringLength(45, ErrorMessage = "Only 45 characters for the Account Number")]
        [Column("supplierReturnAccount")]
        public string SupplierReturnAccount { get; set; }

        [Display(Name = "Name of Procurement")]
        [Column("ncrProcurementId")]
        public int NcrProcurementId { get; set; }

        [Display(Name = "Procurement")]
        [ForeignKey("NcrProcurementId")]
        public virtual NcrProcurement NcrProcurement { get; set; }

    }
}

