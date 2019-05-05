using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class TestAn
    {
        public int TeAnId { get; set; }
        public int QuId { get; set; }
        public int TestId { get; set; }
        public int EmId { get; set; }
        public string RW { get; set; }
        public string TeAn { get; set; }
        public int TestaId { get; set; }
        public int? TequId { get; set; }

        public DbEm Em { get; set; }
        public DbQu Qu { get; set; }
        public TestQu Tequ { get; set; }
        public Test Test { get; set; }
    }
}
