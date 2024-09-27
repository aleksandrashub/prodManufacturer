using System;
using System.Collections.Generic;

namespace ProdManufacturer.Models;

public partial class Sale
{
    public int IdSale { get; set; }

    public DateOnly? Date { get; set; }

    public virtual ICollection<SaleTovar> SaleTovars { get; set; } = new List<SaleTovar>();
}
