using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class TeTe
    {
        public int TeId { get; set; }
        public string EmAnswer { get; set; }
        public int? PaxId { get; set; }
        public string RW { get; set; }
        public int PaId { get; set; }

        public Test Pa { get; set; }
    }
}
