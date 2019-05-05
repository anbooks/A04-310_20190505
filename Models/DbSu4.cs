using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public partial class DbSu4
    {
        public DbSu4()
        {
            DbQu = new HashSet<DbQu>();
            DbSu5 = new HashSet<DbSu5>();
        }

        public int SudId { get; set; }
        public string SudName { get; set; }
        public int SucId { get; set; }

        public DbSu3 Suc { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
        public ICollection<DbSu5> DbSu5 { get; set; }
    }
}