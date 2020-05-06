using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Xml;
using System.Runtime.CompilerServices;

namespace C971Project
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    public enum AssessmentType { Objective, Performance};
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                TestButton.Clicked += createEvaluationData;

            }
            catch(System.AggregateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditTerm());
        }
        public static List<Term> terms;
        public static List<User> userList = new List<User>();
        public static char[] pin;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Purge.Clicked += async (object sender, EventArgs e) => App.getDatabase.Purge();
            TermStack.Children.Clear();
            terms = await App.getDatabase.GetTerms();
            userList = await App.getDatabase.GetUsers();
            foreach(Term term in terms)
            {
                Color bgColor = Color.FromRgb(130, 188, 212);
                Color textColor = Color.White;
                Button newTerm = new Button
                {
                    Text = String.Format("{0} ({1} to {2})", term.Name, term.StartDate.ToShortDateString(), term.EndDate.ToShortDateString()),
                    BorderColor = Color.FromHex("#000000"),
                    BackgroundColor = bgColor,
                    TextColor = textColor,
                    HeightRequest = 85,
                    FontSize = 25,
                    BorderWidth = 2,
                    CornerRadius = 10
                };
                newTerm.Clicked += async (object sender, EventArgs args) => await Navigation.PushAsync(new TermView(term));
                TermStack.Children.Add(newTerm);
            }

            if (userList.Count == 0)
            {
                User user = new User();
                string name = null;
                string pass = null;
                while (true)
                {
                    name = await DisplayPromptAsync("Setup", "Welcome to AcadAgenda! Please enter your name.");
                    if (String.IsNullOrWhiteSpace(name))
                    {
                        await DisplayAlert("Error", "Please enter a valid name.", "Ok");
                    }
                    else
                    {
                        break;
                    }
                }
                while (true)
                {
                    pass = await DisplayPromptAsync("Setup", "Hi, " + name + "! Please enter a 4 digit pin.", keyboard: Keyboard.Numeric, maxLength: 4);
                    bool isNumber = true;
                    if (!String.IsNullOrWhiteSpace(pass))
                    {
                        foreach (char c in pass.ToCharArray())
                        {
                            if (!char.IsDigit(c))
                            {
                                isNumber = false;
                            }
                        }
                    }               
                    if(String.IsNullOrWhiteSpace(pass) || !isNumber || pass.Length!=4)
                    {
                        await DisplayAlert("Error", "Plesae enter a valid pin.", "Ok");
                    }
                    else
                    {
                        pin = pass.ToCharArray();
                        break;
                    }
                }
                user.Name = name;
                user.Pass = pass;
                await App.getDatabase.SaveUser(user);
                App.firstAppearance = false;
            }
            else
            {
                pin = userList[0].Pass.ToCharArray();
                if (App.firstAppearance)
                {
                    App.firstAppearance = false;
                    await Navigation.PushAsync(new PinView());
                }
            }
        }

        private async void createEvaluationData(object sender, EventArgs e)
        {
            Term evalTerm = new Term();
            evalTerm.Name = "Term 2";
            evalTerm.StartDate = DateTime.Now;
            evalTerm.EndDate = DateTime.Now.AddMonths(1);
            await App.getDatabase.SaveTerm(evalTerm);
            Course evalCourse = new Course();
            evalCourse.Name = "Mobile Application Development Using C#";
            evalCourse.StartDate = DateTime.Now;
            evalCourse.EndDate = DateTime.Now.AddMonths(1);
            evalCourse.InstructorName = "Bryan Best";
            evalCourse.InstructorPhone = "732-208-1615";
            evalCourse.InstructorEmail = "bbest9@wgu.edu";
            evalCourse.Status = "In Progress";
            evalCourse.Notes = "Hope I pass this PA!";
            evalCourse.TermId = evalTerm.TermID;
            await App.getDatabase.SaveCourse(evalCourse);
            Assessment evalOA = new Assessment();
            evalOA.Name = "Exam";
            evalOA.StartDate = DateTime.Now;
            evalOA.EndDate = DateTime.Now.AddDays(1);
            evalOA.CourseID = evalCourse.CourseId;
            evalOA.Type = "Objective";
            await App.getDatabase.SaveAssessment(evalOA);
            Assessment evalPA = new Assessment();
            evalPA.Type = "Performance";
            evalPA.Name = "Make an App";
            evalPA.StartDate = DateTime.Now;
            evalPA.EndDate = DateTime.Now.AddDays(1);
            evalPA.CourseID = evalCourse.CourseId;
            await App.getDatabase.SaveAssessment(evalPA);
            OnAppearing();
        }

        private Button createTermButton()
        {
            Button newTerm = new Button
            {
                Text = "Term 1",
                BorderColor = Color.FromHex("#000000"),
                HeightRequest = 85,
                FontFamily = "Tahoma",
                FontSize = 25,
                BorderWidth = 2
            };
            return newTerm;
        }
    }
}
