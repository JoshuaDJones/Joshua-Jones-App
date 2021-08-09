using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Joshua_Jones_App.Models.DataBase;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNotificationPage : ContentPage
    {

        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        int termId = 0;
        int userId = 0;
        int courseId = 0;
        int notificationId = 0;
        int courseOrPage = 0;
        // 1=Assessment 2=course
        public CreateNotificationPage(int uId, int tId, int cId, int cOrAPage)
        {
            InitializeComponent();

            termId = tId;
            userId = uId;
            courseId = cId;
            courseOrPage = cOrAPage;

        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Notification>();

            var maxPk = db.Table<Notification>().OrderByDescending(c => c.notificationId).FirstOrDefault();

            notificationId = (maxPk == null ? 1 : maxPk.notificationId + 1);

            Notification notification = new Notification()
            {
                notificationId = notificationId,
                userId = userId,
                notificationTitle = notificationTitleEntry.Text,
                notificationBody = notificationBodyEntry.Text,
                notificationStartDate = notificationDatePicker.Date
            };

            db.Insert(notification);

            await DisplayAlert(null, notification.notificationTitle + " Saved", "OK");

            if (courseOrPage == 1)
            {
                await Navigation.PushAsync(new AssessmentsPage(userId, termId, courseId));
            }
            else if(courseOrPage == 2)
            {
                await Navigation.PushAsync(new CoursesPage(userId, termId));
            }
        }
    }
}