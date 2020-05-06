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
    public partial class AddEditTerm : ContentPage
    {
        Term editTerm;
        public AddEditTerm()
        {
            InitializeComponent();
            SaveTerm.Clicked += SaveTermClicked;
            EndDate.Date = DateTime.Now.AddDays(1);
        }

        public AddEditTerm(Term term)
        {
            InitializeComponent();
            editTerm = term;
            TermName.Text = term.Name;
            StartDate.Date = term.StartDate;
            EndDate.Date = term.EndDate;
            TitleBar.Text = "Edit Term";
            SaveTerm.Clicked += SaveTermClicked;
        }

        public async void SaveTermClicked(object sender, System.EventArgs e)
        {
            if(DateTime.Compare(StartDate.Date, EndDate.Date) >= 0)
            {
                await DisplayAlert("Error", "Start Date must be before End Date!", "OK");
                return;
            }else if(String.IsNullOrWhiteSpace(TermName.Text)){
                await DisplayAlert("Error", "Please enter a Term Name.", "OK");
                return;
            }
            if(editTerm == null)
            {
                Term newTerm = new Term();
                newTerm.Name = TermName.Text;
                newTerm.StartDate = StartDate.Date;
                newTerm.EndDate = EndDate.Date;
                await App.getDatabase.SaveTerm(newTerm);
                await Navigation.PopAsync();
            }
            else
            {
                editTerm.Name = TermName.Text;
                editTerm.StartDate = StartDate.Date;
                editTerm.EndDate = EndDate.Date;
                await App.getDatabase.SaveTerm(editTerm);
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