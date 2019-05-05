using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    
    public partial class DbPa
    {   //dfgasdsakjdhkjxzczxc
        public DbPa()
        {
            DbExam = new HashSet<DbExam>();
            DbPax = new HashSet<DbPax>();
            DbSc = new HashSet<DbSc>();
            DbTe = new HashSet<DbTe>();
        }

        public int PaId { get; set; }
        public int? EmId { get; set; }
        public string Tester { get; set; }
        public int? SuaId { get; set; }
        public int? SubId { get; set; }
        public int? SucId { get; set; }

        public DbEm Em { get; set; }
        public DbSu1 Sua { get; set; }
        public DbSu2 Sub { get; set; }
        public DbSu3 Suc { get; set; }
        public ICollection<DbExam> DbExam { get; set; }
        public ICollection<DbPax> DbPax { get; set; }
        public ICollection<DbSc> DbSc { get; set; }
        public ICollection<DbTe> DbTe { get; set; }
    }
}

    