using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{
    public partial class DbSu1
    {
        public DbSu1()
        {
            DbPa = new HashSet<DbPa>();
            DbQu = new HashSet<DbQu>();
            DbSu2 = new HashSet<DbSu2>();
        }

        public int SuaId { get; set; }
        public string SuaName { get; set; }

        public ICollection<DbPa> DbPa { get; set; }
        public ICollection<DbQu> DbQu { get; set; }
        public ICollection<DbSu2> DbSu2 { get; set; }
    }
}
