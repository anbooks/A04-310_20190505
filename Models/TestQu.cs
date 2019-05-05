using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class TestQu
    {
        public TestQu()
        {
            TestAn = new HashSet<TestAn>();
        }

        public int TequId { get; set; }
        public int? TestId { get; set; }
        public int? QuId { get; set; }
        public int? TestaId { get; set; }

        public DbQu Qu { get; set; }
        public Test Test { get; set; }
        public ICollection<TestAn> TestAn { get; set; }
    }
}
