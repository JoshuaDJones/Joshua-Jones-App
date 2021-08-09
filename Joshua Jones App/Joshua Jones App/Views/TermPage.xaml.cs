using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Joshua_Jones_App.Models.DataBase;
using Plugin.LocalNotifications;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        Term term = new Term();

        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");
            
        int userId = 0;
        int termId = 0;
        public TermPage(int id)
        {
            InitializeComponent();

            userId = id;

            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Term>();

            termListView.ItemsSource =  from p in db.Table<Term>()
                                        where (p.loginId == userId)
                                        select p;
        }

        protected override void OnAppearing()
        {
            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Notification>();

            var notificationVar = from p in db.Table<Notification>()
                                  where (p.userId == userId)
                                  select p;

            foreach (var item in notificationVar)
            {
                if (DateTime.Now.Date == item.notificationStartDate)
                {
                    CrossLocalNotifications.Current.Show(item.notificationTitle, item.notificationBody);
                }
            }
        }
        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditTerm(userId));
        }

        private void TermListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            term = (Term)e.SelectedItem;
            termId = term.termId;
        }
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {

            if (termId == 0)
            {
                await DisplayAlert(null, "You must select an item before deleting...", "OK");
            }
            else
            {
                var db = new SQLiteConnection(_dbPath);

                db.Table<Term>().Delete(x => x.termId == term.termId);

                await Navigation.PushAsync(new TermPage(userId));
            }
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            if (termId == 0)
            {
                await DisplayAlert(null, "You must select an item before editing...", "OK");
            }
            else
            {
                await Navigation.PushAsync(new AddEditTerm(userId, termId));
            }
        }

        private async void ModifyCoursesButton_Clicked(object sender, EventArgs e)
        {
            if (termId == 0)
            {
                await DisplayAlert(null, "You must select an item before editing...", "OK");
            }
            else
            {
                await Navigation.PushAsync(new CoursesPage(userId, termId));
            }
        }

        private async void backToLoginPageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}