using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Joshua_Jones_App.Models.DataBase;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Login>();

            var maxPk = db.Table<Login>().OrderByDescending(c => c.loginId).FirstOrDefault();

            Login login = new Login()
            {
                loginId = (maxPk == null ? 1 : maxPk.loginId + 1),
                loginName = newUsername.Text,
                loginPassword = newPassword.Text
            };

            db.Insert(login);
            await DisplayAlert(null, login.loginName + " Saved", "OK");
            await Navigation.PushAsync(new LoginPage());

        }
    }
}