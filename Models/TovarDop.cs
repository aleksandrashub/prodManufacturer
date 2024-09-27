using System;
using System.Collections.Generic;

namespace ProdManufacturer.Models;

public partial class TovarDop
{
    public int? IdDopTov { get; set; }

    public int IdMainTov { get; set; }

    public virtual Tovar? IdDopTovNavigation { get; set; }

    public virtual Tovar IdMainTovNavigation { get; set; } = null!;
}
