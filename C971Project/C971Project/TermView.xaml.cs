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
    public partial class TermView : ContentPage
    {
        Term currentTerm;
        public TermView(Term term)
        {
            InitializeComponent();
            currentTerm = term;
            AddCourseBtn.Clicked += AddCourseClicked;
            DeleteButton.Clicked += DeleteButtonClicked;
            EditButton.Clicked += async (object sender, EventArgs e) => await Navigation.PushAsync(new AddEditTerm(currentTerm));
        }

        public async void AddCourseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditCourse(currentTerm));
        }

        public async void DeleteButtonClicked(object sender, EventArgs e)
        {
            bool delete = await DisplayAlert("Confirm Term Deletion", String.Format("Are you sure you would like to delete the term '{0}'?", currentTerm.Name), "Delete", "Cancel");
            if (delete)
            {
                await App.getDatabase.DeleteTerm(currentTerm);
                await Navigation.PopAsync();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            currentTerm = await App.getDatabase.GetTerm(currentTerm.TermID);
            TermLabel.Text = String.Format("Viewing Term: '{0}'", currentTerm.Name);
            DateLabel.Text = String.Format("{0} -> {1}", currentTerm.StartDate.ToShortDateString(), currentTerm.EndDate.ToShortDateString());
            List<Course> courses = await App.getDatabase.GetCourses(currentTerm.TermID);
            AddCourseBtn.IsVisible = true;
            if(courses.Count > 5)
            {
                AddCourseBtn.IsVisible = false;
            }
            CourseStack.Children.Clear();
            foreach(Course course in courses)
            {
                Console.WriteLine(String.Format("Adding Course '{0}' to CourseStack", course.Name));              
                Color bgColor = Color.FromRgb(130, 188, 212);
                Color textColor = Color.White;
                Button newCourse = new Button
                {
                    Text = course.Name,
                    BorderColor = Color.FromHex("#000000"),
                    BackgroundColor = bgColor,
                    TextColor = textColor,
                    HeightRequest = 85,
                    FontSize = 25,
                    BorderWidth = 2,
                    CornerRadius = 10
                };
                newCourse.Clicked += async (object sender, EventArgs e) => await Navigation.PushAsync(new CourseView(course));
                CourseStack.Children.Add(newCourse);
            }
            if (App.firstAppearance)
            {
                App.firstAppearance = false;
                await Navigation.PushAsync(new PinView());
            }
        }
    }
}