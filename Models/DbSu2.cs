using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class DbSu2
    {
        public DbSu2()
        {
            DbPa = new HashSet<DbPa>();
            DbQu = new HashSet<DbQu>();
            DbSu3 = new HashSet<DbSu3>();
        }

        public int SubId { get; set; }
        public string SubName { get; set; }
        public int SuaId { get; set; }

        public DbSu1 Sua { get; set; }
        public ICollection<DbPa> DbPa { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
        public ICollection<DbSu3> DbSu3 { get; set; }
    }
}
