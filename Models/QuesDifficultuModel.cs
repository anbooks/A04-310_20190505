using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class QuesDifficultuModel
    {
        public List<DbQu> movies;
        public List<DbSu1> movies1;
        public List<DbSu2> movies2;
        public List<DbSu3> movies3;
        public SelectList BuName;
        public string BuString { get; set; }
        public SelectList Difficulty;
        public string DifficultyString { get; set; }
        public SelectList Type;
        public string TypeString { get; set; }
        public SelectList SuaName;
        public string S1String { get; set; }
        public SelectList SubName;
        public string S2String { get; set; }
        public SelectList SucName;
        public string S3String { get; set; }

    }
}
