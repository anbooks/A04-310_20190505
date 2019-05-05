using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  //display


namespace WebApplication8.Models
{
    public partial class DbBm
    {
        public DbBm()
        {
            DbEm = new HashSet<DbEm>();
        }

        public int Id { get; set; }

        [Display(Name = "部门")]
        public string Branch { get; set; }

        public ICollection<DbEm> DbEm { get; set; }
    }
}
