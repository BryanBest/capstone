using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinView : ContentPage
    {
        char[] pin = MainPage.pin;
        public static Func<IList<char>, bool> ValidatorFunc
        {
            get;
            set;
        }
        public PinView()
        {
            InitializeComponent();
            ValidatorFunc = x => x.SequenceEqual(pin);
            PinViewItem.Validator = ValidatorFunc;
        }
        public async void Handle_Success(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}