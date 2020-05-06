using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;

namespace C971Project
{
    public partial class App : Application
    {
        public static bool firstAppearance { get; set; } = true;
        
        
        public App()
        {
            
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromRgb(193, 193, 193)
            };       
        }

        protected async override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            firstAppearance = true;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        static Database database;
        public static Database getDatabase
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CourseTrackerDB.db3"));
                }
                return database;
            }
        }
    }
}
