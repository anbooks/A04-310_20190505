using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbTe
    {
        public int TeId { get; set; }
        [Display(Name = "考生答案")]
        public string EmAnswer { get; set; }
        public int PaId { get; set; }
        public int PaxId { get; set; }

        //判断用户答案对错
        public string RW { get; set; }

        public DbPax Pax { get; set; }
        public DbPa Pa { get; set; }
    }
}
