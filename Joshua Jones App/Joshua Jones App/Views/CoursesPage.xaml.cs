using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Joshua_Jones_App.Models.DataBase;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        Course course = new Course();

        int termId = 0;
        int userId = 0;
        int courseId = 0;
        public CoursesPage(int uId, int tId)
        {
            InitializeComponent();

            termId = tId;
            userId = uId;

            var db = new SQLiteConnection(_dbPath);

            db.CreateTable<Course>();

            CourseListView.ItemsSource = from p in db.Table<Course>()
                                       where (p.termId == termId)
                                       select p;

        }
        private void CourseListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            course = (Course)e.SelectedItem;
            courseId = course.courseId;
        }

        private async void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditCoursesPage(userId, termId));
        }

        private async void EditCourseButton_Clicked(object sender, EventArgs e)
        {
            if (courseId == 0)
            {
                await DisplayAlert(null, "You must select an item before editing...", "OK");
            }
            else
            {
                await Navigation.PushAsync(new AddEditCoursesPage(userId, termId, courseId));
            }
        }

        private async void TermPageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermPage(userId));
        }

        private async void ViewAssessmentsButton_Clicked(object sender, EventArgs e)
        {
            if (courseId == 0)
            {
                await DisplayAlert(null, "You must select an item before editing...", "OK");
            }
            else
            {
                await Navigation.PushAsync(new AssessmentsPage(userId, termId, courseId));
            }
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(_dbPath);

            if (courseId == 0)
            {
                await DisplayAlert(null, "You must select an item before deleting...", "OK");
            }
            else
            {
                var termVar = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                termVar.courseAmounts = termVar.courseAmounts - 1;

                db.Update(termVar);

                db.Table<Course>().Delete(x => x.courseId == courseId);

                await Navigation.PushAsync(new CoursesPage(userId, termId));
            }
        }
    }
}