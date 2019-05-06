using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbEm
    {
        public DbEm()
        {
          
            DbQu = new HashSet<DbQu>();
            DbSc = new HashSet<DbSc>();
            TeSc = new HashSet<TeSc>();
            DbPa = new HashSet<DbPa>();
            DbSta = new HashSet<DbSta>();
            TestAn = new HashSet<TestAn>();
            TeEm= new HashSet<TeEm>();
        }

        public int EmId { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(25, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Required(ErrorMessage ="密码不能为空")]
        [Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string Password1 { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必须输入{0}")]
        public string EmName { get; set; }

        [Display(Name = "部门")]
        [Required(ErrorMessage = "必须输入{0}")]
        public int Branch { get; set; }


        [Display(Name = "用户权限")]
        public int PoId { get; set; }

        [Display(Name = "职位")]
        [Required(ErrorMessage = "必须输入{0}")]
        public int BuId { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "帐号")]




        //public DbBm BranchNavigation { get; set; }
        public string  CardId { get; set; }
        public DbBm Bm { get; set; }
        public DbBu Bu { get; set; }
       
        public DbPo Po { get; set; }
        [Display(Name = "角色")]

        public string Picture { get; set; }

        public ICollection<DbSc> DbSc { get; set; }
        public ICollection<TeSc> TeSc { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
        public ICollection<DbPa> DbPa { get; set; }
        public ICollection<DbSta> DbSta { get; set; }
        public ICollection<TestAn> TestAn { get; set; }

        public ICollection<TeEm> TeEm { get; set; }
    }
}
