using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Joshua_Jones_App.Models
{
    public class DataBase
    {
        public class Login
        {
            [PrimaryKey, AutoIncrement]
            public int loginId { get; set; }
            public string loginName { get; set; }
            public string loginPassword { get; set; }
        }
        public class Term
        {
            [PrimaryKey, AutoIncrement]
            public int termId { get; set; }
            public string termName { get; set; }
            public int loginId { get; set; }
            public int courseAmounts { get; set; }
            public DateTime startTermDate { get; set; }
            public DateTime endTermDate { get; set; }

            public override string ToString()
            {
                return this.termName + " Amount of courses: " + this.courseAmounts.ToString() + " Start Date: " +
                    this.startTermDate.ToString("MM-dd-yyyy") + " End Date: " + this.endTermDate.ToString("MM-dd-yyyy");
            }
        }

        public class Course
        {
            [PrimaryKey, AutoIncrement]
            public int courseId { get; set; }
            public int termId { get; set; }
            public string courseName { get; set; }
            public DateTime startCourseDate { get; set; }
            public DateTime endCourseDate { get; set; }
            public string courseStatus { get; set; }
            public string courseNote { get; set; }
            public string instructorName { get; set; }
            public string instructorsEmail { get; set; }
            public string instructorsPhoneNumber { get; set; }
        }

        public class Assessment
        {
            [PrimaryKey, AutoIncrement]
            public int assessmentId { get; set; }
            public int courseId { get; set; }
            public string assessmentName { get; set; }
            public string assessmentType { get; set; }
            public DateTime assessmentDueDate { get; set; }
        }

        public class Notification
        {
            [PrimaryKey, AutoIncrement]
            public int notificationId { get; set; }
            public int userId { get; set; }
            public string notificationTitle { get; set; }
            public string notificationBody { get; set; }
            public DateTime notificationStartDate { get; set; }
        }
    } 
}
