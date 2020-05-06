using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;
using System.Collections.ObjectModel;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentView : ContentPage
    {
        List<Assessment> assessments = new List<Assessment>();
        ObservableCollection<Assessment> obAssessments = new ObservableCollection<Assessment>();
        Course currentCourse;
        public AssessmentView(Course course)
        {
            InitializeComponent();
            currentCourse = course;
        }

        private async void Refresh()
        {
            AssessmentList.ItemsSource = null;
            assessments = await App.getDatabase.GetAssessments(currentCourse.CourseId);
            obAssessments.Clear();
            foreach(Assessment assessment in assessments)
            {
                obAssessments.Add(assessment);
            }
            AssessmentList.ItemsSource = obAssessments;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            assessments = await App.getDatabase.GetAssessments(currentCourse.CourseId);
            Refresh();
            if (App.firstAppearance)
            {
                App.firstAppearance = false;
                await Navigation.PushAsync(new PinView());
            }
        }

        void OnSearchChanged(object sender, EventArgs e)
        {
            obAssessments.Clear();
            foreach(Assessment assessment in assessments)
            {
                if (assessment.Name.ToLower().Contains(SearchBar.Text.ToLower()))
                {
                    obAssessments.Add(assessment);
                }
            }
        }

        async void ItemSelected(object sender, EventArgs e)
        {
            Assessment selectedAssessment = (Assessment)AssessmentList.SelectedItem;
            string completed;
            if (selectedAssessment.Completed)
            {
                completed = "Completed";
            }
            else
            {
                completed = "In Progress";
            }
            bool select = await DisplayAlert(selectedAssessment.Name, "\nStart Date: " + selectedAssessment.StartDate.ToString("d") + "\nEnd Date: " + selectedAssessment.EndDate.ToString("d") + "\nStatus:" + completed, "Options", "Close");
            if (select)
            {
                string action = await DisplayActionSheet(selectedAssessment.Name + " Options", "Cancel", "Delete Assessment", "Mark as Completed");
                switch (action)
                {
                    case "Mark as Completed":
                        selectedAssessment.Completed = true;
                        await App.getDatabase.SaveAssessment(selectedAssessment);
                        break;
                    case "Delete Assessment":
                        bool confirm = await DisplayAlert("Deletion Confirmation", String.Format("Are you sure you would like to delete the Assessment {0}?", selectedAssessment.Name), "Delete", "Cancel");
                        if (confirm)
                        {
                            await App.getDatabase.DeleteAssessment(selectedAssessment);
                            obAssessments.Remove(selectedAssessment);
                            Refresh();
                        }
                        break;
                    default:
                        break;
                }
            }           
        }

        async void AddAssessmentButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessment(currentCourse,AssessmentType.Objective));
        }
    }
}