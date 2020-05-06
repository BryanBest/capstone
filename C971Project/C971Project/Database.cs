using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace C971Project
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;
        //public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ProtectionCompleteUnlessOpen;
        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }
        public Task<int> SaveTerm(Term term)
        {
            if (term.TermID != 0)
            {
                return _database.UpdateAsync(term);
            }
            else
            {
                return _database.InsertAsync(term);
            }
        }

        public Task<int> SaveCourse(Course course)
        {
            if (course.CourseId != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        public Task<int> SaveAssessment(Assessment assessment)
        {
            if(assessment.AssessmentId != 0)
            {
                return _database.UpdateAsync(assessment);
            }
            else
            {
                return _database.InsertAsync(assessment);
            }
        }

        public Task<int> SaveUser(User user)
        {
            return _database.InsertAsync(user);
        }
        public Task<int> DeleteTerm(Term term)
        {
            return _database.DeleteAsync(term);
        }

        public Task<int> DeleteCourse(Course course)
        {
            return _database.DeleteAsync(course);
        }

        public Task<int> DeleteAssessment(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }
        public void Purge()
        {
            _database.DropTableAsync<Course>();
            _database.DropTableAsync<Term>();
            _database.DropTableAsync<Assessment>();
            _database.DropTableAsync<User>();
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
            _database.CreateTableAsync<User>().Wait();

        }

        public Task<List<Term>> GetTerms()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<Term> GetTerm(int id)
        {
            return _database.Table<Term>().Where(i => i.TermID == id).FirstOrDefaultAsync();
        }

        public Task<List<Course>> GetCourses(int termID)
        {
            return _database.Table<Course>().Where(i => i.TermId == termID).ToListAsync();
        }

        public Task<Course> GetCourse(int courseID)
        {
            return _database.Table<Course>().Where(i => i.CourseId == courseID).FirstOrDefaultAsync();
        }

        public Task<List<Assessment>> GetAssessments(int courseId)
        {
            return _database.Table<Assessment>().Where(i => i.CourseID == courseId).ToListAsync();
        }

        public Task<Assessment> GetAssessment(int id)
        {
            return _database.Table<Assessment>().Where(i => i.AssessmentId == id).FirstOrDefaultAsync();
        }

        public Task<List<User>> GetUsers()
        {
            return _database.Table<User>().ToListAsync();
        }
    }
}
