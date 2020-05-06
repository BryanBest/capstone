using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void EnterAndCheckAllData()
        {
            AddTerm("Term 1");
            var termExists = app.Query(x => x.Marked("TermStack").Child().Property("text").Contains("Term 1")).Any();
            Assert.IsTrue(termExists, "The term was not created successfully.");
            app.Screenshot("Term Added");
            app.Tap(x => x.Marked("TermStack").Child().Property("text").Contains("Term 1"));
            AddCourse("Course 1", "John Doe", "123-465-7899", "john@email.com", "In Progress", "Notes test");
            var courseExists = app.Query(x => x.Marked("CourseStack").Child().Property("text").Contains("Course 1")).Any();
            Assert.IsTrue(courseExists, "The course was not created successfully.");
            app.Screenshot("Course Added");
            app.Tap(x => x.Marked("CourseStack").Child().Property("text").Contains("Course 1").Descendant(0));
            CheckAndTap("Assessments", false);
            CheckAndTap("Add Assessment", true);
            AddAssessment("Assessment 1");
            app.WaitForElement(x => x.Marked("AssessmentList"));
            var assessmentExists = app.Query(x => x.Marked("Assessment 1")).Any();
            Assert.IsTrue(assessmentExists, "The assessment was not created successfully.");
            app.Screenshot("Assessment Added");
        }

        [Test]
        public void SearchForAssessment()
        {
            AddTerm("Term 2");
            app.Tap(x => x.Marked("TermStack").Child().Property("text").Contains("Term 2"));
            AddCourse("Course 2", "Jane Doe", "123-456-7899", "Jane@email.com", "Completed", "Notes test");
            app.Tap(x => x.Marked("CourseStack").Child().Property("text").Contains("Course 2").Descendant(0));
            CheckAndTap("Assessments", true);
            CheckAndTap("Add Assessment", true);
            AddAssessment("Assessment 1");
            CheckAndTap("Add Assessment", true);
            AddAssessment("Assessment 2");
            CheckAndTap("Add Assessment", true);
            AddAssessment("Assessment 3");
            Search("Assessment 2");
            Assert.IsTrue(app.Query(x => x.Marked("AssessmentList").Child()).Length == 1, "More than one Assessment was found by the search.");
            Search("Assessment 4");
            Assert.IsTrue(app.Query(x => x.Marked("AssessmentList").Child()).Length == 0, "Search returned a value when incorrect query was entered.");
        }


        public void CheckAndTap(string marked, bool check)
        {
            if (check)
            {
                app.WaitForElement(x => x.Marked(marked));
            }         
            app.Tap(x => x.Marked(marked));
        }

        public void AddTerm(string termName)
        {
            CheckAndTap("Add Term", true);
            CheckAndTap("Enter Term Name", false);
            app.EnterText(termName);
            CheckAndTap("Save", false);
        }

        public void AddCourse(string courseName, string instructorName, string phoneNumber, string email, string status, string notes)
        {
            CheckAndTap("Add Course", true);
            CheckAndTap("Enter Course Name", false);
            app.EnterText(courseName);
            CheckAndTap("Enter Instructor Name", false);
            app.EnterText(instructorName);
            CheckAndTap("Enter Phone Number", false);
            app.EnterText(phoneNumber);
            CheckAndTap("Enter Email", false);
            app.EnterText(email);
            CheckAndTap("Select Course Status", false);
            CheckAndTap(status, false);
            CheckAndTap("Enter Notes", false);
            app.EnterText(notes);
            app.DismissKeyboard();
            CheckAndTap("Save", false);
        }

        public void AddAssessment(string assessmentName)
        {
            CheckAndTap("Enter Assessment Name", true);
            app.EnterText(assessmentName);
            app.DismissKeyboard();
            CheckAndTap("Save", false);
        }

        public void Search(string query)
        {
            CheckAndTap("SearchBar", true);
            app.ClearText();
            app.EnterText(query);
        }
    }
}
