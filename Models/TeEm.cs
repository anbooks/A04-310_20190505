using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class TeEm
    {
        public int Id { get; set; }
        public int? TeEmId { get; set; }
        public int? Teid { get; set; }
        public int? TeFlag { get; set; }

        public Test Te { get; set; }
        public DbEm Em { get; set; }
    }
}
