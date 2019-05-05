using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication8.Models
{
    public partial class DbPax
    {
        public DbPax()
        {
            DbTe = new HashSet<DbTe>();
        }
        
        public int Pax1_ID { get; set; }//试卷题号
        public int PaxId { get; set; }//主键
        public int QuId { get; set; }
        public int PaId { get; set; }
        [Display(Name = "题号")]       
        public DbPa Pa { get; set; }
        public DbQu Qu { get; set; }
        public ICollection<DbTe> DbTe { get; set; }
    }
}
