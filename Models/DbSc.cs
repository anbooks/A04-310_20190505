using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbSc
    {
        public int ScId { get; set; }
        public int BuId { get; set; }
        [Display(Name = "成绩")]
        public double? Score { get; set; }
        [Display(Name = "考试开始时间")]
        public DateTime? TestStart { get; set; }
        [Display(Name = "考试结束时间")]
        public DateTime? TestEnd { get; set; }
        public int EmId { get; set; }
        public int Correct { get; set; }
        public int? PaId { get; set; }
        public DbBu Bu { get; set; }
        public DbEm Em { get; set; }

        public DbPa Pa { get; set; }
    }
}
