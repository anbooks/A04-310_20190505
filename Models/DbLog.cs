using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbLog
    {
        public int AlterId { get; set; }
        [Display(Name = "题号")]
        public int  QuId { get; set; }
        [Display(Name = "修改人")]
        public string  EmName { get; set; }
        [Display(Name = "修改时间")]
        public DateTime? AlterTime { get; set; }
        [Display(Name = "题目")]
        public string AlQuestion { get; set; }
        [Display(Name = "A")]
        public string AlOptionA { get; set; }
        [Display(Name = "B")]
        public string AlOptionB { get; set; }
        [Display(Name = "C")]
        public string AlOptionC { get; set; }
        [Display(Name = "D")]
        public string AlOptionD { get; set; }
        [Display(Name = "正确答案")]
        public string AlRightAnswer { get; set; }
        [Display(Name = "难度")]
        public string AlDifficulty { get; set; }
        [Display(Name = "操作")]
        public string AlEdit { get; set; }
    }
}
