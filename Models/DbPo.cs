using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbPo
    {
        public DbPo()
        {
            DbEm = new HashSet<DbEm>();
        }

        public int PoId { get; set; }
        [Display(Name = "权限")]
        public string PoName { get; set; }

        public ICollection<DbEm> DbEm { get; set; }
    }
}
