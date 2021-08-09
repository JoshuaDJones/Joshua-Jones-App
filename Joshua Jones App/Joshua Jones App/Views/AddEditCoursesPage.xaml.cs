using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Joshua_Jones_App.Models.DataBase;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditCoursesPage : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        int userId = 0;
        int termId = 0;
        int courseId = 0;
        int courseOrAssessment = 2;
        public AddEditCoursesPage(int uId, int tId)
        {
            InitializeComponent();
            termId = tId;
            userId = uId;

            var courseStatusList = new List<string>();

            courseStatusList.Add("progress");
            courseStatusList.Add("completed");
            courseStatusList.Add("dropped");
            courseStatusList.Add("plan to take");

            courseStatusPicker.Title = "Select the status of the course...";
            courseStatusPicker.ItemsSource = courseStatusList;
        }

        public AddEditCoursesPage(int uId, int tId, int cId)
        {
            InitializeComponent();

            var db = new SQLiteConnection(_dbPath);

            var courseStatusList = new List<string>();

            courseStatusList.Add("progress");
            courseStatusList.Add("completed");
            courseStatusList.Add("dropped");
            courseStatusList.Add("plan to take");

            courseStatusPicker.Title = "Select the status of the course...";
            courseStatusPicker.ItemsSource = courseStatusList;

            termId = tId;
            userId = uId;
            courseId = cId;

            var courseVar = db.Table<Course>().FirstOrDefault(c => c.courseId == courseId);

            courseNameEntry.Text = courseVar.courseName.ToString();
            startCourseDatePicker.Date = courseVar.startCourseDate;
            endCourseDatePicker.Date = courseVar.endCourseDate;
            courseStatusPicker.SelectedItem = courseVar.courseStatus.ToString();
            courseNoteEntry.Text = courseVar.courseNote.ToString();
            courseInstructorsNameEntry.Text = courseVar.instructorName.ToString();
            courseInstructorEmailEntry.Text = courseVar.instructorsEmail.ToString();
            courseInstructorPhoneEntry.Text = courseVar.instructorsPhoneNumber.ToString();
        }

            private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (CheckForNull() == 1)
            {
                await DisplayAlert(null, "One or more of the fields are empty or invalid. Check that the end date is after the start date...", "OK");
            }
            else if(IsValidEmail(courseInstructorEmailEntry.Text) == false)
            {
                await DisplayAlert(null, "Please check that your instructors email is valid...", "OK");
            }
            else
            {

                if (courseId == 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    db.CreateTable<Course>();

                    var maxPk = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseId = (maxPk == null ? 1 : maxPk.termId + 1);

                    Course course = new Course()
                    {
                        courseId = courseId,
                        termId = termId,
                        courseName = courseNameEntry.Text,
                        startCourseDate = startCourseDatePicker.Date,
                        endCourseDate = endCourseDatePicker.Date,
                        courseStatus = courseStatusPicker.SelectedItem.ToString(),
                        courseNote = courseNoteEntry.Text,
                        instructorName = courseInstructorsNameEntry.Text,
                        instructorsEmail = courseInstructorEmailEntry.Text,
                        instructorsPhoneNumber = courseInstructorPhoneEntry.Text
                    };

                    db.Insert(course);

                    var termVar = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                    termVar.courseAmounts = termVar.courseAmounts + 1;

                    db.Update(termVar);

                    await DisplayAlert(null, course.courseName + " Saved", "OK");

                    await Navigation.PushAsync(new CoursesPage(userId, termId));
                }
                else if (courseId != 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    var courseVar = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseVar.courseId = courseId;
                    courseVar.termId = termId;
                    courseVar.courseName = courseNameEntry.Text;
                    courseVar.startCourseDate = startCourseDatePicker.Date;
                    courseVar.endCourseDate = endCourseDatePicker.Date;
                    courseVar.courseStatus = courseStatusPicker.SelectedItem.ToString();
                    courseVar.courseNote = courseNoteEntry.Text;
                    courseVar.instructorName = courseInstructorsNameEntry.Text;
                    courseVar.instructorsEmail = courseInstructorEmailEntry.Text;
                    courseVar.instructorsPhoneNumber = courseInstructorPhoneEntry.Text;

                    db.Update(courseVar);

                    await Navigation.PushAsync(new CoursesPage(userId, termId));
                }
            }
        }

        private async void notificationSaveButton_Clicked(object sender, EventArgs e)
        {
            if (CheckForNull() == 1)
            {
                await DisplayAlert(null, "One or more of the fields are empty or invalid. Check that the end date is after the start date...", "OK");
            }
            else if (IsValidEmail(courseInstructorEmailEntry.Text) == false)
            {
                await DisplayAlert(null, "Please check that your instructors email is valid...", "OK");
            }
            else
            {

                if (courseId == 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    db.CreateTable<Course>();

                    var maxPk = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseId = (maxPk == null ? 1 : maxPk.termId + 1);

                    Course course = new Course()
                    {
                        courseId = courseId,
                        termId = termId,
                        courseName = courseNameEntry.Text,
                        startCourseDate = startCourseDatePicker.Date,
                        endCourseDate = endCourseDatePicker.Date,
                        courseStatus = courseStatusPicker.SelectedItem.ToString(),
                        courseNote = courseNoteEntry.Text,
                        instructorName = courseInstructorsNameEntry.Text,
                        instructorsEmail = courseInstructorEmailEntry.Text,
                        instructorsPhoneNumber = courseInstructorPhoneEntry.Text
                    };

                    db.Insert(course);

                    var termVar = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                    termVar.courseAmounts = termVar.courseAmounts + 1;

                    db.Update(termVar);

                    await DisplayAlert(null, course.courseName + " Saved", "OK");

                    await Navigation.PushAsync(new CreateNotificationPage(userId, termId, courseId, courseOrAssessment));
                }
                else if (courseId != 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    var courseVar = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseVar.courseId = courseId;
                    courseVar.termId = termId;
                    courseVar.courseName = courseNameEntry.Text;
                    courseVar.startCourseDate = startCourseDatePicker.Date;
                    courseVar.endCourseDate = endCourseDatePicker.Date;
                    courseVar.courseStatus = courseStatusPicker.SelectedItem.ToString();
                    courseVar.courseNote = courseNoteEntry.Text;
                    courseVar.instructorName = courseInstructorsNameEntry.Text;
                    courseVar.instructorsEmail = courseInstructorEmailEntry.Text;
                    courseVar.instructorsPhoneNumber = courseInstructorPhoneEntry.Text;

                    db.Update(courseVar);

                    await Navigation.PushAsync(new CreateNotificationPage(userId, termId, courseId, courseOrAssessment));
                }
            }
        }

        private async void sendSaveNoteButton_Clciked(object sender, EventArgs e)
        {
            if (CheckForNull() == 1)
            {
                await DisplayAlert(null, "One or more of the fields are empty or invalid. Check that the end date is after the start date...", "OK");
            }
            else if (IsValidEmail(courseInstructorEmailEntry.Text) == false)
            {
                await DisplayAlert(null, "Please check that your instructors email is valid...", "OK");
            }
            else
            {

                if (courseId == 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    db.CreateTable<Course>();

                    var maxPk = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseId = (maxPk == null ? 1 : maxPk.termId + 1);

                    Course course = new Course()
                    {
                        courseId = courseId,
                        termId = termId,
                        courseName = courseNameEntry.Text,
                        startCourseDate = startCourseDatePicker.Date,
                        endCourseDate = endCourseDatePicker.Date,
                        courseStatus = courseStatusPicker.SelectedItem.ToString(),
                        courseNote = courseNoteEntry.Text,
                        instructorName = courseInstructorsNameEntry.Text,
                        instructorsEmail = courseInstructorEmailEntry.Text,
                        instructorsPhoneNumber = courseInstructorPhoneEntry.Text
                    };

                    db.Insert(course);

                    var termVar = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                    termVar.courseAmounts = termVar.courseAmounts + 1;

                    db.Update(termVar);

                    await DisplayAlert(null, course.courseName + " Saved", "OK");

                    await Navigation.PushAsync(new ShareNotesPage(userId, termId, courseId, course.courseNote));
                }
                else if (courseId != 0)
                {
                    var db = new SQLiteConnection(_dbPath);

                    var courseVar = db.Table<Course>().OrderByDescending(c => c.termId).FirstOrDefault();

                    courseVar.courseId = courseId;
                    courseVar.termId = termId;
                    courseVar.courseName = courseNameEntry.Text;
                    courseVar.startCourseDate = startCourseDatePicker.Date;
                    courseVar.endCourseDate = endCourseDatePicker.Date;
                    courseVar.courseStatus = courseStatusPicker.SelectedItem.ToString();
                    courseVar.courseNote = courseNoteEntry.Text;
                    courseVar.instructorName = courseInstructorsNameEntry.Text;
                    courseVar.instructorsEmail = courseInstructorEmailEntry.Text;
                    courseVar.instructorsPhoneNumber = courseInstructorPhoneEntry.Text;

                    db.Update(courseVar);

                    await Navigation.PushAsync(new ShareNotesPage(userId, termId, courseId, courseVar.courseNote));
                }
            }
        }

        private int CheckForNull()
        {
            int incorrectIsOne = 0;

            if (courseNameEntry.Text == null || endCourseDatePicker.Date <= startCourseDatePicker.Date || courseStatusPicker.SelectedItem == null ||
                courseInstructorsNameEntry.Text == null || courseInstructorEmailEntry.Text == null || courseInstructorPhoneEntry.Text == null || courseNoteEntry.Text == null)
            {
                incorrectIsOne = 1;
            }
           
            return incorrectIsOne;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}