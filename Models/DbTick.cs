using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public partial class DbTick
    {
        public int TickId { get; set; }
        [Display(Name = "反馈内容")]
        public string Ticking { get; set; }
        [Display(Name = "处理状态")]
        public string Handle { get; set; }
        [Display(Name = "更新状态")]
        public string Update { get; set; }
        [Display(Name = "反馈人")]
        public string EmName { get; set; }
        public int? QuId { get; set; }

        public DbQu Qu { get; set; }

    }
}
