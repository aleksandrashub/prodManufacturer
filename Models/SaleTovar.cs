using System;
using System.Collections.Generic;

namespace ProdManufacturer.Models;

public partial class SaleTovar
{
    public int IdSale { get; set; }

    public int IdTovar { get; set; }

    public int? Amount { get; set; }

    public virtual Sale IdSaleNavigation { get; set; } = null!;

    public virtual Tovar IdTovarNavigation { get; set; } = null!;
}
