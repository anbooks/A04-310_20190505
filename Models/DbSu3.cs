using System;
using System.Collections.Generic;

namespace WebApplication8.Models
{

        public partial class DbSu3
        {
            public DbSu3()
            {
                DbPa = new HashSet<DbPa>();
                DbQu = new HashSet<DbQu>();
                DbSu4 = new HashSet<DbSu4>();
            }

            public int SucId { get; set; }
            public string SucName { get; set; }
            public int SubId { get; set; }

            public DbSu2 Sub { get; set; }
            public ICollection<DbPa> DbPa { get; set; }
            public ICollection<DbQu> DbQu { get; set; }
            public ICollection<DbSu4> DbSu4 { get; set; }
        }
    }
