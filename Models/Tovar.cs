using System;
using System.Collections.Generic;

namespace ProdManufacturer.Models;

public partial class Tovar
{
    public string? NameTovar { get; set; }

    public float? Cost { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? IdManufacturer { get; set; }

    public int? IdStatus { get; set; }

    public int IdTovar { get; set; }

    public virtual ICollection<DopImg> DopImgs { get; set; } = new List<DopImg>();

    public virtual Manufacturer? IdManufacturerNavigation { get; set; }

    public virtual Status? IdStatusNavigation { get; set; }

    public virtual ICollection<SaleTovar> SaleTovars { get; set; } = new List<SaleTovar>();

    public virtual ICollection<Tovar> IdDopTovs { get; set; } = new List<Tovar>();

    public virtual ICollection<Tovar> IdMainTovs { get; set; } = new List<Tovar>();
}
