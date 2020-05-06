using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.LocalNotifications;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseView : ContentPage
    {
        Course currentCourse;
        public CourseView(Course course)
        {
            InitializeComponent();
            currentCourse = course;
            AssessmentsButton.Clicked += AssessmentsButtonClicked;
            EditButton.Clicked += async (object sender, EventArgs e) => await Navigation.PushAsync(new AddEditCourse(currentCourse));
            DeleteButton.Clicked += DeleteButtonClicked;
            ShareButton.Clicked += ShareButtonClicked;
            StartNotificationButton.Clicked += StartNotificationButtonClicked;
            EndNotificationButton.Clicked += EndNotificationButtonClicked;
        }

        private async void AssessmentsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentView(currentCourse));
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            bool delete = await DisplayAlert("Course Deletion Confirmation", String.Format("Are you sure you'd like to delete the course '{0}'", currentCourse.Name), "Delete", "Cancel");
            if (delete)
            {
                await App.getDatabase.DeleteCourse(currentCourse);
                await Navigation.PopAsync();
            }          
        }

        private async Task ShareNotes()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = NotesLabel.Text,
                Title = "Share Notes"
            });
        }

        private async void ShareButtonClicked(object sender, EventArgs e)
        {
            await ShareNotes();
        }

        private async void StartNotificationButtonClicked(object sender, EventArgs e)
        {
            if(!currentCourse.StartNotification)
            {
                CrossLocalNotifications.Current.Show("Course Start Reminder", String.Format("Just a friendly reminder that '{0}' is scheduled to begin today!",currentCourse.Name),0,currentCourse.StartDate);
                StartNotificationButton.ImageSource = "NotificationOn.png";
                currentCourse.StartNotification = true;
                await App.getDatabase.SaveCourse(currentCourse);
            }
            else
            {
                CrossLocalNotifications.Current.Cancel(0);
                StartNotificationButton.ImageSource = "NotificationOff.png";
                currentCourse.StartNotification = false;
                await App.getDatabase.SaveCourse(currentCourse);
            }
        }

        private async void EndNotificationButtonClicked(object sender, EventArgs e)
        {
            if (!currentCourse.EndNotification)
            {
                CrossLocalNotifications.Current.Show("Course End Reminder", String.Format("Just a friendly reminder that '{0}' is scheduled to end today!", currentCourse.Name), 1, currentCourse.EndDate);
                EndNotificationButton.ImageSource = "NotificationOn.png";
                currentCourse.EndNotification = true;
                await App.getDatabase.SaveCourse(currentCourse);
            }
            else
            {
                CrossLocalNotifications.Current.Cancel(0);
                EndNotificationButton.ImageSource = "NotificationOff.png";
                currentCourse.EndNotification = false;
                await App.getDatabase.SaveCourse(currentCourse);
            }
        }

        protected override async void OnAppearing()
        {
            currentCourse = await App.getDatabase.GetCourse(currentCourse.CourseId);
            base.OnAppearing();
            CourseNameLabel.Text = currentCourse.Name;
            StartDateLabel.Text = currentCourse.StartDate.ToShortDateString();
            EndDateLabel.Text = currentCourse.EndDate.ToShortDateString();
            InstructorNameLabel.Text = currentCourse.InstructorName;
            InstructorEmailLabel.Text = currentCourse.InstructorEmail;
            InstructorPhoneLabel.Text = currentCourse.InstructorPhone;
            StatusLabel.Text = currentCourse.Status;
            NotesLabel.Text = currentCourse.Notes;
            if (currentCourse.StartNotification)
            {
                StartNotificationButton.ImageSource = "NotificationOn.png";
            }
            else
            {
                StartNotificationButton.ImageSource = "NotificationOff.png";
            }
            if (currentCourse.EndNotification)
            {
                EndNotificationButton.ImageSource = "NotificationOn.png";
            }
            else
            {
                EndNotificationButton.ImageSource = "NotificationOff.png";
            }
            if (App.firstAppearance)
            {
                App.firstAppearance = false;
                await Navigation.PushAsync(new PinView());
            }
        }
    }
}