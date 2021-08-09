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

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        public LoginPage()
        {
            InitializeComponent();

            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Login>();

            var maxPk = db.Table<Login>().OrderByDescending(c => c.loginId).FirstOrDefault();
            try
            {
                if (maxPk.loginName != "test") ;
            }
            catch
            {
                Login login = new Login()
                {
                    loginId = 1,
                    loginName = "test",
                    loginPassword = "test"
                };

                db.Insert(login);

                //Creating term table and filling with test data
                db.CreateTable<Term>();

                Term term = new Term()
                {
                    termId = 1,
                    termName = "Spring 2021",
                    loginId = 1,
                    courseAmounts = 1,
                    startTermDate = DateTime.Now.Date,
                    endTermDate = DateTime.Now.Date.AddDays(100)
                };

                db.Insert(term);

                //creating course table and filling with test data
                db.CreateTable<Course>();

                Course course = new Course()
                {
                    courseId = 1,
                    termId = 1,
                    courseName = "Physics",
                    startCourseDate = DateTime.Now.Date,
                    endCourseDate = DateTime.Now.Date.AddDays(100),
                    courseStatus = "progress",
                    courseNote = "Do assignment",
                    instructorName = "Joshua D Jones",
                    instructorsEmail = "jdj92993@gmail.com",
                    instructorsPhoneNumber = "15709025045"
                };

                db.Insert(course);

                db.CreateTable<Assessment>();

                Assessment assessment = new Assessment()
                {
                    assessmentId = 1,
                    courseId = 1,
                    assessmentName = "Physics Project",
                    assessmentType = "Objective Assessment",
                    assessmentDueDate = DateTime.Now.Date.AddDays(100)
                };

                db.Insert(assessment);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(_dbPath);

            string user = null;
            string pass = null;
            int id = 0;

            var person = from p in db.Table<Login>()
                          where (p.loginName == userNameEntry.Text) && (p.loginPassword == passwordEntry.Text)
                          select p;

            foreach (var item in person)
            {
                user = item.loginName.ToString();
                pass = item.loginPassword.ToString();
                id = item.loginId;
            };

            if (user == null)
            {
                await DisplayAlert(null, "That Login was not found...", "OK");
            }
            else if (user != null)
            {
                await Navigation.PushAsync(new TermPage(id));
            }
        }

        private async void Button_Clicked1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}