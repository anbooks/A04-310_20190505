using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class Test
    {
        public Test()
        {
            TeSc = new HashSet<TeSc>();
            TeTe = new HashSet<TeTe>();
            TestAn = new HashSet<TestAn>();
            TestQu = new HashSet<TestQu>();
            TeEm = new HashSet<TeEm>();
        }

        public int TestId { get; set; }

        [Display(Name = "试卷名称")]
        public string TestName { get; set; }
        [Display(Name = "出题人")]
        public string Tester { get; set; }
        [Display(Name = "单选数")]
        public int Dan { get; set; }
        [Display(Name = "多选数")]
        public int Duo { get; set; }
        [Display(Name = "考试时长")]
        public int? Time { get; set; }
        public int? MobanFlag { get; set; }
        [Display(Name = "判断数")]
        public int? Pan { get; set; }

        [Display(Name = "单选题分值")]
        public int? DanScore { get; set; }
        [Display(Name = "多选题分值")]
        public int? DuoScore { get; set; }
        [Display(Name = "判断题分值")]
        public int? PanScore { get; set; }
        [Display(Name = "审批标记")]
        public int? ShenpiFlag { get; set; }
        public ICollection<TestAn> TestAn { get; set; }
        public ICollection<TestQu> TestQu { get; set; }
        public ICollection<TeSc> TeSc { get; set; }
        public ICollection<TeTe> TeTe { get; set; }
        public ICollection<TeEm> TeEm { get; set; }
    }
}
