using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace C971Project
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId
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
        public string InstructorName
        {
            get;
            set;
        }
        public string InstructorPhone
        {
            get;
            set;
        }
        public string InstructorEmail
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public bool StartNotification
        {
            get;
            set;
        }
        public bool EndNotification
        {
            get;
            set;
        }
        [ForeignKey(typeof(Term))]
        public int TermId
        {
            get;
            set;
        }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<Assessment> Assessments
        {
            get;
            set;
        }

        public Course()
        {
            StartNotification = false;
            EndNotification = false;
        }
    }
}
