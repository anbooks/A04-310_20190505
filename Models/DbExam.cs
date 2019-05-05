using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbExam
    {
        public int ExamId { get; set; }
        [Display(Name = "题号")]
        public int? QuId { get; set; }
        [Display(Name = "题目")]
        public string Question { get; set; }
        [Display(Name = "A")]
        public string OptionA { get; set; }
        [Display(Name = "B")]
        public string OptionB { get; set; }
        [Display(Name = "C")]
        public string OptionC { get; set; }
        [Display(Name = "D")]
        public string OptionD { get; set; }
        [Display(Name = "正确答案")]
        public string RightAnswer { get; set; }
        [Display(Name = "考试答案")]
        public string EmAnswer { get; set; }
        [Display(Name = "对错")]
        public string RW { get; set; }
        [Display(Name = "题型")]
        public string Type { get; set; }
        [Display(Name = "试卷号")]
        public int PaId { get; set; }
        [Display(Name = "试题号")]
        public int Pax1_ID { get; set; }

        public DbPa Pa { get; set; }
    }
}
