using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaverDevProject.ViewModels;

public class ItemApiDTO
{    
    public int ItemId { get; set; }

    public string Summary => $"{ItemNumber} - {ItemName}";    
    public int ItemNumber { get; set; }    
    public string ItemName { get; set; }    

    //[InverseProperty("Item")]
    //public ICollection<NcrQa> NcrQas { get; set; } = new HashSet<NcrQa>();
}
