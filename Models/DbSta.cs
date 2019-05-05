using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class DbSta
    {
        public int StaId { get; set; }
        public int EmId { get; set; }
        public double Accuracy { get; set; }
        public int Quantity { get; set; }
        public double Average { get; set; }
        public string QueryNum { get; set; }

        public DbEm Em { get; set; }
    }
}
