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
using System.Data;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditTerm : ContentPage
    {
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        int termId = 0;
        int userId = 0;
        public AddEditTerm(int id)
        {
            InitializeComponent();
            userId = id;
        }
        public AddEditTerm(int id, int tId)
        {
            InitializeComponent();
            var db = new SQLiteConnection(_dbPath);

            userId = id;
            termId = tId;

            var termVar = db.Table<Term>().FirstOrDefault(c => c.termId == termId);

            termNameEntry.Text = termVar.termName.ToString();
            startDatePicker.Date = termVar.startTermDate;
            endDatePicker.Date = termVar.endTermDate;

        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (termNameEntry.Text == null || startDatePicker.Date >= endDatePicker.Date)
            {
                await DisplayAlert(null, "All fields must have input and the end date must be before the start date...", "OK");
            }
            else
            {
                var db = new SQLiteConnection(_dbPath);

                if (termId == 0)
                {
                    db.CreateTable<Term>();
                    db.CreateTable<Course>();

                    var maxPk = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                    int tId = (maxPk == null ? 1 : maxPk.termId + 1);

                    Term term = new Term()
                    {
                        termId = tId,
                        termName = termNameEntry.Text,
                        loginId = userId,
                        courseAmounts = 0,
                        startTermDate = Convert.ToDateTime(startDatePicker.Date.ToString()),
                        endTermDate = Convert.ToDateTime(endDatePicker.Date.ToString())
                    };

                    db.Insert(term);
                    await DisplayAlert(null, term.termName + " Saved", "OK");
                }
                else if (termId != 0)
                {
                    var termVar = db.Table<Term>().OrderByDescending(c => c.termId).FirstOrDefault();

                    termVar.termName = termNameEntry.Text;
                    termVar.startTermDate = Convert.ToDateTime(startDatePicker.Date.ToString());
                    termVar.endTermDate = Convert.ToDateTime(endDatePicker.Date.ToString());

                    db.Update(termVar);
                }

                await Navigation.PushAsync(new TermPage(userId));
            }
        }
    }
}