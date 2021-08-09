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
    public partial class AssessmentsPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        Assessment assessment = new Assessment();

        int userId = 0;
        int termId = 0;
        int courseId = 0;
        int assessmentId = 0;
        int objectiveExists = 0;
        int performanceExists = 0;
        public AssessmentsPage(int uId, int tId, int cId)
        {
            InitializeComponent();

            userId = uId;
            termId = tId;
            courseId = cId;

            var db = new SQLiteConnection(_dbPath);

            var typeOfAssessment = from p in db.Table<Assessment>()
                                      where (p.courseId == courseId)
                                      select p;

            foreach(var item in typeOfAssessment)
            {
                if(item.assessmentType == "Objective Assessment")
                {
                    objectiveExists = 1;
                }
                else if(item.assessmentType == "Performance Assessment")
                {
                    performanceExists = 1;
                }
            }

            if(performanceExists == 1 && objectiveExists == 1)
            {
                newAssessmentButton.IsEnabled = false;
            }

            db.CreateTable<Assessment>();

            AssessmentListView.ItemsSource = from p in db.Table<Assessment>()
                                         where (p.courseId == courseId)
                                         select p;
        }

        private void AssessmentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            assessment = (Assessment)e.SelectedItem;
            assessmentId = assessment.assessmentId;
        }

        private async void NewAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditAssessmentsPage(userId, termId, courseId));
        }

        private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
        {
            if (assessmentId == 0)
            {
                await DisplayAlert(null, "You must select an item before editing...", "OK");
            }
            else
            {
                await Navigation.PushAsync(new AddEditAssessmentsPage(userId, termId, courseId, assessmentId));
            }
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            if (assessmentId == 0)
            {
                await DisplayAlert(null, "You must select an item before deleting...", "OK");
            }
            else
            {
                var db = new SQLiteConnection(_dbPath);

                db.Table<Assessment>().Delete(x => x.assessmentId == assessmentId);

                await Navigation.PushAsync(new AssessmentsPage(userId, termId, courseId));
            }
        }
        private async void backToCoursePageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage(userId, termId));
        }
    }
}