using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace C971Project
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public DateTime StartDate
        {
            get;
            set;
        }
        public DateTime EndDate
        {
            get;
            set;
        }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<Course> Courses
        {
            get;
            set;
        }
    }
}
