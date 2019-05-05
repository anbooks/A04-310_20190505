using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public partial class DbBu
    {
        public DbBu()
        {
            DbEm = new HashSet<DbEm>();
            DbQu = new HashSet<DbQu>();
            TeSc = new HashSet<TeSc>();
            DbSc = new HashSet<DbSc>();
        }

        public int BuId { get; set; }
        [Display(Name = "职位")]
        public string BuName { get; set; }

        public ICollection<DbEm> DbEm { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
        public ICollection<DbSc> DbSc { get; set; }
        public ICollection<TeSc> TeSc { get; set; }
    }
}
