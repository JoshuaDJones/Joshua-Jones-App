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
    public partial class AddEditAssessmentsPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        int userId = 0;
        int termId = 0;
        int courseId = 0;
        int assessmentId = 0;
        int objExists = 0;
        int perExists = 0;
        int courseOrAssessment = 1;

        public AddEditAssessmentsPage(int uId, int tId, int cId)
        {
            InitializeComponent();

            var db = new SQLiteConnection(_dbPath);

            userId = uId;
            termId = tId;
            courseId = cId;

            var typeOfAssessment = from p in db.Table<Assessment>()
                                   where (p.courseId == courseId)
                                   select p;

            foreach (var item in typeOfAssessment)
            {
                if (item.assessmentType == "Objective Assessment")
                {
                    objExists = 1;
                }
                else if (item.assessmentType == "Performance Assessment")
                {
                    perExists = 1;
                }
            }

            if (perExists == 1 && objExists == 0)
            {
                var assessmentType = new List<string>();

                assessmentType.Add("Objective Assessment");

                assessmentTypePicker.Title = "Select the status of the course...";
                assessmentTypePicker.ItemsSource = assessmentType;
            }
            else if (perExists == 0 && objExists == 1)
            {
                var assessmentType = new List<string>();

                assessmentType.Add("Performance Assessment");

                assessmentTypePicker.Title = "Select the status of the course...";
                assessmentTypePicker.ItemsSource = assessmentType;
            }
            else if (perExists == 0 && objExists == 0)
            {
                var assessmentType = new List<string>();

                assessmentType.Add("Performance Assessment");
                assessmentType.Add("Objective Assessment");

                assessmentTypePicker.Title = "Select the status of the course...";
                assessmentTypePicker.ItemsSource = assessmentType;
            }
        }
            public AddEditAssessmentsPage(int uId, int tId, int cId, int aId)
        {
            InitializeComponent();

            var db = new SQLiteConnection(_dbPath);

            userId = uId;
            termId = tId;
            courseId = cId;
            assessmentId = aId;

            var typeOfAssessment = from p in db.Table<Assessment>()
                                   where (p.courseId == courseId)
                                   select p;

            foreach (var item in typeOfAssessment)
            {
                if (item.assessmentType == "Objective Assessment")
                {
                    objExists = 1;
                }
                else if (item.assessmentType == "Performance Assessment")
                {
                    perExists = 1;
                }
            }


            var assessmentVar = db.Table<Assessment>().FirstOrDefault(c => c.assessmentId == assessmentId);

            if(assessmentVar.assessmentType.ToString() == "Performance Assessment")
            {
                var assessmentType = new List<string>();

                if(objExists == 0)
                {
                    assessmentType.Add("Objective Assessment");
                }

                assessmentType.Add("Performance Assessment");

                assessmentTypePicker.Title = "Select the status of the course...";
                assessmentTypePicker.ItemsSource = assessmentType;
            }
            else if (assessmentVar.assessmentType.ToString() == "Objective Assessment")
            {
                var assessmentType = new List<string>();

                if (perExists == 0)
                {
                    assessmentType.Add("Performance Assessment");
                }

                assessmentType.Add("Objective Assessment");

                assessmentTypePicker.Title = "Select the status of the course...";
                assessmentTypePicker.ItemsSource = assessmentType;
            }

            assessmentNameEntry.Text = assessmentVar.assessmentName.ToString();
            assessmentTypePicker.SelectedItem = assessmentVar.assessmentType.ToString();
            dueDatePicker.Date = assessmentVar.assessmentDueDate;

        }
        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (assessmentNameEntry.Text == null || assessmentTypePicker.SelectedItem == null)
            {
                await DisplayAlert(null, "Please make sure that all fields are completed before saving...", "OK");
            }
            else
            {
                if (assessmentId == 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    db.CreateTable<Assessment>();

                    var maxPk = db.Table<Assessment>().OrderByDescending(c => c.assessmentId).FirstOrDefault();

                    assessmentId = (maxPk == null ? 1 : maxPk.assessmentId + 1);

                    Assessment assessment = new Assessment()
                    {
                        assessmentId = assessmentId,
                        courseId = courseId,
                        assessmentName = assessmentNameEntry.Text,
                        assessmentType = assessmentTypePicker.SelectedItem.ToString(),
                        assessmentDueDate = dueDatePicker.Date
                    };

                    db.Insert(assessment);

                    await DisplayAlert(null, assessment.assessmentName + " Saved", "OK");

                    await Navigation.PushAsync(new AssessmentsPage(userId, termId, courseId));
                }
                else if (assessmentId != 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    var assessmentVar = db.Table<Assessment>().OrderByDescending(c => c.assessmentId).FirstOrDefault();

                    assessmentVar.assessmentId = assessmentId;
                    assessmentVar.courseId = courseId;
                    assessmentVar.assessmentName = assessmentNameEntry.Text;
                    assessmentVar.assessmentType = assessmentTypePicker.SelectedItem.ToString();
                    assessmentVar.assessmentDueDate = dueDatePicker.Date;

                    db.Update(assessmentVar);

                    await Navigation.PushAsync(new AssessmentsPage(userId, termId, courseId));
                }
            }
        }

        private async void setNotificationButton_Clicked(object sender, EventArgs e)
        {
            if (assessmentNameEntry.Text == null || assessmentTypePicker.SelectedItem == null)
            {
                await DisplayAlert(null, "Please make sure that all fields are completed before saving...", "OK");
            }
            else
            {
                if (assessmentId == 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    db.CreateTable<Assessment>();

                    var maxPk = db.Table<Assessment>().OrderByDescending(c => c.assessmentId).FirstOrDefault();

                    assessmentId = (maxPk == null ? 1 : maxPk.assessmentId + 1);

                    Assessment assessment = new Assessment()
                    {
                        assessmentId = assessmentId,
                        courseId = courseId,
                        assessmentName = assessmentNameEntry.Text,
                        assessmentType = assessmentTypePicker.SelectedItem.ToString(),
                        assessmentDueDate = dueDatePicker.Date
                    };

                    db.Insert(assessment);

                    await DisplayAlert(null, assessment.assessmentName + " Saved", "OK");

                    await Navigation.PushAsync(new CreateNotificationPage(userId, termId, courseId, courseOrAssessment));
                }
                else if (assessmentId != 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    var assessmentVar = db.Table<Assessment>().OrderByDescending(c => c.assessmentId).FirstOrDefault();

                    assessmentVar.assessmentId = assessmentId;
                    assessmentVar.courseId = courseId;
                    assessmentVar.assessmentName = assessmentNameEntry.Text;
                    assessmentVar.assessmentType = assessmentTypePicker.SelectedItem.ToString();
                    assessmentVar.assessmentDueDate = dueDatePicker.Date;

                    db.Update(assessmentVar);

                    await Navigation.PushAsync(new CreateNotificationPage(userId, termId, courseId, courseOrAssessment));
                }
            }
        }
    }
}