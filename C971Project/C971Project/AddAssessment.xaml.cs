using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAssessment : ContentPage
    {
        Course currentCourse;
        AssessmentType assessmentType;
        string newAssessmentName;
        DateTime newAssessmentStart;
        DateTime newAssessmentEnd;
        int courseID;
        Assessment newAssessment = new Assessment();
        Assessment editAssessment;
        public AddAssessment(Course course, AssessmentType type)
        {
            InitializeComponent();
            currentCourse = course;
            assessmentType = type;         
            courseID = currentCourse.CourseId;          
            newAssessment.Type = assessmentType.ToString();
            newAssessment.CourseID = courseID;
            SaveAssessment.Clicked += SaveAssessmentClicked;

        }
        public AddAssessment(Assessment assessment)
        {
            InitializeComponent();
            editAssessment = assessment;
            AssessmentName.Text = editAssessment.Name;
            StartDate.Date = editAssessment.StartDate;
            EndDate.Date = editAssessment.EndDate;
            SaveAssessment.Clicked += SaveAssessmentClicked;

        }
        private async void SaveAssessmentClicked(object sender, EventArgs e)
        {
            if (editAssessment != null)
            {
                editAssessment.Name = AssessmentName.Text;
                editAssessment.StartDate = StartDate.Date;
                editAssessment.EndDate = EndDate.Date;
                await App.getDatabase.SaveAssessment(editAssessment);
                await Navigation.PopAsync();
            }
            else
            {
                newAssessmentName = AssessmentName.Text;
                newAssessmentStart = StartDate.Date;
                newAssessmentEnd = StartDate.Date;
                newAssessment.Name = newAssessmentName;
                newAssessment.StartDate = newAssessmentStart;
                newAssessment.EndDate = newAssessmentEnd;
                await App.getDatabase.SaveAssessment(newAssessment);
                await Navigation.PopAsync();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (App.firstAppearance)
            {
                App.firstAppearance = false;
                await Navigation.PushAsync(new PinView());
            }
        }
    }
}