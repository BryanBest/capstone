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
    public partial class AddEditCourse : ContentPage
    {
        Term currentTerm;
        Course editCourse;
        public AddEditCourse(Term term)
        {
            InitializeComponent();
            TitleBar.Text = "Add Course";
            currentTerm = term;
            EndPicker.Date = DateTime.Today.AddDays(1);
            SaveButton.Clicked += SaveButtonClicked;
        }

        public AddEditCourse(Course course)
        {
            InitializeComponent();
            TitleBar.Text = "Edit Course";
            editCourse = course;
            NameEntry.Text = editCourse.Name;
            StartPicker.Date = editCourse.StartDate;
            EndPicker.Date = editCourse.EndDate;
            InstructorNameEntry.Text = editCourse.InstructorName;
            InstructorPhoneEntry.Text = editCourse.InstructorPhone;
            InstructorEmailEntry.Text = editCourse.InstructorEmail;
            StatusPicker.SelectedItem = editCourse.Status;
            NotesEntry.Text = editCourse.Notes;
            SaveButton.Clicked += SaveButtonClicked;
        }

        public async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(InstructorEmailEntry.Text) || String.IsNullOrWhiteSpace(InstructorPhoneEntry.Text) || String.IsNullOrWhiteSpace(InstructorNameEntry.Text) || String.IsNullOrWhiteSpace(NameEntry.Text) || String.IsNullOrWhiteSpace(StatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Error", "Course name, status, and instructor information are all required fields.", "OK");
                return;
            }else if(DateTime.Compare(StartPicker.Date, EndPicker.Date) >= 0)
            {
                await DisplayAlert("Error", "Start Date must be before End Date!", "OK");
                return;
            }else if(editCourse!=null){
                editCourse.Name = NameEntry.Text;
                editCourse.StartDate = StartPicker.Date;
                editCourse.EndDate = EndPicker.Date;
                editCourse.InstructorName = InstructorNameEntry.Text;
                editCourse.InstructorPhone = InstructorPhoneEntry.Text;
                editCourse.InstructorEmail = InstructorEmailEntry.Text;
                editCourse.Status = StatusPicker.SelectedItem.ToString();
                editCourse.Notes = NotesEntry.Text;
                await App.getDatabase.SaveCourse(editCourse);
                await Navigation.PopAsync();
            }
            else
            {
                Course newCourse = new Course();
                newCourse.Name = NameEntry.Text;
                newCourse.StartDate = StartPicker.Date;
                newCourse.EndDate = EndPicker.Date;
                newCourse.InstructorName = InstructorNameEntry.Text;
                newCourse.InstructorPhone = InstructorPhoneEntry.Text;
                newCourse.InstructorEmail = InstructorEmailEntry.Text;
                newCourse.Status = StatusPicker.SelectedItem.ToString();
                newCourse.Notes = NotesEntry.Text;
                newCourse.TermId = currentTerm.TermID;
                await App.getDatabase.SaveCourse(newCourse);
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