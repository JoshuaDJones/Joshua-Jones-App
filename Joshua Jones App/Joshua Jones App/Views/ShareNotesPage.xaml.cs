using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joshua_Jones_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShareNotesPage : ContentPage
    {
        int userId = 0;
        int termId = 0;
        int courseId = 0;
        string courseNote = null;
        public ShareNotesPage(int uId, int tId, int cId, string cNote)
        {
            InitializeComponent();

            userId = uId;
            termId = tId;
            courseId = cId;
            courseNote = cNote;
        }

        private async void sendSMSButton_Clicked(object sender, EventArgs e)
        {
            if(textMessagePhoneNumberEntry.Text == null)
            {
                await DisplayAlert(null, "You must enter in all fields to send the text...", "OK");
            }
            else
            {
                try
                {
                    SendSms(courseNote, textMessagePhoneNumberEntry.Text);
                }
                catch
                {
                    await DisplayAlert(null, "The phone number provided is invalid...", "OK");
                }
            }
        }

        private async void sendEmailButton_Clicked(object sender, EventArgs e)
        {
            if(emailRecipientEntry.Text == null || emailSubjectEntry.Text == null)
            {
                await DisplayAlert(null, "You must enter in all fields to send the email...", "OK");
            }
            else
            {
                if(IsValidEmail(emailRecipientEntry.Text) == false)
                {
                    await DisplayAlert(null, "You must enter in a valid email...", "OK");
                }
                else
                {
                    List<string> recipient = new List<string>();

                    recipient.Add(emailRecipientEntry.Text);

                    await SendEmail(emailSubjectEntry.Text, courseNote, recipient);
                }
            }
        }

        private async void backToCoursePageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursesPage(userId, termId));
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = recipients,
            };
            await Email.ComposeAsync(message);
        }

        public async Task SendSms(string messageText, string recipient)
        {

            var message = new SmsMessage(messageText, new[] { recipient });
            await Sms.ComposeAsync(message);
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