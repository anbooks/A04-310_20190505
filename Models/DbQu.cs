using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbQu
    {
        public DbQu()
        {
            DbPax = new HashSet<DbPax>();
            DbTick = new HashSet<DbTick>();
            TestAn = new HashSet<TestAn>();
            TestQu = new HashSet<TestQu>();
        }

        public int QuId { get; set; }
        [Display(Name = "题型")]
        public string Type { get; set; }
        [Display(Name = "职位")]
        public int? BuId { get; set; }
        [Display(Name = "题目")]
        public string Question { get; set; }
        [Display(Name = "选项A")]
        public string OptionA { get; set; }
        [Display(Name = "选项B")]
        public string OptionB { get; set; }
        [Display(Name = "选项C")]
        public string OptionC { get; set; }
        [Display(Name = "选项D")]
        public string OptionD { get; set; }
        [Display(Name = "正确答案")]
        public string RightAnswer { get; set; }
        [Display(Name = "难度")]
        public string Difficulty { get; set; }
        public int? SuaId { get; set; }
        public int? SubId { get; set; }
        public int? SucId { get; set; }
        public int? SudId { get; set; }
        public int? SueId { get; set; }
        public int EmId { get; set; }
        public DbEm Em { get; set; }
        public DbBu Bu { get; set; }
        public DbSu1 Sua { get; set; }
        public DbSu2 Sub { get; set; }
        public DbSu3 Suc { get; set; }
        public DbSu4 Sud { get; set; }
        public DbSu5 Sue { get; set; }
        public ICollection<DbPax> DbPax { get; set; }

        public ICollection<DbTick> DbTick { get; set; }
        public ICollection<TestAn> TestAn { get; set; }
        public ICollection<TestQu> TestQu { get; set; }
    }
}
