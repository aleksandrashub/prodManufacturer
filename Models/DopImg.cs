using System;
using System.Collections.Generic;

namespace ProdManufacturer.Models;

public partial class DopImg
{
    public int? IdTovar { get; set; }

    public string? NameImg { get; set; }

    public int IdDopImg { get; set; }

    public virtual Tovar? IdTovarNavigation { get; set; }
}
