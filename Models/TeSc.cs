using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class TeSc
    {
        public int ScId { get; set; }
        [Display(Name = "职位")]
        public int BuId { get; set; }
        [Display(Name = "分数")]
        public double? Score { get; set; }
        [Display(Name = "开始时间")]
        public DateTime? TestStart { get; set; }
        [Display(Name = "结束时间")]
        public DateTime? TestEnd { get; set; }
        [Display(Name = "答题人")]
        public int EmId { get; set; }
        [Display(Name = "正确数")]
        public int Correct { get; set; }
        [Display(Name = "试卷名称")]
        public int? PaId { get; set; }

        [Display(Name = "岗位")]
        public DbBu Bu { get; set; }
        [Display(Name = "答题人姓名")]
        public DbEm Em { get; set; }
        [Display(Name = "试卷名称")]
        public Test Pa { get; set; }
    }
}
