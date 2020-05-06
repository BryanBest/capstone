using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace C971Project
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Type
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
        public bool Notification
        {
            get;
            set;
        }
        public bool Completed
        {
            get;
            set;
        } = false;
        [ForeignKey(typeof(Course))]
        public int CourseID
        {
            get;
            set;
        }
    }
}
