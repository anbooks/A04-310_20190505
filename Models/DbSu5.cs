using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public partial class DbSu5
    {
        public DbSu5()
        {
            DbQu = new HashSet<DbQu>();
        }

        public int SueId { get; set; }
        public string SueName { get; set; }
        public int SudId { get; set; }

        public DbSu4 Sud { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
    }
}
